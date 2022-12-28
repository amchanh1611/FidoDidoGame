using AutoMapper;
using FidoDidoGame.Mapping;
using FidoDidoGame.Middleware;
using FidoDidoGame.Modules.FidoDidos.Service;
using FidoDidoGame.Modules.Ranks.Services;
using FidoDidoGame.Modules.Users.Services;
using FidoDidoGame.Persistents.Context;
using FidoDidoGame.Persistents.Redis.Services;
using FidoDidoGame.Persistents.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using Hangfire;
using Hangfire.MySql;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

//ConfigureManager
ConfigurationManager configure = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Redis
builder.Services.AddSingleton<IConnectionMultiplexer>(options => ConnectionMultiplexer.Connect(configure["Appsettings:Database:RedisConnection"]));

// MySql
builder.Services.AddDbContextPool<AppDbContext>(option =>
    option.UseMySql(configure["Appsettings:Database:ConnectionString"],
    ServerVersion.AutoDetect(configure["Appsettings:Database:ConnectionString"])));

//Hangfire
builder.Services.AddHangfire
    (x => x.UseStorage(
        new MySqlStorage("server=localhost;database=emailmarketing;uid=root;pwd='';Allow User Variables=True",
        new MySqlStorageOptions())));
builder.Services.AddHangfireServer(options => configure.GetSection("HangfireSettings:Server").Bind(options));

//HttpContextAccessor
builder.Services.AddHttpContextAccessor();


//Repository
builder.Services.AddScoped<IRepository, Repository>();

//AutoMapper
//builder.Services.AddAutoMapper(typeof(Profiles));
builder.Services.AddScoped(provider => new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new Profiles(provider.GetService<IRepository>()!));
}).CreateMapper());

//Fluent Validation
builder.Services.AddFluentValidationAutoValidation()
            .AddFluentValidationClientsideAdapters()
            .AddValidatorsFromAssemblyContaining(typeof(Program));

//Services
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IFidoDidoService, FidoDidoService>();
builder.Services.AddScoped<IRedisService, RedisService>();
builder.Services.AddScoped<IRankService, RankService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//ErorHandlerMiddleware
app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
