using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using QuestionSysTB.Data;
using QuestionSysTB.Models;
using QuestionSysTB.Services;

namespace QuestionSysTB
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                    .AddNewtonsoftJson();
            services.AddSingleton<BotService>();
            services.AddSingleton<CommandHandler>();
            services.AddSingleton<CallbackQueryHandlerService>();
            services.AddSingleton<FileDataService>();
            services.AddSingleton<UsersStateMachineService>();
            services.AddSingleton<ReactionService>();

            services.AddSingleton<QiwiService>();
            services.AddSingleton<PaymentsService>();

            services.Configure<BotSettings>(Configuration);
            services.Configure<PaymentsSettings>(Configuration);

            services.AddDbContext<ApplicationDbContext>(opt =>
            {
                opt.UseSqlite("Data Source=applicationDatabase.db;",opt=> { });
            },ServiceLifetime.Singleton);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseRouting();
            app.UseCors();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
