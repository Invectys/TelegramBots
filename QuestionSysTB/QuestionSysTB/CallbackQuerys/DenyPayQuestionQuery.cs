using QuestionSysTB.Data;
using QuestionSysTB.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace QuestionSysTB.CallbackQuerys
{
    public class DenyPayQuestionQuery : CallbackQueryCommand
    {
        protected override string Text => "denypay";

        protected override async Task Handle(CallbackQuery callbackQuery, FileDataService fileDataService, BotService botService)
        {
            var messageId = callbackQuery.Message.MessageId;
            var fromChat = long.Parse(callbackQuery.Data.Split("|")[1]);

            var messages = fileDataService.GetMessages();

            await botService.Client.SendTextMessageAsync(fromChat, messages.Messages[DefaultMessagesKeys.AllKeys[6]]);

            await botService.Client.DeleteMessageAsync(callbackQuery.Message.Chat.Id, messageId);
        }
    }
}
