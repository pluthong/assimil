using Newtonsoft.Json;
using System.Collections.Generic;

namespace Assimil.Domain
{
    public class Lesson
    {
        [JsonProperty("age")]
        public int Age { get; set; }

        [JsonProperty("haveIntro")]
        public bool HaveIntro { get; set; }

        [JsonProperty("page_number_descr_left")]
        public string PageNumberDescrLeft { get; set; }

        [JsonProperty("page_number_descr_right")]
        public string PageNumberDescrRight { get; set; }

        [JsonProperty("introduction")]
        public string Introduction { get; set; }

        [JsonProperty("idfile")]
        public string Idfile { get; set; }

        [JsonProperty("page_info_english")]
        public InfoPage PageInfoEnglish { get; set; }

        [JsonProperty("page_info_french")]
        public InfoPage PageInfoFrench { get; set; }

        [JsonProperty("isRevision")]
        public bool IsRevision { get; set; }

        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }

        [JsonProperty("audioUrl")]
        public string AudioUrl { get; set; }

        [JsonProperty("english_lesson")]
        public IEnumerable<Paragraph> EnglishLesson { get; set; }

        [JsonProperty("notes")]
        public IEnumerable<Note> Notes { get; set; }
    }
}
