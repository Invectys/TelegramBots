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
    public abstract class MultipleChoiceCommand : Command
    {
        protected override string Text => "";

        For[] Fors;
        public MultipleChoiceCommand()
        {
            Fors = InitForHandlers();
        }

        protected abstract For[] InitForHandlers();

        protected override sealed async Task<UserState> Handle(Message msg, FileDataService fileDataService, BotService botService)
        {
            string text = msg.Text;
            
            foreach (var item in Fors)
            {
                var parser = new CommandLineParser(item.ArgsCount);
                bool ok = parser.Parse(text, out string[] args);
                if (!ok)
                {
                    continue;
                }

                if (item.IsFor(args[1]))
                {
                    await item.Handle(args, msg, fileDataService, botService);
                    return null;
                }
            }

            botService.SendWrongFormat(msg.Chat.Id);
            return null;
        }

        protected abstract class For
        {
            public virtual int ArgsCount { get; } = 3;
            protected abstract string ForName { get; }

            public bool IsFor(string arg) => arg == ForName;

            public abstract Task Handle(string[] args,Message msg, FileDataService fileDataService, BotService botService);
        }

        
    }
}
