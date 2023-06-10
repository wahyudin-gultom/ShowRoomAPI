using Google.Cloud.PubSub.V1;
using Google.Cloud.Iam;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ShowRoomAPI;
using ShowRoomAPI.DataAccess.Implementation;
using ShowRoomAPI.DataAccess.Interface;
using ShowRoomAPI.Models.Entitas;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//connect to database
builder.Services.AddDbContext<ShowRoomDataContext>( op => {
    op.UseNpgsql(builder.Configuration.GetConnectionString("ShowroomDb"));
});

// Add services to the container.
builder.Services.AddTransient<ITransientService, TransientService>();
builder.Services.AddScoped<IScopedService, ScopedService>();
builder.Services.AddScoped<ICarRepository, CarRepository>();
//builder.Services.AddScoped<IJwtBearerManager, JwtBearerManager>();

//Configure Google pub/sub
var projectId = builder.Configuration["GcpCredential:projectId"];
var topicId = builder.Configuration["GcpCredential:topicId"];
var subscribeId = builder.Configuration["GcpCredential:subscribeId"];
var credential = builder.Configuration["GcpCredential:secret"];

builder.Services.AddPublisherClient(cfg =>
{
    var topicName = TopicName.FromProjectTopic(projectId, topicId);
    cfg.TopicName = topicName;
    cfg.JsonCredentials = credential;
});

builder.Services.AddSubscriberClient(cfg =>
{
    var topicName = TopicName.FromProjectTopic(projectId, topicId);
    cfg.SubscriptionName = SubscriptionName.FromProjectSubscription(projectId, subscribeId);
    cfg.JsonCredentials = credential;
});

builder.Services.AddSingleton<GooglePubSubService>();

builder.Services.Configure<JwtConfig>(
    builder.Configuration.GetSection("AppConfigJwt"));

builder.Services.AddAuthentication(m =>
{
    m.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    m.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(n =>
{
    n.RequireHttpsMetadata = false;
    n.SaveToken = true;
    n.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = false,
        ValidateAudience = false,
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(builder.Configuration["AppConfigJwt:secret"]))
    };

    n.Events = new JwtBearerEvents();
    n.Events.OnChallenge = context =>
    {
        //if any issue token then get forbidden response
        return Task.FromResult(context.Response.StatusCode = 403);
    };
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen( c =>
{
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });

});


var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
   
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
