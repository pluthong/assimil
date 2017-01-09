using Newtonsoft.Json;
using System.Collections.Generic;

namespace Assimil.Domain
{
    public class Paragraph
    {
        [JsonProperty("paragraph")]
        public IEnumerable<Sentence> Sentences { get; set; }
    }
}
