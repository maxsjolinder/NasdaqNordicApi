using System.IO;
using System.Linq;
using System.Reflection;

namespace NorthernLight.NasdaqNordic.Test.Utils
{
    public static class EmbeddedResourceUtil
    {
        public static string ReadFileContent(string filename)
        {
            var assembly = Assembly.GetExecutingAssembly();
            string resourceName = assembly.GetManifestResourceNames().Single(str => str.EndsWith(filename));
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
