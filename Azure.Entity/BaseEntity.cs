namespace Azure.Entity
{
    using Newtonsoft.Json;
    public abstract class BaseEntity
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
    }
}
