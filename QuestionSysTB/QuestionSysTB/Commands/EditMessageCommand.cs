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
    public class EditMessageCommand : Command
    {
        protected override string Text => "/edit";

        public EditMessageCommand()
        {
           
        }

        protected override async Task<UserState> Handle(Message msg, FileDataService fileDataService, BotService botService)
        {
            string text = msg.Text;
            var parser = new CommandLineParser(3);
            bool ok = parser.Parse(text, out string[] args);
            if (!ok)
            {
                botService.SendWrongFormat(msg.Chat.Id);
                return null;
            }
            string key = args[1];
            string newMsg = args[2];

            var source = fileDataService.Get<MessagesDataSource>();
            var model = (MessagesModel)source.Get();
            if(DefaultMessagesKeys.AllKeys.Contains(key))
            {
                model.Messages[key] = newMsg;
                await botService.Client.SendTextMessageAsync(msg.Chat.Id, DefaultMessages.MessageEdited);
                source.Save();
                return null;
            }
            

            botService.SendWrongFormat(msg.Chat.Id);
            return null;
        }

       
    }
}
