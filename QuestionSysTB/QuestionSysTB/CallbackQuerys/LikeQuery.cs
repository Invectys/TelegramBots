using QuestionSysTB.Data;
using QuestionSysTB.Markup;
using QuestionSysTB.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace QuestionSysTB.CallbackQuerys
{
    public class LikeQuery : CallbackQueryCommand
    {
        ReactionService _reactionService;
        public LikeQuery(ReactionService reactionService)
        {
            _reactionService = reactionService;
        }
        protected override string Text => "like";

        protected override async Task Handle(CallbackQuery callbackQuery, FileDataService fileDataService, BotService botService)
        {
            var msgId = callbackQuery.Message.MessageId;
            var chatId = callbackQuery.Message.Chat.Id;

            await _reactionService.SetReaction(callbackQuery.From.Username, msgId, chatId, 1);

            var reactions = await _reactionService.GetReactions(msgId, chatId);
            var m = InlineKeyboards.GetReactionKeyboard(reactions[1], reactions[0]);
            await botService.Client.EditMessageReplyMarkupAsync(chatId, msgId, replyMarkup: m);
        }
    }
}
