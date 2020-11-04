using QuestionSysTB.Data;
using QuestionSysTB.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace QuestionSysTB.Commands
{
    public class AddCommand : MultipleChoiceCommand
    {
        protected override string Text => "/add";

        protected override For[] InitForHandlers()
        {
            return new For[]
            {
                new Discussion(),
                new PublishChannel(),
                new Admin()
            };
        }

        private class Discussion : For
        {
            protected override string ForName => "discussion";
            public override async Task Handle(string[] args, Message msg, FileDataService fileDataService, BotService botService)
            {
                var source = fileDataService.Get<DefaultDataSource>();
                var model = (DataModel)source.Get();
                var id = long.Parse(args[2]);
                bool added = model.DiscussionList.Add(id);
                source.Save();
                if (added)
                    await botService.Client.SendTextMessageAsync(msg.Chat.Id, DefaultMessages.DiscAdded);
                else
                    await botService.Client.SendTextMessageAsync(msg.Chat.Id, DefaultMessages.AlreadyDiscAdded);
            }
        }
        private class PublishChannel : For
        {
            protected override string ForName => "channel";
            public override async Task Handle(string[] args, Message msg, FileDataService fileDataService, BotService botService)
            {
                var source = fileDataService.Get<DefaultDataSource>();
                var model = (DataModel)source.Get();
                var id = long.Parse(args[2]);
                bool added = model.PublishChannelList.Add(id);
                source.Save();
                if (added)
                    await botService.Client.SendTextMessageAsync(msg.Chat.Id, DefaultMessages.PublishAdded);
                else
                    await botService.Client.SendTextMessageAsync(msg.Chat.Id, DefaultMessages.AlreadyPublishAdded);

            }
        }
        private class Admin : For
        {
            protected override string ForName => "admin";
            public override async Task Handle(string[] args, Message msg, FileDataService fileDataService, BotService botService)
            {
                var source = fileDataService.Get<DefaultDataSource>();
                var model = (DataModel)source.Get();
                bool added = model.Admins.Add(args[2]);
                source.Save();
                if (added)
                    await botService.Client.SendTextMessageAsync(msg.Chat.Id, DefaultMessages.AdminAdded);
                else
                    await botService.Client.SendTextMessageAsync(msg.Chat.Id, DefaultMessages.AdminAlreadyAdded);

            }
        }
    }
}
