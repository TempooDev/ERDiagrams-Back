using ERDiagrams.Collaborative.Hubs;
using ERDiagrams.Collaborative.Models;
using ERDiagrams.Collaborative.Repositories;
using ERDiagrams.Collaborative.Repositories.Interfaces;
using ERDiagrams.Collaborative.Services;
using ERDiagrams.Collaborative.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
builder.Services.AddDbContext<CosmosContext>();
builder.Services.AddScoped<IDiagramService,DiagramService>();
builder.Services.AddScoped<IRepository<Diagram>, DiagramRepository>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder
            .WithOrigins("http://localhost:3000/chat")
            .AllowAnyMethod()
            .AllowAnyHeader()
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
app.MapHub<BoardHub>("/hub/board");
app.MapHub<ChatHub>("/hub/chat");
app.Run();