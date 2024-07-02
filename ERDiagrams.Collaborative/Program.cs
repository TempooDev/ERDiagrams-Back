using AutoMapper;
using ERDiagrams.Collaborative.Hubs;
using ERDiagrams.Collaborative.Models;
using ERDiagrams.Collaborative.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
var  connectionString = builder.Configuration.GetConnectionString("MongoDB");
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
            .WithOrigins("https://localhost:7112","http://localhost:5010","http://localhost:3000",  "http://localhost:5000", "https://localhost:5001","https://erdiagrams-react-hrcnsaoa7-tempoodevs-projects.vercel.app","https://erdiagrams-react.vercel.app/")
            .AllowCredentials()
            );
});
var config = new MapperConfiguration(cfg => cfg.CreateMap<Diagram, DiagramDto>());
var app = builder.Build();
app.UseCors("CorsPolicy");
// Configure the HTTP request pipeline.

    app.UseSwagger();
    app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/diagrams", ([FromServices] IMongoClient mongoClient) =>
    {
        var mapper = config.CreateMapper();
        var database = DiagramDbContext.Create(mongoClient.GetDatabase("ERDiagram"));
        var diagrams =database.Diagrams.ToArrayAsync();
        Task.WaitAll();
        return mapper.Map<Diagram[], DiagramDto[]>(diagrams.Result);

    })
    .WithName("GetAllDiagrams")
    .WithOpenApi();

app.MapGet("/diagrams/{id}", DiagramDto ([FromServices] IMongoClient mongoClient, string id) =>
    {
        
        var mapper = config.CreateMapper();
        var database = DiagramDbContext.Create(mongoClient.GetDatabase("ERDiagram"));
        var diagram =  database.Diagrams.FirstOrDefaultAsync(diagram => diagram.diagramId == id);
        Task.WaitAll();
        return mapper.Map<Diagram, DiagramDto>(diagram.Result);
        
    }).WithName("GetDiagramById")
    .WithOpenApi();

app.MapGet("/diagrams/user/{userId}", ([FromServices] IMongoClient mongoClient, string userId) =>
    {
        var mapper = config.CreateMapper();
        var database = DiagramDbContext.Create(mongoClient.GetDatabase("ERDiagram"));
        var diagramResult = database.Diagrams.Where(diagram => diagram.userId == userId).ToArrayAsync();
        Task.WaitAll();
        return mapper.Map<Diagram[], DiagramDto[]>(diagramResult.Result);
    }).WithName("GetDiagramsByUserId")
    .WithOpenApi();

app.MapHub<BoardHub>("/hub/board");
app.MapHub<ChatHub>("/hub/chat");

app.Run();