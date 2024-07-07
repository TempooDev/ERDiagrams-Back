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

builder.Services.AddControllers()
    .AddJsonOptions(
        options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
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


#region SERVICES

builder.Services.AddControllers();
builder.Services.AddSignalR();

#endregion

var config = new MapperConfiguration(cfg => cfg.CreateMap<Diagram, DiagramDto>());

var app = builder.Build();

app.UseCors("AllowSpecificOrigin");

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();


app.UseRouting();
app.UseAuthorization();

#region HUB Endpoints

app.UseEndpoints(endpoints => { endpoints.MapHub<BoardHub>("/hub/board"); });

#endregion

app.MapControllers();

#region API DIAGRAMS

app.MapGet("/diagrams", ([FromServices] IMongoClient mongoClient) =>
    {
        try
        {
            var mapper = config.CreateMapper();
            app.Logger.LogInformation("Getting all diagrams");
            var database = DiagramDbContext.Create(mongoClient.GetDatabase("ERDiagram"));
            var diagrams = database.Diagrams.ToArrayAsync();
            Task.WaitAll();
            var diagramDto = mapper.Map<Diagram[], DiagramDto[]>(diagrams.Result);
            return Results.Ok(diagramDto);
        }
        catch (Exception e)
        {
            app.Logger.LogError(e.Message);
            return Results.Problem();
        }
    })
    .WithName("GetAllDiagrams")
    .WithOpenApi();

app.MapGet("/diagrams/{diagramId}", ([FromServices] IMongoClient mongoClient, string diagramId) =>
    {
        try
        {
            var mapper = config.CreateMapper();
            app.Logger.LogInformation("Getting all diagrams");
            var database = DiagramDbContext.Create(mongoClient.GetDatabase("ERDiagram"));
            var diagram = database.Diagrams.FirstOrDefaultAsync(diagram => diagram.diagramId == diagramId);
            Task.WaitAll();
            if (diagram.Result == null) return Results.NotFound();
            var diagramDto = mapper.Map<Diagram, DiagramDto>(diagram.Result);
            return Results.Ok(diagramDto);
        }
        catch (Exception e)
        {
            app.Logger.LogError(e.Message);
            return Results.Problem();
        }
    }).WithName("GetDiagramById")
    .WithOpenApi();

app.MapGet("/diagrams/user/{userId}", ([FromServices] IMongoClient mongoClient, string userId) =>
    {
        try
        {
            var mapper = config.CreateMapper();
            var database = DiagramDbContext.Create(mongoClient.GetDatabase("ERDiagram"));
            var diagramResult = database.Diagrams.Where(diagram => diagram.userId == userId).ToArrayAsync();
            Task.WaitAll();
            var diagramDto = mapper.Map<Diagram[], DiagramDto[]>(diagramResult.Result);
            return Results.Ok(diagramDto);
        }
        catch (Exception e)
        {
            app.Logger.LogError(e.Message);
            return Results.Problem();
        }
    }).WithName("GetDiagramsByUserId")
    .WithOpenApi();
app.MapPost("/diagrams", async ([FromServices] IMongoClient mongoClient, Diagram diagram) =>
    {
        try
        {
            var database = DiagramDbContext.Create(mongoClient.GetDatabase("ERDiagram"));
            database.Diagrams.Add(diagram);
            await database.SaveChangesAsync();
            return Results.Created($"/diagrams/{diagram.diagramId}", diagram);
        }
        catch (Exception e)
        {
            app.Logger.LogError(e.Message);
            return Results.Problem();
        }
    }).WithName("PostDiagram")
    .WithOpenApi();

app.MapPut("/diagrams/{diagramId}",
        async ([FromServices] IMongoClient mongoClient, string diagramId, DiagramDto diagramToUpdate) =>
        {
            try
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
            }
            catch (Exception e)
            {
                app.Logger.LogError(e.Message);
                return Results.Problem();
            }
        }).WithName("UpdateDiagram")
    .WithOpenApi();

app.MapDelete("/diagrams/{diagramId}", async ([FromServices] IMongoClient mongoClient, string diagramId) =>
{
    try
    {
        var database = DiagramDbContext.Create(mongoClient.GetDatabase("ERDiagram"));
        if (await database.Diagrams.FirstOrDefaultAsync(dia => dia.diagramId == diagramId) is not Diagram diagram)
            return Results.NotFound();
        database.Diagrams.Remove(diagram);
        await database.SaveChangesAsync();
        return Results.NoContent();
    }
    catch (Exception e)
    {
        app.Logger.LogError(e.Message);
        return Results.Problem();
    }
});

#endregion


app.Run();