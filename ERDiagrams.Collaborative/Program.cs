using ERDiagrams.Collaborative.Hubs;
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
        var database = DiagramDbContext.Create(mongoClient.GetDatabase("ERDiagram"));
        return database.Diagrams.ToArrayAsync();
    })
    .WithName("GetAllDiagrams")
    .WithOpenApi();

app.MapGet("/diagrams/{id}", ([FromServices] IMongoClient mongoClient, string id) =>
    {
        var database = DiagramDbContext.Create(mongoClient.GetDatabase("ERDiagram"));
        return database.Diagrams.FirstOrDefaultAsync(diagram => diagram.diagramId == id);
    }).WithName("GetDiagramById")
    .WithOpenApi();

app.MapGet("/diagrams/user/{userId}", ([FromServices] IMongoClient mongoClient, string userId) =>
    {
        var database = DiagramDbContext.Create(mongoClient.GetDatabase("ERDiagram"));
        return database.Diagrams.Where(diagram => diagram.userId == userId).ToArrayAsync();
    }).WithName("GetDiagramsByUserId")
    .WithOpenApi();

app.MapHub<BoardHub>("/hub/board");
app.MapHub<ChatHub>("/hub/chat");

app.Run();