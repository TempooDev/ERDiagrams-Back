using Newtonsoft.Json;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace ERDiagrams.Collaborative.Models;

public class Diagram 
{
    
    [JsonProperty(PropertyName = "_id",Required = Required.Always)]
    public ObjectId _id { get; set; }
    
    [JsonProperty(PropertyName = "diagramId",Required = Required.Always)]
    public string diagramId { get; set; }
    
    [JsonProperty(PropertyName = "nodeDataArray",Required = Required.Always)]
    public IEnumerable<Node?> nodeDataArray { get; set; }
    
    [JsonProperty(PropertyName = "linkDataArray",Required = Required.Always)]
    public IEnumerable<Relationship?> linkDataArray { get; set; }
    
    
    [JsonProperty(PropertyName = "userId",Required = Required.Always)]
    public string? userId { get; set; }
    
    [JsonProperty(PropertyName = "name",Required = Required.Always)]
    public string? name { get; set; }
    
    [JsonProperty(PropertyName = "image")]
    public string? image { get; set; }
}

public class Node
{
    [JsonProperty(PropertyName = "key",Required = Required.Always)]
    public int? key { get; set; }
    
    [JsonProperty(PropertyName = "name",Required = Required.Always)]
    public string? name { get; set; }
    
    [JsonProperty(PropertyName = "items",Required = Required.Always)]
    public IEnumerable<Property>? items { get; set; }
    
    [JsonProperty(PropertyName = "inheriteditems",Required = Required.Always)]
    public IEnumerable<Property>? inheriteditems { get; set; }
    
    [JsonProperty(PropertyName = "visibility",Required = Required.Always)]
    public bool? visibility { get; set; }
    
    [JsonProperty(PropertyName = "location",Required = Required.Always)]
    public Location? location { get; set; }
}
public class Location
{
    [JsonProperty(PropertyName = "x",Required = Required.Always)]
    public double? x { get; set; }
   
    [JsonProperty(PropertyName = "y",Required = Required.Always)]
    public double? y { get; set; }
}
public class Property
{
    [JsonProperty(PropertyName = "name",Required = Required.Always)]
    public string? name { get; set; }
    
    [JsonProperty(PropertyName = "iskey",Required = Required.Always)]
    public bool? iskey { get; set; }
    
    [JsonProperty(PropertyName = "type",Required = Required.Always)]
    public string type { get; set; }
    
    
    [JsonProperty(PropertyName = "figure",Required = Required.Always)]
    public string figure { get; set; }
    
    [JsonProperty(PropertyName = "color",Required = Required.Always)]
    public string color { get; set; }
}
public class Relationship
{
    [JsonProperty(PropertyName = "to",Required = Required.Always)]
    public int to { get; set; }
    
    [JsonProperty(PropertyName = "from",Required = Required.Always)]
    public int from { get; set; }
    
    [JsonProperty(PropertyName = "toText",Required = Required.Always)]
    public string toText { get; set; }
    
    [JsonProperty(PropertyName = "text",Required = Required.Always)]
    public string text { get; set; }
}