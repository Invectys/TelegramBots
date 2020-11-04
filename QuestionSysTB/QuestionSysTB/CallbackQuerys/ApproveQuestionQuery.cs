using QuestionSysTB.Data;
using QuestionSysTB.Markup;
using QuestionSysTB.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace QuestionSysTB.CallbackQuerys
{
    public class ApproveQuestionQuery : CallbackQueryCommand
    {

        protected override string Text => "approve";

        protected override async Task Handle(CallbackQuery callbackQuery, FileDataService fileDataService, BotService botService)
        {

            var message = callbackQuery.Message;
            var source = fileDataService.Get<DefaultDataSource>();
            var messagesSource = fileDataService.Get<MessagesDataSource>();
            DataModel data = (DataModel)source.Get();
            MessagesModel messages = (MessagesModel)messagesSource.Get();

            string questionText = callbackQuery.Message.Text;

            foreach (var item in data.DiscussionList)
            {
                //send to discussion
                var m1 = InlineKeyboards.LinkToChannel(DefConfig.LinkToChannel);
                await botService.Client.SendTextMessageAsync(item, messages.Messages[DefaultMessagesKeys.AllKeys[1]],
                    replyMarkup: m1);
                await botService.Client.SendTextMessageAsync(item, questionText);
            }

            foreach (var item in data.PublishChannelList)
            {
                var m = InlineKeyboards.GetReactionKeyboard(0, 0);

                //send to publish channel
                await botService.Client.SendTextMessageAsync(item, questionText);
            }


            //remove from moderation
            await botService.Client.DeleteMessageAsync(message.Chat.Id, message.MessageId);
        }
    }
}
