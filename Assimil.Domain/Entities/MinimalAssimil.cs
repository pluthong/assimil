
using Newtonsoft.Json;
namespace Assimil.Domain
{
    public class MinimalAssimil
    {
        [JsonProperty("age")]
        public int Age { get; set; }

        [JsonProperty("idfile")]
        public string Idfile { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("isRevision")]
        public bool IsRevision { get; set; }

        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }

        [JsonProperty("audioUrl")]
        public string AudioUrl { get; set; }
    }
}
