using Newtonsoft.Json;

namespace ERDiagrams.Collaborative.Models;

public class DiagramDto 
{
    
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
