using Newtonsoft.Json;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace ERDiagrams.Collaborative.Models;

public class Diagram 
{
    
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [JsonProperty(PropertyName = "_id",Required = Required.Always)]
    public string _id { get; set; }
    
    [BsonElement("nodeDataArray")]
    [JsonProperty(PropertyName = "nodeDataArray",Required = Required.Always)]
    public IEnumerable<Node> NodeDataArray { get; set; }
    
    [JsonProperty(PropertyName = "linkDataArray",Required = Required.Always)]
    [BsonElement("linkDataArray")]
    public IEnumerable<Relationship> LinkDataArray { get; set; }
    
    
    [JsonProperty(PropertyName = "userId",Required = Required.Always)]
    [BsonElement("userId")]
    public string UserId { get; set; }
    
    [JsonProperty(PropertyName = "name",Required = Required.Always)]
    [BsonElement("name")]
    public string Name { get; set; }
    
    [JsonProperty(PropertyName = "image",Required = Required.Always)]
    [BsonElement("image")]
    public string Image { get; set; }
}

public class Node
{
    [BsonElement("key")]
    [BsonRepresentation(BsonType.Int64)]
    [JsonProperty(PropertyName = "key",Required = Required.Always)]
    public int Key { get; set; }
    
    [BsonElement("name")]
    [JsonProperty(PropertyName = "name",Required = Required.Always)]
    public string Name { get; set; }
    
    [BsonElement("items")]
    [JsonProperty(PropertyName = "items",Required = Required.Always)]
    public IEnumerable<Property> Items { get; set; }
    
    [BsonElement("inheriteditems")]
    [JsonProperty(PropertyName = "inheriteditems",Required = Required.Always)]
    public IEnumerable<Property> Inheriteditems { get; set; }
    
    [BsonElement("visibility")]
    [BsonRepresentation(BsonType.Boolean)]
    [JsonProperty(PropertyName = "visibility",Required = Required.Always)]
    public bool Visibility { get; set; }
    
    [BsonElement("location")]
    [JsonProperty(PropertyName = "location",Required = Required.Always)]
    public Location Location { get; set; }
}
public class Location
{
    [BsonElement("x")]
    [BsonRepresentation(BsonType.Double)]
    [JsonProperty(PropertyName = "x",Required = Required.Always)]
    public double X { get; set; }
    [BsonElement("y")]
    [BsonRepresentation(BsonType.Double)]
    [JsonProperty(PropertyName = "y",Required = Required.Always)]
    public double Y { get; set; }
}
public class Property
{
    [BsonElement("name")]
    [JsonProperty(PropertyName = "name",Required = Required.Always)]
    public string PropertyName { get; set; }
    
    [BsonElement("iskey")]
    [BsonRepresentation(BsonType.Boolean)]
    [JsonProperty(PropertyName = "iskey",Required = Required.Always)]
    public bool IsKey { get; set; }
    
    [BsonElement("type")]
    [JsonProperty(PropertyName = "type",Required = Required.Always)]
    public string Type { get; set; }
    
    
    [BsonElement("figure")]
    [JsonProperty(PropertyName = "figure",Required = Required.Always)]
    public string Figure { get; set; }
    
    [BsonElement("color")]
    [JsonProperty(PropertyName = "color",Required = Required.Always)]
    public string Color { get; set; }
}
public class Relationship
{
    [BsonElement("to")]
    [BsonRepresentation(BsonType.Int32)]
    [JsonProperty(PropertyName = "to",Required = Required.Always)]
    public int To { get; set; }
    [BsonRepresentation(BsonType.Int32)]
    [JsonProperty(PropertyName = "from",Required = Required.Always)]
    [BsonElement("from")]
    public int From { get; set; }
    [BsonElement("toText")]
    [JsonProperty(PropertyName = "toText",Required = Required.Always)]
    public string ToText { get; set; }
    [BsonElement("text")]
    [JsonProperty(PropertyName = "text",Required = Required.Always)]
    public string Text { get; set; }
}