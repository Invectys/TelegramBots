using QuestionSysTB.Commands;
using QuestionSysTB.Data;
using QuestionSysTB.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace QuestionSysTB.Services
{
    public class CommandHandler
    {

        Command[] _commands;
        FileDataService _fileDataService;
        BotService _botService;

        public CommandHandler(FileDataService fileDataService,BotService botService)
        {
            _fileDataService = fileDataService;
            _botService = botService;

            RegisterCommands();
        }


        public async Task<CmdHandleResult> Handle(Message message)
        {
            if(message.Text == null)
            {
                return CmdHandleResult.Wrong;
            }

            foreach (var item in _commands)
            {
                if(item.IsMe(message.Text))
                {
                    return await item.CallHandler(message, _fileDataService, _botService);
                }
            }
            return CmdHandleResult.Wrong;
        }


        private void RegisterCommands()
        {
            _commands = new Command[]
            {
                new StartCommand(),
                new SetCommand(),
                new AddCommand(),
                new RemoveCommand(),
                new EditMessageCommand(),
                new AllMessagesCommand(),
                new AdminsCommand(),
                new PayQuestionCommand()
            };
        }
    }
}
