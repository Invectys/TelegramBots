using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using QuestionSysTB.Services;

namespace QuestionSysTB
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            var bot = host.Services.GetService<BotService>();
            var payments = host.Services.GetService<PaymentsService>();
            bot.StartClient().Wait();
            payments.StartClients().Wait();
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.ConfigureKestrel(opt =>
                    {
                        opt.ConfigureHttpsDefaults(opt =>
                        {
                            opt.ServerCertificate = new X509Certificate2("certificate.pfx", "qzwsecrftb1");
                        });
                    }).UseUrls(/*"https://127.0.0.1:443", "http://127.0.0.1:5001"*/);
                });
    }
}
