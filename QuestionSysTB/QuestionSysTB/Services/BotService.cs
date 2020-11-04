using Microsoft.Extensions.Options;
using QuestionSysTB.Data;
using QuestionSysTB.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;

namespace QuestionSysTB.Services
{
    public class BotService
    {
        public TelegramBotClient Client;

        IOptions<BotSettings> _settings;
        string _updateApiPath = DefConfig.ApiUpdatePath;

        public BotService(IOptions<BotSettings> settings)
        {
            _settings = settings;
        }

        public async Task StartClient()
        {
            Client = new TelegramBotClient(_settings.Value.BotToken);
            if(_settings.Value.WebHook)
            {
                if(_settings.Value.UseCertificate)
                {
                    using (Stream stream = System.IO.File.OpenRead("YOURPUBLIC.pem"))
                    {
                        var pubkey = new InputFileStream(stream);
                        await Client.SetWebhookAsync(_settings.Value.Domain + _updateApiPath, pubkey);
                    }
                }
                else
                {
                    await Client.SetWebhookAsync(_settings.Value.Domain + _updateApiPath);
                }
            }
        }

    }

    public static class BotServiceExtension
    {
        public static void SendWrongFormat(this BotService botService,long id)
        {
            botService.Client.SendTextMessageAsync(id, DefaultMessages.WrongFormat);
        }

    }

}
