using System.Collections;
using System.Linq.Expressions;
using ERDiagrams.Collaborative.Models;
using ERDiagrams.Collaborative.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ERDiagrams.Collaborative.Repositories;
public class DiagramsRepository: IRepository<Diagram>
{
    private readonly IMongoCollection<Diagram> _diagramCollection;

    public DiagramsRepository (
        IOptions<DiagramDatabaseSettings> diagramDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            diagramDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            diagramDatabaseSettings.Value.DatabaseName);

        _diagramCollection = mongoDatabase.GetCollection<Diagram>(
            diagramDatabaseSettings.Value.DiagramsCollectionName);
    }

    public async Task<List<Diagram>> GetAsync() =>
        await _diagramCollection.Find(_ => true).ToListAsync();

    public async Task<Diagram?> GetAsync(string id) =>
        await _diagramCollection.Find(x => x._id == id).FirstOrDefaultAsync();
    
    public async Task<IEnumerable<Diagram>> GetByCondition(Expression<Func<Diagram, bool>> predicate) =>
        await _diagramCollection.Find(predicate).ToListAsync();

    public async Task CreateAsync(Diagram newBook) =>
        await _diagramCollection.InsertOneAsync(newBook);

    public async Task UpdateAsync(string id, Diagram updateDiagram) =>
        await _diagramCollection.ReplaceOneAsync(x => x._id == id, updateDiagram);

    public async Task RemoveAsync(string id) =>
        await _diagramCollection.DeleteOneAsync(x => x._id == id);
}