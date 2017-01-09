using Newtonsoft.Json;
using System.Collections.Generic;

namespace Assimil.Domain
{
    public class InfoPage
    {
        [JsonProperty("description_lang")]
        public string DescriptionLang { get; set; }

        [JsonProperty("description_audio_begin")]
        public double DescriptionAudioBegin { get; set; }

        [JsonProperty("description_audio_end")]
        public double DescriptionAudioEnd { get; set; }

        [JsonProperty("page_number_descr")]
        public string PageNumberDescr { get; set; }
    }
}
