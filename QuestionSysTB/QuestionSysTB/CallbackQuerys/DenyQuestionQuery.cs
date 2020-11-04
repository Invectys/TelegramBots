using QuestionSysTB.Data;
using QuestionSysTB.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace QuestionSysTB.CallbackQuerys
{
    public class DenyQuestionQuery : CallbackQueryCommand
    {
        protected override string Text => "deny";

        protected override async Task Handle(CallbackQuery callbackQuery, FileDataService fileDataService, BotService botService)
        {
            var message = callbackQuery.Message;
            var source = fileDataService.Get<DefaultDataSource>();
            var messagesSource = fileDataService.Get<MessagesDataSource>();
            DataModel data = (DataModel)source.Get();
            MessagesModel messages = (MessagesModel)messagesSource.Get();

            long toId = long.Parse(callbackQuery.Data.Split("|")[1]);

            var msg = messages.Messages[DefaultMessagesKeys.AllKeys[2]];
            await botService.Client.SendTextMessageAsync(toId, msg);

            //remove from moderation
            await botService.Client.DeleteMessageAsync(message.Chat.Id, message.MessageId);

        }
    }
}
