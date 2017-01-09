
using Assimil.Domain;
using Newtonsoft.Json;

namespace Assimil.Core
{
    public class LessonProvider : ILesson
    {

        public Lesson GetLesson(int id)
        {
            var json = Helpers.GetJson("Assimil.Core.Resource.lesson_" +  id.ToString().PadLeft(3,'0') + ".json");

            Lesson lesson = JsonConvert.DeserializeObject<Lesson>(json);

            return lesson;
        }
    }
}
