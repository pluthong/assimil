using Newtonsoft.Json;

namespace Assimil.Domain
{
    public class Sentence
    {
        [JsonProperty("speech")]
        public string Speech { get; set; }

        [JsonProperty("alterSpeech")]
        public bool AlterSpeech { get; set; }

        [JsonProperty("frenchspeech")]
        public string FrenchSpeech { get; set; }

        [JsonProperty("beginplay")]
        public double BeginPlay { get; set; }

        [JsonProperty("endplay")]
        public double EndPlay { get; set; }
    }
}
