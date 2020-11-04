using Microsoft.Extensions.Configuration;
using QuestionSysTB.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuestionSysTB.Commands
{
    public class CmdHandleResult
    {
        public static CmdHandleResult Wrong => new CmdHandleResult();

        public bool WasExecuted { get; set; } = false;
        public UserState NewState { get; set; }
    }
}
