using QuestionSysTB.Data;
using QuestionSysTB.Services;
using QuestionSysTB.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace QuestionSysTB.Commands
{
    public class AdminsCommand : Command
    {
        protected override string Text => "/admins";

        protected override async Task<UserState> Handle(Message msg, FileDataService fileDataService, BotService botService)
        {
            var data = (DataModel)fileDataService.Get<DefaultDataSource>().Get();

            string str = "Весь список админов\n";
            foreach (var item in data.Admins)
            {
                str += item + "\n";
            }

            await botService.Client.SendTextMessageAsync(msg.Chat.Id, str);
            return null;
        }
    }
}
