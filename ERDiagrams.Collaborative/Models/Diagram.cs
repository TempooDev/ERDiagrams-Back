
using ERDiagrams.Collaborative.Models.Interfaces;
using Newtonsoft.Json;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace ERDiagrams.Collaborative.Models;

public class Diagram : Entity
{
    
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [JsonProperty(PropertyName = "id",Required = Required.Always)]
    public string Id { get; set; }
    public IEnumerable<Node> Nodes { get; set; }
    public IEnumerable<Relationship> Relationships { get; set; }
    public string User { get; set; }
}

public class Node
{
    [JsonProperty(PropertyName = "name",Required = Required.Always)]
    public string Name { get; set; }
    [JsonProperty(PropertyName = "properties",Required = Required.Always)]
    public IEnumerable<Property> Properties { get; set; }
}

public class Property
{
    [JsonProperty(PropertyName = "propertyName",Required = Required.Always)]
    public string PropertyName { get; set; }
    [JsonProperty(PropertyName = "isKey",Required = Required.Always)]
    public string IsKey { get; set; }
}
public class Relationship
{
    [JsonProperty(PropertyName = "to",Required = Required.Always)]
    public string to { get; set; }
    [JsonProperty(PropertyName = "from",Required = Required.Always)]
    public string from { get; set; }
    [JsonProperty(PropertyName = "levelto",Required = Required.Always)]
    public string levelto { get; set; }
    [JsonProperty(PropertyName = "levelfrom",Required = Required.Always)]
    public string levelfrom { get; set; }
}