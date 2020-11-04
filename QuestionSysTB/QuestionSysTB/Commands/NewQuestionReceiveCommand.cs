using QuestionSysTB.Data;
using QuestionSysTB.Markup;
using QuestionSysTB.Services;
using QuestionSysTB.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace QuestionSysTB.Commands
{
    public class NewQuestionReceiveCommand : Command
    {
        public override bool NeedAdmin => false;

        protected override string Text => "";

        protected override async Task<UserState> Handle(Message msg, FileDataService fileDataService, BotService botService)
        {
            DataModel data = (DataModel)fileDataService.Get<DefaultDataSource>().Get();
            long moderationId = data.ModerationId;
            if (moderationId == -1)
            {
                return null;
            }

            var moderM = InlineKeyboards.GetModerationMarkup(msg.From.Username, msg.Chat.Id);
            await botService.Client.SendTextMessageAsync(new ChatId(moderationId), msg.Text, replyMarkup: moderM);

            var messages = fileDataService.GetMessages();

            await botService.Client.SendTextMessageAsync(msg.Chat.Id, messages.Messages[DefaultMessagesKeys.AllKeys[3]]);
            return null;
        }
    }
}
