using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuestionSysTB.Models
{
    public class PaymentsSettings
    {
        public int Cost { get; set; } = 1;
        public string SuccessUrl { get; set; }
        public string QiwiSecret { get; set; }
        public string QiwiPublic { get; set; }
        public int ExpirationBillInHours { get; set; }
        public int CheckPaymentTimeMinutes { get; set; }
    }
}
