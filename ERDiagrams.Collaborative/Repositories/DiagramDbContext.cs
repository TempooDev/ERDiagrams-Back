using ERDiagrams.Collaborative.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;

namespace ERDiagrams.Collaborative.Repositories;

public class DiagramDbContext: DbContext
{
    public DbSet<Diagram> Diagrams { get; init; }
    
    public static DiagramDbContext Create(IMongoDatabase database) =>
        new(new DbContextOptionsBuilder<DiagramDbContext>()
            .UseMongoDB(database.Client, database.DatabaseNamespace.DatabaseName)
            .Options);
    
    public DiagramDbContext(DbContextOptions options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Diagram>().ToCollection("Diagrams");
    }
}