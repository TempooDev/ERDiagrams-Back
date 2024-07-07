using AutoMapper;
using ERDiagrams.Collaborative.Hubs;
using ERDiagrams.Collaborative.Models;
using ERDiagrams.Collaborative.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

#region CONFIG

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var connectionString = builder.Configuration.GetConnectionString("MongoDB");
// Add services to the container.
builder.Services.AddSingleton<IMongoClient, MongoClient>(_ =>
    new MongoClient(connectionString));

#endregion

#region CORS POLICY

string[] allowedOrigins =
[
    "http://localhost:3000",
    "https://*.vercel.app/"
];

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins(allowedOrigins) // Reemplaza con tus orígenes permitidos
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()); // Importante para SignalR
});

#endregion

builder.Services.AddControllers()
    .AddJsonOptions(
        options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

#region SERVICES

builder.Services.AddControllers();
builder.Services.AddSignalR();

#endregion

var config = new MapperConfiguration(cfg => cfg.CreateMap<Diagram, DiagramDto>());

var app = builder.Build();
// Configure the HTTP request pipeline.
app.UseCors("AllowSpecificOrigin");
app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

#region API DIAGRAMS

app.MapGet("/diagrams", ([FromServices] IMongoClient mongoClient) =>
    {
        var mapper = config.CreateMapper();
        var database = DiagramDbContext.Create(mongoClient.GetDatabase("ERDiagram"));
        var diagrams = database.Diagrams.ToArrayAsync();
        Task.WaitAll();
        return mapper.Map<Diagram[], DiagramDto[]>(diagrams.Result);
    })
    .WithName("GetAllDiagrams")
    .WithOpenApi();

app.MapGet("/diagrams/{diagramId}", DiagramDto ([FromServices] IMongoClient mongoClient, string diagramId) =>
    {
        var mapper = config.CreateMapper();
        var database = DiagramDbContext.Create(mongoClient.GetDatabase("ERDiagram"));
        var diagram = database.Diagrams.FirstOrDefaultAsync(diagram => diagram.diagramId == diagramId);
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
app.MapPost("/diagrams", async ([FromServices] IMongoClient mongoClient, Diagram diagram) =>
    {
        var database = DiagramDbContext.Create(mongoClient.GetDatabase("ERDiagram"));
        database.Diagrams.Add(diagram);
        await database.SaveChangesAsync();
        return Results.Created($"/diagrams/{diagram.diagramId}", diagram);
    }).WithName("PostDiagram")
    .WithOpenApi();

app.MapPut("/diagrams/{diagramId}",
        async ([FromServices] IMongoClient mongoClient, string diagramId, DiagramDto diagramToUpdate) =>
        {
            var database = DiagramDbContext.Create(mongoClient.GetDatabase("ERDiagram"));
            var diagram = database.Diagrams.FirstOrDefaultAsync(dia => dia.diagramId.Equals(diagramId));
            Task.WaitAll();
            if (diagram.Result is null) return Results.NotFound();
            diagram.Result.image = diagramToUpdate.image;
            diagram.Result.name = diagramToUpdate.name;
            diagram.Result.linkDataArray = diagramToUpdate.linkDataArray;
            diagram.Result.nodeDataArray = diagramToUpdate.nodeDataArray;
            await database.SaveChangesAsync();
            return Results.NoContent();
        }).WithName("UpdateDiagram")
    .WithOpenApi();

app.MapDelete("/diagrams/{diagramId}", async ([FromServices] IMongoClient mongoClient, string diagramId) =>
{
    var database = DiagramDbContext.Create(mongoClient.GetDatabase("ERDiagram"));
    if (await database.Diagrams.FirstOrDefaultAsync(dia => dia.diagramId == diagramId) is Diagram diagram)
    {
        database.Diagrams.Remove(diagram);
        await database.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NotFound();
});

#endregion

#region HUB

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<BoardHub>("/hub/board");
});

#endregion

app.UseRouting();
app.Run();