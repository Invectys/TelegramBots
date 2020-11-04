using QuestionSysTB.Data;
using QuestionSysTB.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace QuestionSysTB.Commands
{
    public class RemoveCommand : MultipleChoiceCommand
    {
        protected override string Text => "/remove";

        protected override For[] InitForHandlers()
        {
            return new For[]
            {
                new Moderation(),
                new Discussion(),
                new PublishChannel(),
                new All(),
                new PublAll(),
                new DiscAll(),
                new Admin()
            };
        }

        private class Moderation : For
        {
            protected override string ForName => "moderation";

            public override async Task Handle(string[] args, Message msg, FileDataService fileDataService, BotService botService)
            {
                var source = fileDataService.Get<DefaultDataSource>();
                var model = (DataModel)source.Get();
                model.ModerationId = -1;
                source.Save();
                await botService.Client.SendTextMessageAsync(msg.Chat.Id, DefaultMessages.ModerationRemoved);

            }
        }
        private class Discussion : For
        {
            protected override string ForName => "discussion";

            public override async Task Handle(string[] args, Message msg, FileDataService fileDataService, BotService botService)
            {
                var source = fileDataService.Get<DefaultDataSource>();
                var model = (DataModel)source.Get();
                long id = long.Parse(args[2]);
                bool deleted = model.DiscussionList.Remove(id);
                if(deleted)
                {
                    await botService.Client.SendTextMessageAsync(msg.Chat.Id, DefaultMessages.DiscRemoved);
                    source.Save();
                }

            }
        }
        private class PublishChannel : For
        {
            protected override string ForName => "channel";

            public override async Task Handle(string[] args, Message msg, FileDataService fileDataService, BotService botService)
            {
                var source = fileDataService.Get<DefaultDataSource>();
                var model = (DataModel)source.Get();
                long id = long.Parse(args[2]);
                bool deleted = model.DiscussionList.Remove(id);
                if(deleted)
                {
                    await botService.Client.SendTextMessageAsync(msg.Chat.Id, DefaultMessages.PublRemoved);
                    source.Save();
                }

            }
        }
        private class All : For
        {
            public override int ArgsCount => 2;
            protected override string ForName => "all";

            public override async Task Handle(string[] args, Message msg, FileDataService fileDataService, BotService botService)
            {
                var source = fileDataService.Get<DefaultDataSource>();
                var model = (DataModel)source.Get();
                model.PublishChannelList.Clear();
                model.ModerationId = -1;
                model.DiscussionList.Clear();
                source.Save();
                await botService.Client.SendTextMessageAsync(msg.Chat.Id, DefaultMessages.AllRemoved);
            }
        }
        private class PublAll : For
        {
            protected override string ForName => "allpublish";

            public override async Task Handle(string[] args, Message msg, FileDataService fileDataService, BotService botService)
            {
                var source = fileDataService.Get<DefaultDataSource>();
                var model = (DataModel)source.Get();
                model.PublishChannelList.Clear();
                source.Save();
                await botService.Client.SendTextMessageAsync(msg.Chat.Id, DefaultMessages.AllPublRemoved);

            }
        }
        private class DiscAll : For
        {
            protected override string ForName => "alldiscussion";

            public override async Task Handle(string[] args, Message msg, FileDataService fileDataService, BotService botService)
            {
                var source = fileDataService.Get<DefaultDataSource>();
                var model = (DataModel)source.Get();
                model.DiscussionList.Clear();
                source.Save();
                await botService.Client.SendTextMessageAsync(msg.Chat.Id, DefaultMessages.AllDiscRemoved);
            }
        }
        private class Admin : For
        {
            protected override string ForName => "admin";

            public override async Task Handle(string[] args, Message msg, FileDataService fileDataService, BotService botService)
            {
                var source = fileDataService.Get<DefaultDataSource>();
                var model = (DataModel)source.Get();
                bool removed = model.Admins.Remove(args[2]);
                if(removed)
                {
                    source.Save();
                    await botService.Client.SendTextMessageAsync(msg.Chat.Id, DefaultMessages.AdminRemoved);
                }

            }
        }
    }
}
