using QuestionSysTB.Data;
using QuestionSysTB.Services;
using QuestionSysTB.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace QuestionSysTB.Commands
{
    public class AllMessagesCommand : Command
    {
        protected override string Text => "/messages";

        protected override async Task<UserState> Handle(Message msg, FileDataService fileDataService, BotService botService)
        {
            string str = "";
            for(int i=0; i < DefaultMessagesKeys.AllKeys.Length; i++)
            {
                str += "*" + DefaultMessagesKeys.AllKeys[i] + "* " + DefaultMessages.AllMessages[i] + "\n";
            }

            await botService.Client.SendTextMessageAsync(msg.Chat.Id, str,parseMode:ParseMode.Markdown);
            return null;
        }
    }
}
