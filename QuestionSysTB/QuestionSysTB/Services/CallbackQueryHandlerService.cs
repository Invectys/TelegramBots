using QuestionSysTB.CallbackQuerys;
using QuestionSysTB.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace QuestionSysTB.Services
{
    public class CallbackQueryHandlerService
    {

        CallbackQueryCommand[] queryCommands;
        FileDataService _fileDataService;
        BotService _botService;
        ReactionService _reactionService;
        PaymentsService _paymentsService;

        public CallbackQueryHandlerService(FileDataService fileDataService, BotService botService
            ,PaymentsService paymentsService,ReactionService reactionService)
        {
            _fileDataService = fileDataService;
            _botService = botService;
            _paymentsService = paymentsService;
            _reactionService = reactionService;
            
            RegisterQueryCommands();

        }

        public async Task Handle(CallbackQuery query)
        {
            foreach (var item in queryCommands)
            {
                if (item.IsMe(query.Data))
                {
                    await item.CallHandler(query, _fileDataService, _botService);
                    return;
                }
            }
        }


        private void RegisterQueryCommands()
        {
            queryCommands = new CallbackQueryCommand[]
            {
                new ApproveQuestionQuery(),
                new DenyQuestionQuery(),
                new ApprovePayQuestionQuery(_paymentsService),
                new DenyPayQuestionQuery(),
                new LikeQuery(_reactionService),
                new DislikeQuery(_reactionService)
            };
        }

    }
}
