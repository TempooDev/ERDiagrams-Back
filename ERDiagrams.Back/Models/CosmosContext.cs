using Microsoft.Azure.Cosmos.Serialization.HybridRow.Schemas;
using Microsoft.EntityFrameworkCore;

namespace ERDiagrams.Back.Models;

public class CosmosContext : DbContext
{
    private readonly FunctionConfiguration _configuration;

    public DbSet<Book> Books { get; set; }

    public CosmosContext(FunctionConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Book>().ToContainer("Diagrams")
            .HasNoDiscriminator().HasPartitionKey("Id");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseCosmos(
            accountEndpoint: _configuration.CosmosAccountEndpoint,
            accountKey: _configuration.CosmosAccountKey,
            databaseName: _configuration.CosmosDatabaseName);
    }
    
}