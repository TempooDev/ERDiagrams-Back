using System;
using System.Collections.Generic;
using ERDiagrams.Back.Models.Interfaces;
using Newtonsoft.Json;

namespace ERDiagrams.Back.Models;

public class Diagram : Entity
{
    [JsonProperty(PropertyName = "diagramId",Required = Required.Always)]
    public Guid Id { get; set; }
    [JsonProperty(PropertyName = "nodes",Required = Required.Always)]
    public IEnumerable<Node> Nodes { get; set; }
    [JsonProperty(PropertyName = "relationships",Required = Required.Always)]
    public IEnumerable<Relationship> Relationships { get; set; }
    [JsonProperty(PropertyName = "user",Required = Required.Always)]
    public Guid User { get; set; }
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