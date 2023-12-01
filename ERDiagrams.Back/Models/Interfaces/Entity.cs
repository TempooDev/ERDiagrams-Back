using Newtonsoft.Json;
namespace ERDiagrams.Back.Models.Interfaces;

public  abstract class Entity: IEntity
{
    [JsonProperty(PropertyName = "id",NullValueHandling = NullValueHandling.Ignore)]
    public string Id { get; set; }
}