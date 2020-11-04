using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuestionSysTB.Models
{
    public class BotSettings
    {
        public string BotToken { get; set; }
        public bool WebHook { get; set; }
        public bool UseCertificate { get; set; } = false;
        public bool UsePayQuestion { get; set; } = false;
        public string Domain { get; set; }

    }
}
