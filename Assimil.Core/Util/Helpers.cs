
using System.IO;
using System.Reflection;

namespace Assimil.Core
{
    public class Helpers
    {
        public static string GetJson(string resourceName)
        {
            string content = string.Empty;
            var assembly = Assembly.GetExecutingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                content = reader.ReadToEnd();
            }
            return content;
        }
    }
}
