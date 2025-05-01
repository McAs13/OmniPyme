using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OmniPyme.Data;
using OmniPyme.Web.Data.Entities;
using OmniPyme.Web.Data.Seeders;
using OmniPyme.Web.Helpers;
using OmniPyme.Web.Services;

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

            //Identoty an Acces Managment
            addIAM(builder);

            // Toast Notification SetUp
            builder.Services.AddNotyf(config =>
            {
                config.DurationInSeconds = 10;
                config.IsDismissable = true;
                config.Position = NotyfPosition.BottomRight;
            });

            //Log setup 
           // AddLogConfiguration(builder);

            return builder;
        }

        //private static void AddLogConfiguration(WebApplicationBuilder builder)
        //{
         //   throw new NotImplementedException();
        //}

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

        private static void addIAM(WebApplicationBuilder builder)
        {
            builder.Services.AddIdentity<Users, Role>(conf =>
            {
                conf.User.RequireUniqueEmail = true;
                conf.Password.RequireDigit = false;
                conf.Password.RequiredUniqueChars = 0;
                conf.Password.RequireLowercase = false;
                conf.Password.RequireUppercase = false;
                conf.Password.RequireNonAlphanumeric = false;
                conf.Password.RequiredLength = 4;
            }).AddEntityFrameworkStores<DataContext>()
              .AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie( conf=>
            {
                conf.Cookie.Name = "Auth";
                conf.ExpireTimeSpan = TimeSpan.FromDays(100);
                conf.LoginPath = "/Account/Login";
                conf.AccessDeniedPath = "/Account/NotAuthorized";
            
            });
                
        }
    }
}
