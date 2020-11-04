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
    public class SetCommand : Command
    {
        protected override string Text => "/set";

        SetFor[] setFors;
        public SetCommand()
        {
            setFors = new SetFor[]
            {
                new Moderation()
            };
        }

        protected override async Task<UserState> Handle(Message msg, FileDataService fileDataService, BotService botService)
        {
            string text = msg.Text;
            var parser = new CommandLineParser(3);
            bool ok = parser.Parse(text, out string[] args);
            if(!ok)
            {
                botService.SendWrongFormat(msg.Chat.Id);
                return null;
            }

            foreach (var item in setFors)
            {
                if(item.IsFor(args[1]))
                {
                    item.Handle(args[2], fileDataService);
                    return null;
                }
            }
            return null;
        }

        private abstract class SetFor
        {
            protected abstract string ForName { get; }

            public bool IsFor(string arg) => arg == ForName;

            public abstract void Handle(string param,FileDataService fileDataService);
        }

        private class Moderation : SetFor
        {
            protected override string ForName => "moderation";

            public override void Handle(string param, FileDataService fileDataService)
            {
                var source = fileDataService.Get<DefaultDataSource>();
                var model = (DataModel)source.Get();
                var id = long.Parse(param);
                model.ModerationId = id;
                source.Save();
            }
        }
    }

}
