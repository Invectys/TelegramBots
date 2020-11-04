﻿using QuestionSysTB.Data;
using QuestionSysTB.Services;
using QuestionSysTB.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace QuestionSysTB.Commands
{
    public class PayQuestionCommand : Command
    {
        public override bool NeedAdmin => false;
        protected override string Text => DefaultMessages.PayQuestion;

        protected override async Task<UserState> Handle(Message msg, FileDataService fileDataService, BotService botService)
        {
            var messages = fileDataService.GetMessages();
            await botService.Client.SendTextMessageAsync(msg.Chat.Id, messages.Messages[DefaultMessagesKeys.AllKeys[4]],
                replyMarkup: new ReplyKeyboardRemove());

            return new WaitPayQuestion(fileDataService, botService);
        }
    }
}
