using QuestionSysTB.Commands;
using QuestionSysTB.Data;
using QuestionSysTB.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace QuestionSysTB.CallbackQuerys
{
    public abstract class CallbackQueryCommand
    {
        protected abstract string Text { get; }

        public async Task CallHandler(CallbackQuery callbackQuery, FileDataService fileDataService, BotService botService)
        {

            await Handle(callbackQuery, fileDataService, botService);
        }
        public bool IsMe(string data)
        {
            var key = data.Split("|")[0];
            return key == Text;
        }

        protected abstract Task Handle(CallbackQuery callbackQuery, FileDataService fileDataService, BotService botService);
    }
}
