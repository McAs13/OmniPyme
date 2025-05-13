using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using Microsoft.EntityFrameworkCore;
using OmniPyme.Data;
using OmniPyme.Web.Data.Seeders;
using OmniPyme.Web.Helpers;
using OmniPyme.Web.Services;
using Serilog;

namespace OmniPyme.Web
{
    public static class CustomConfiguration
    {
        public static WebApplicationBuilder AddCustomConfiguration(this WebApplicationBuilder builder)
        {
            // Data Context
            builder.Services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection"));
            });

            //AutoMapper
            builder.Services.AddAutoMapper(typeof(Program));

            //Services
            AddServices(builder);

            // Toast Notification SetUp
            builder.Services.AddNotyf(config =>
            {
                config.DurationInSeconds = 10;
                config.IsDismissable = true;
                config.Position = NotyfPosition.BottomRight;
            });

            //Log Setup
            AddLogConfiguration(builder);

            return builder;
        }

        private static void AddLogConfiguration(WebApplicationBuilder builder)
        {
            Log.Logger = new LoggerConfiguration().WriteTo.File("logs/log.log",
                                                                rollingInterval: RollingInterval.Day,
                                                                restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Warning)
                                                  .WriteTo.Console(restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)
                                                  .CreateLogger();
            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog();
        }

        private static void AddServices(WebApplicationBuilder builder)
        {
            //Services
            builder.Services.AddScoped<IClientsService, ClientsService>();
            builder.Services.AddScoped<IRolesService, RolesService>();
            builder.Services.AddScoped<IInvoicesService, InvoicesService>();
            builder.Services.AddScoped<ISalesService, SalesService>();
            builder.Services.AddScoped<ISaleDetailService, SaleDetailService>();
            builder.Services.AddScoped<IProductCategoriesService, ProductCategoriesService>();
            builder.Services.AddScoped<IProductsService, ProductsService>();
            builder.Services.AddTransient<SeedDb>();
            builder.Services.AddTransient<IReadLogsService, ReadPlainTextLogsService>();

            //Helpers
            builder.Services.AddScoped<ICombosHelper, CombosHelper>();
        }

        public static WebApplication AddCustomWebApplicationConfiguration(this WebApplication app)
        {
            app.UseNotyf();

            SeedData(app);

            return app;
        }

        private static void SeedData(WebApplication app)
        {
            IServiceScopeFactory scopeFactory = app.Services.GetService<IServiceScopeFactory>();

            using (IServiceScope scope = scopeFactory.CreateScope())
            {
                SeedDb service = scope.ServiceProvider.GetService<SeedDb>();
                service.SeedAsync().Wait();
            }
        }
    }
}
