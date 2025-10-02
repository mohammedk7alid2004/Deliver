using Deliver.Api.AppConfiguration;
using Deliver.BLL.DTOs.Account;
using Deliver.Dal.Data;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDependencies(builder.Configuration);



builder.Services
            .AddFluentValidationAutoValidation()
            .AddValidatorsFromAssemblies(new[]
            {
        Assembly.GetExecutingAssembly(),           // Current Web.APIs
        typeof(LoginDTO).Assembly,               // Web.Application  
        typeof(ApplicationDbContext).Assembly              // Web.Infrastructure
            });

builder.Services.AddAuthentication()
           .AddGoogle(options =>
           {
               IConfigurationSection googleAuthSection = builder.Configuration.GetSection("Authentication:Google");

               options.ClientId = googleAuthSection["ClientId"];
               options.ClientSecret = googleAuthSection["ClientSecret"];
           });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();
app.MapControllers();

app.Run();
