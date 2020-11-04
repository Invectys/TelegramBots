using QuestionSysTB.Commands;
using QuestionSysTB.Data;
using QuestionSysTB.Markup;
using QuestionSysTB.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace QuestionSysTB.States
{
    public class DefaultState : UserState
    {
        public DefaultState(FileDataService fileDataService, BotService botService) : base(fileDataService, botService)
        {
        }

        public override async Task<UserState> HandleTextMessage(Message msg, CommandHandler commandHandler)
        {
            var result = await commandHandler.Handle(msg);
            if(!result.WasExecuted)
            {
                result = await new NewQuestionReceiveCommand().CallHandler(msg, FileDataService, BotService);
            }
            return result.NewState;
        }

        public override async Task InitState(Message msg)
        {
            if(false)
            {
                var m = Keyboards.QuestionTypekeyboard();
                await BotService.Client.SendTextMessageAsync(msg.Chat.Id, "-", replyMarkup: m);
            }
        }

    }
}
