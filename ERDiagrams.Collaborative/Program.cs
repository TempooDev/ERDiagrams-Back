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

// Add services to the container.
builder.Services.Configure<DiagramDatabaseSettings>(
builder.Configuration.GetSection("ConnectionStrings"));
builder.Services.AddSingleton<IRepository<Diagram>, DiagramsRepository>();
builder.Services.AddScoped<IDiagramService,DiagramService>();
builder.Services.AddControllers()
    .AddJsonOptions(
        options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithOrigins("http://localhost:3000",  "http://localhost:5000", "https://localhost:5001","https://erdiagrams-react-hrcnsaoa7-tempoodevs-projects.vercel.app","https://erdiagrams-react.vercel.app/")
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
app.MapHub<BoardHub>("/hub/board");
app.MapHub<ChatHub>("/hub/chat");

app.Run();