namespace Azure.Entity
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class Item : BaseEntity
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "colors")]
        public List<string> Colors { get; set; }
    }
}
