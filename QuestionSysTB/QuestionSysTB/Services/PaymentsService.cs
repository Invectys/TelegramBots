using Microsoft.Extensions.Options;
using Qiwi.BillPayments.Model;
using Qiwi.BillPayments.Model.In;
using QuestionSysTB.Data;
using QuestionSysTB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuestionSysTB.Services
{
    public class PaymentsService
    {
        QiwiService _qiwiService;
        ApplicationDbContext _dbContext;
        IOptions<PaymentsSettings> _options;
        public PaymentsService(QiwiService qiwiService,ApplicationDbContext dbContext,IOptions<PaymentsSettings> options)
        {
            _qiwiService = qiwiService;
            _dbContext = dbContext;
            _options = options;
        }

        public async Task StartClients()
        {
            _qiwiService.StartClient();
        }

        public async Task<string> CreatePayment(string username,int messageQuestionId)
        {

            var paymentInfo = new CreateBillInfo()
            {
                Amount = new MoneyAmount()
                {
                    CurrencyEnum = CurrencyEnum.Rub,
                    ValueDecimal = 1
                },
                BillId = Guid.NewGuid().ToString(),
                SuccessUrl = new Uri(_options.Value.SuccessUrl),
                ExpirationDateTime = DateTime.Now.AddHours(_options.Value.ExpirationBillInHours)
            };

            var bill = await _qiwiService.Client.CreateBillAsync(paymentInfo);

            await SavePaymentInfo(new ApplicationBill()
            {
                BillId = paymentInfo.BillId,
                ExpirationTime = paymentInfo.ExpirationDateTime,
                MessagesId = messageQuestionId,
                Username = username
            });

            return bill.PayUrl.ToString();
        }

        private async Task SavePaymentInfo(ApplicationBill bill)
        {
            _dbContext.Bills.Add(bill);
            await _dbContext.SaveChangesAsync();
        }


        private void StartCheckingPayments()
        {
            Console.WriteLine("Start");
            var period = TimeSpan.FromMinutes(_options.Value.CheckPaymentTimeMinutes);
            var timer = new System.Threading.Timer((e) =>
            {
                CheckPayments();
            },null,TimeSpan.Zero, period);
        }
        private void CheckPayments()
        {
            Console.WriteLine("check");
        }

    }
}
