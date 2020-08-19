using System;
using Newtonsoft.Json;

namespace hortus.entities
{
    public class GardenStory
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string GardenName { get; set; }

        [JsonProperty(PropertyName = "postcode")]
        public string PostCode {get; set;}

        [JsonProperty(PropertyName = "firstname")]
        public string FirstName { get; set; }   

        [JsonProperty(PropertyName = "lastname")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "datecreated")]
        public DateTime DateCreated { get; set; }

        [JsonProperty(PropertyName = "lastupdated")]
        public DateTime LastUpdated { get; set; }
    }
}
