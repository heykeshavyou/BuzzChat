
using BuzzTalk.Business;
using BuzzTalk.Business.Services;
using BuzzTalk.Data.Entities;
using BuzzTalk.Data.Repositories;
using BuzzTalk.Server.Hubs;
using BuzzTalk.Server.Mapper;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
// Add authentication and authorization services
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                if (!string.IsNullOrEmpty(accessToken))
                {
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
    });
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var provider = builder.Services.BuildServiceProvider();
var configuration = provider.GetRequiredService<IConfiguration>();
builder.Services.AddDbContext<BuzzTalkContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("buzzConnect")));
var config = new AutoMapper.MapperConfiguration(cfg =>
{
    cfg.AddProfile(new MapperDto());
    cfg.AddProfile(new MapperModel());
});
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }

    });

});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});
// And later
builder.Services.AddSignalR();
var mapper = config.CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services
    .AddTransient<IAccountService, AccountService>()
    .AddTransient<IMessageService, MessageService>()
    .AddTransient<IAccountRepository, AccountRepository>()
    .AddTransient<IMessageRepository, MessageRepository>()
    .AddTransient<IGroupRepository, GroupRepository>()
    .AddTransient<IGroupService, GroupService>()
    .AddTransient<INotificationService,NotificationService>();
FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromFile("FireChat.json")
});
var app = builder.Build();
app.UseStaticFiles();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseCors("AllowFrontend");


app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapHub<BuzzChatHub>("/Buzz/TalkHub");
app.Run();
