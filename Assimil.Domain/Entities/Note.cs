using Newtonsoft.Json;
using System.Collections.Generic;

namespace Assimil.Domain
{
   public class Note
    {
        [JsonProperty("keynote")]
        public string KeyNote { get; set; }

        [JsonProperty("valuenote")]
        public string ValueNote { get; set; }
    }
}
