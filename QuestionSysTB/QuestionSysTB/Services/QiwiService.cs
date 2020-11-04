using Microsoft.Extensions.Options;
using Qiwi.BillPayments.Client;
using QuestionSysTB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuestionSysTB.Services
{
    public class QiwiService
    {
        public BillPaymentsClient Client { get; private set; }
        IOptions<PaymentsSettings> _options;

        public QiwiService(IOptions<PaymentsSettings> options)
        {
            _options = options;
        }

        public void StartClient()
        {
            Client = BillPaymentsClientFactory.Create(
                secretKey: _options.Value.QiwiSecret
            );
        }

    }
}
