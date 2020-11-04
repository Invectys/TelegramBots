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
    public abstract class Command
    {
        public virtual bool NeedAdmin { get;} = true;
        protected virtual List<ChatType> avaliableChats { get; set; } = new List<ChatType>() { ChatType.Private };
        protected abstract string Text { get; }

        public async Task<CmdHandleResult> CallHandler(Message message,FileDataService fileDataService,BotService botService)
        {
            var from = message.From;
            if(!avaliableChats.Contains(message.Chat.Type))
            {
                return CmdHandleResult.Wrong;
            }
            if(from == null)
            {
                return CmdHandleResult.Wrong;
            }
            if(from.IsBot)
            {
                return CmdHandleResult.Wrong;
            }
            if(NeedAdmin)
            {
                DataModel data = (DataModel)fileDataService.Get<DefaultDataSource>().Get();
                bool isAdmin = data.Admins.Contains(from.Username);
                if (!isAdmin)
                    return null;
            }

            var state = await Handle(message, fileDataService, botService);
            return new CmdHandleResult() { NewState = state, WasExecuted = true };
        }
        public bool IsMe(string msg)
        {
            return msg.StartsWith(Text);
        }

        protected abstract Task<UserState> Handle(Message msg, FileDataService fileDataService, BotService botService);
       
    }
}
