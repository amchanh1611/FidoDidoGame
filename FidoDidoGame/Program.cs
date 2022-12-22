using FidoDidoGame.Mapping;
using FidoDidoGame.Modules.FidoDidos.Service;
using FidoDidoGame.Modules.Users.Services;
using FidoDidoGame.Persistents.Context;
using FidoDidoGame.Persistents.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//ConfigureManager
ConfigurationManager configure = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// MySql
builder.Services.AddDbContextPool<AppDbContext>(option =>
    option.UseMySql(configure["Appsettings:Database:ConnectionString"],
    ServerVersion.AutoDetect(configure["Appsettings:Database:ConnectionString"])));

//Repository
builder.Services.AddScoped<IRepository, Repository>();

//AutoMapper
builder.Services.AddAutoMapper(typeof(Profiles));

//Fluent Validation
builder.Services.AddFluentValidationAutoValidation()
            .AddFluentValidationClientsideAdapters()
            .AddValidatorsFromAssemblyContaining(typeof(Program));

//Services
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IFidoDidoService, FidoDidoService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
