using AutoMapper;
using ERDiagrams.Collaborative.Hubs;
using ERDiagrams.Collaborative.Models;
using ERDiagrams.Collaborative.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ER-Diagrams", Version = "v1" });

    // Configura la seguridad de JWT
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[] {}
        }
    });
});
builder.Services.AddSignalR();

var connectionString = builder.Configuration.GetConnectionString("MongoDB");
// Add services to the container.
builder.Services.AddSingleton<IMongoClient, MongoClient>(_ =>
    new MongoClient(connectionString));

builder.Services.AddControllers()
    .AddJsonOptions(
        options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builderCors => builderCors
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithOrigins("https://localhost:7112", "http://localhost:5010", "http://localhost:3000",
                "http://localhost:5000", "https://localhost:5001",
                "https://erdiagrams-react-hrcnsaoa7-tempoodevs-projects.vercel.app",
                "https://erdiagrams-react.vercel.app/")
            .AllowCredentials()
            
    );
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, c =>
    {
        c.Authority = $"https://{builder.Configuration["Auth0:Domain"]}";
        c.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidAudience = builder.Configuration["Auth0:Audience"],
            ValidIssuer = $"{builder.Configuration["Auth0:Domain"]}"
        };
    });

builder.Services.AddAuthorization(o =>
{
    o.AddPolicy("diagrams:read-write", p => p.
        RequireAuthenticatedUser().
        RequireClaim("scope", "diagrams:read-write"));
});

var config = new MapperConfiguration(cfg => cfg.CreateMap<Diagram, DiagramDto>());
var app = builder.Build();

app.UseCors("CorsPolicy");
app.UseHttpsRedirection();

// Configure the HTTP request pipeline.
app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/diagrams", ([FromServices] IMongoClient mongoClient) =>
    {
        var mapper = config.CreateMapper();
        var database = DiagramDbContext.Create(mongoClient.GetDatabase("ERDiagram"));
        var diagrams = database.Diagrams.ToArrayAsync();
        Task.WaitAll();
        return mapper.Map<Diagram[], DiagramDto[]>(diagrams.Result);
    })
    .WithName("GetAllDiagrams")
    .WithOpenApi()    .RequireAuthorization("diagrams:read-write");

app.MapGet("/diagrams/{id}", DiagramDto ([FromServices] IMongoClient mongoClient, string id) =>
    {
        var mapper = config.CreateMapper();
        var database = DiagramDbContext.Create(mongoClient.GetDatabase("ERDiagram"));
        var diagram = database.Diagrams.FirstOrDefaultAsync(diagram => diagram.diagramId == id);
        Task.WaitAll();
        return mapper.Map<Diagram, DiagramDto>(diagram.Result);
    }).WithName("GetDiagramById")
    .WithOpenApi()
    .RequireAuthorization("diagrams:read-write");

app.MapGet("/diagrams/user/{userId}", ([FromServices] IMongoClient mongoClient, string userId) =>
    {
        var mapper = config.CreateMapper();
        var database = DiagramDbContext.Create(mongoClient.GetDatabase("ERDiagram"));
        var diagramResult = database.Diagrams.Where(diagram => diagram.userId == userId).ToArrayAsync();
        Task.WaitAll();
        return mapper.Map<Diagram[], DiagramDto[]>(diagramResult.Result);
    }).WithName("GetDiagramsByUserId")
    .WithOpenApi()
    .RequireAuthorization("diagrams:read-write");

app.MapHub<BoardHub>("/hub/board");
app.MapHub<ChatHub>("/hub/chat");

app.Run();