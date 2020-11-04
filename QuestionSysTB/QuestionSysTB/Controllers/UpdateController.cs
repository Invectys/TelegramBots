using Microsoft.AspNetCore.Mvc;
using QuestionSysTB.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace QuestionSysTB.Controllers
{
    [ApiController]
    public class UpdateController : ControllerBase
    {
        CommandHandler _commandHandler;
        CallbackQueryHandlerService _callbackQueryhandler;
        UsersStateMachineService _usersStateMachineService;
        public UpdateController(CommandHandler commandHandler,
            CallbackQueryHandlerService callbackQueryhandler,
            UsersStateMachineService usersStateMachineService)
        {
            _commandHandler = commandHandler;
            _callbackQueryhandler = callbackQueryhandler;
            _usersStateMachineService = usersStateMachineService;
        }

        [Route(DefConfig.ApiUpdatePath)]
        public async Task ReceiveUpdate([FromBody]Update update)
        {

            try
            {
                if (update.Type == UpdateType.Message)
                {
                    await _usersStateMachineService.HandleUpdate(update);
                    return;
                }
                if(update.Type == UpdateType.CallbackQuery)
                {
                    await _callbackQueryhandler.Handle(update.CallbackQuery);
                    return;
                }
            }catch(Exception e)
            {
                Console.WriteLine("Exception");
            }


        }
    }
}
