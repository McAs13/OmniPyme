﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OmniPyme.Data;
using OmniPyme.Web;
using OmniPyme.Web.Data.Entities; // Asegúrate de importar esto


WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//data context
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection"));
});


builder.AddCustomConfiguration();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{ 
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();

app.UseStatusCodePagesWithReExecute("/Errors/{0}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.AddCustomWebApplicationConfiguration();

// Ejecutar Seeder
/*
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<Users>>();
    var roleManager = services.GetRequiredService<RoleManager<Role>>();

    await DataSeeder.SeedAsync(userManager, roleManager);
}
*/

app.Run();
