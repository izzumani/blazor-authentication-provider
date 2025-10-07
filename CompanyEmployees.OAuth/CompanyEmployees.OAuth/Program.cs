using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompanyEmployees.OAuth.Configuration;
using CompanyEmployees.OAuth.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Get migration assembly
var migrationAssembly = typeof(Program).Assembly.GetName().Name;

// Add services to the container
builder.Services.AddIdentityServer()
    .AddTestUsers(InMemoryConfig.GetUsers())
    .AddDeveloperSigningCredential() //not something we want to use in a production environment;
    .AddConfigurationStore(opt =>
    {
        opt.ConfigureDbContext = c => c.UseSqlServer(
            builder.Configuration.GetConnectionString("sqlConnection"),
            sql => sql.MigrationsAssembly(migrationAssembly));
    })
    .AddOperationalStore(opt =>
    {
        opt.ConfigureDbContext = o => o.UseSqlServer(
            builder.Configuration.GetConnectionString("sqlConnection"),
            sql => sql.MigrationsAssembly(migrationAssembly));
    });

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Migrate database
app.MigrateDatabase();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();
app.UseRouting();

app.UseIdentityServer();

app.UseAuthorization();

app.MapDefaultControllerRoute();

app.Run();
