using QuestionSysTB.Data;
using QuestionSysTB.Markup;
using QuestionSysTB.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace QuestionSysTB.States
{
    public class WaitPayQuestion : UserState
    {
        public WaitPayQuestion(FileDataService fileDataService, BotService botService) : base(fileDataService, botService)
        {
        }

        public override async Task<UserState> HandleTextMessage(Message msg, CommandHandler commandHandler)
        {
            var text = msg.Text;

            var data = (DataModel)FileDataService.Get<DefaultDataSource>().Get();
            var messages = FileDataService.GetMessages();
            var moderId = data.ModerationId;

            //send to moderation
            var m = InlineKeyboards.GetModerationMarkupPayQuestion(msg.From.Username, msg.Chat.Id);
            await BotService.Client.SendTextMessageAsync(moderId, text, replyMarkup: m);

            //send to user
            await BotService.Client.SendTextMessageAsync(msg.Chat.Id, messages.Messages[DefaultMessagesKeys.AllKeys[5]]);


            return new DefaultState(FileDataService,BotService);
        }
    }
}
