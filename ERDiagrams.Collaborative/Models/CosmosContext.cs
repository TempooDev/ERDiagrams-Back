using System.Configuration;
using ERDiagrams.Collaborative.Models;
using Microsoft.EntityFrameworkCore;

namespace ERDiagrams.Collaborative.Models;

public class CosmosContext : DbContext
{
    private readonly Configuration _configuration;


    public DbSet<Diagram> Diagrams { get; set; }
    public CosmosContext()
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Diagram>().ToContainer("Diagrams")
            .HasNoDiscriminator().HasPartitionKey("Id");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseCosmos(
            accountEndpoint: "https://cos-erdiagram.documents.azure.com:443/",
            accountKey: "cQieo6D3c72LQ3VFxtlpwN6u0bArnKkVfM4PX36uGuFrFHfOUkCN4T11MY2xLFUoroYVVelz1JemACDbMJFCbw==",
            databaseName: "ERDiagrmas");
    }
    
}