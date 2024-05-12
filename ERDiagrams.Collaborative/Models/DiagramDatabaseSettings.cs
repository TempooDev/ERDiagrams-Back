namespace ERDiagrams.Collaborative.Models;

public class DiagramDatabaseSettings
{

    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string DiagramsCollectionName { get; set; } = null!;
}