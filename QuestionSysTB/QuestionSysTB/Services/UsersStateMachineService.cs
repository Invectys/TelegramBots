using QuestionSysTB.Data;
using QuestionSysTB.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace QuestionSysTB.Services
{
    public class UsersStateMachineService
    {

        public Dictionary<string, UserState> _states = new Dictionary<string, UserState>();
        CommandHandler _commandHandler;
        FileDataService _fileDataService;
        BotService _botService;

        public UsersStateMachineService(CommandHandler commandHandler,
            FileDataService fileDataService,BotService botService)
        {
            _commandHandler = commandHandler;
            _fileDataService = fileDataService;
            _botService = botService;
        }

        public async Task HandleUpdate(Update update)
        {
     
            var from = update.Message.From;
            if (from == null)
                return;

            UserState state = null;
            bool haveState = _states.ContainsKey(from.Username);
            if(haveState)
            {
                state = _states[from.Username];
            }
            else
            {
                state = new DefaultState(_fileDataService, _botService);
            }

            var newState = await state.HandleTextMessage(update.Message, _commandHandler);

            //Replace or add state
            _states.Remove(from.Username);
            if(newState != null)
            {
                _states.Add(from.Username, newState);
                await newState.InitState(update.Message);
            }
            
        }

    }
}
