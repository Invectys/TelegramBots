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
    public class StartCommand : Command
    {

        public override bool NeedAdmin { get => false; }
        protected override string Text { get => "/start"; }

        protected override async Task<UserState> Handle(Message msg, FileDataService fileDataService, BotService botService)
        {
            var messages = fileDataService.GetMessages();
            await botService.Client.SendTextMessageAsync(msg.Chat.Id, messages.Messages[DefaultMessagesKeys.AllKeys[0]]);

            return new DefaultState(fileDataService, botService);
        }
    }
}
