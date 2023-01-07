using AutoMapper;
using FidoDidoGame.AppSetting;
using FidoDidoGame.Common.Jwt;
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
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//ConfigureManager
ConfigurationManager configure = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "API",
        Description = "QPIN API with ASP.NET 6",
        Contact = new OpenApiContact()
        {
            Name = "Au Minh Chanh",
            Email = "am.chanh16111@gmail.com"
        }
    });
    OpenApiSecurityScheme securitySchema = new() 
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };
    c.AddSecurityDefinition("Bearer", securitySchema);

    OpenApiSecurityRequirement securityRequirement = new();
    securityRequirement.Add(securitySchema, new[] { "Bearer" });
    c.AddSecurityRequirement(securityRequirement);
});

//Redis
builder.Services.AddSingleton<IConnectionMultiplexer>(options => ConnectionMultiplexer.Connect(configure["Appsettings:Database:RedisConnection"]));

// MySql
builder.Services.AddDbContextPool<AppDbContext>(option =>
    option.UseMySql(configure["Appsettings:Database:ConnectionString"],
    ServerVersion.AutoDetect(configure["Appsettings:Database:ConnectionString"])));

//Hangfire
builder.Services.AddHangfire
    (x => x.UseStorage(
        new MySqlStorage(configure["HangfireSettings:Connect"],
        new MySqlStorageOptions())));
builder.Services.AddHangfireServer(options => configure.GetSection("HangfireSettings:Server").Bind(options));

// configure strongly typed settings object
builder.Services.Configure<AppSettings>(configure.GetSection("AppSettings"));

//Authenticate
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configure["Appsettings:Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Appsettings:Jwt:AccessKey"])),
            ClockSkew = TimeSpan.Zero
        };
    })
    .AddCookie(options =>
    {
        options.LoginPath = "/api/Users/FacebookOauth";
    })
    .AddFacebook(options =>
    {
        options.AppId = configure["Appsettings:Authentication:Facebook:AppId"];
        options.AppSecret = configure["Appsettings:Authentication:Facebook:AppSecret"];
        options.SaveTokens = true;
    });


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
builder.Services.AddScoped<IJwtServices, JwtServices>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();

app.UseAuthorization();

app.UseStaticFiles();

//ErorHandlerMiddleware
app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
