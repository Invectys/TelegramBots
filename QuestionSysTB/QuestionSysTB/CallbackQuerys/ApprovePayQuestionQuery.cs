using Qiwi.BillPayments.Model;
using Qiwi.BillPayments.Model.In;
using QuestionSysTB.Data;
using QuestionSysTB.Markup;
using QuestionSysTB.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace QuestionSysTB.CallbackQuerys
{
    public class ApprovePayQuestionQuery : CallbackQueryCommand
    {
        PaymentsService _paymentsService;
        public ApprovePayQuestionQuery(PaymentsService paymentsService)
        {
            _paymentsService = paymentsService;
        }

        protected override string Text => "approvepay";

        protected override async Task Handle(CallbackQuery callbackQuery, FileDataService fileDataService, BotService botService)
        {
            var messageId = callbackQuery.Message.MessageId;
            string[] args = callbackQuery.Data.Split("|");

            var fromChat = long.Parse(args[1]);
            var user = args[2];

            var messages = fileDataService.GetMessages();

            var payUrl = await _paymentsService.CreatePayment(user, messageId);

            await botService.Client.SendTextMessageAsync(fromChat, payUrl);


            var m = InlineKeyboards.GetWaitPayment(user, fromChat);
            await botService.Client.EditMessageReplyMarkupAsync(callbackQuery.Message.Chat.Id, messageId,replyMarkup: m);
        }
    }
}
