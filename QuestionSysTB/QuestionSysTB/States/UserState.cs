using QuestionSysTB.Data;
using QuestionSysTB.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace QuestionSysTB.States
{
    public abstract class UserState
    {

        public FileDataService FileDataService { get { return _fileDataService; } }
        public BotService BotService { get { return _botService; } }

        private FileDataService _fileDataService;
        private BotService _botService;

        public UserState(FileDataService fileDataService,BotService botService)
        {
            _fileDataService = fileDataService;
            _botService = botService;
        }

        public abstract Task<UserState> HandleTextMessage(Message msg,CommandHandler commandHandler);
        public virtual Task InitState(Message msg)
        {
            return Task.CompletedTask;
        }
    }
}
