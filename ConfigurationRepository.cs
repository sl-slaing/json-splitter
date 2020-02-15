using Newtonsoft.Json;
using System.IO;

namespace json_splitter
{
    public class ConfigurationRepository
    {
        private readonly JsonSerializer serialiser;

        public ConfigurationRepository(JsonSerializer serialiser)
        {
            this.serialiser = serialiser;
        }

        public RelatedJsonConfiguration ReadConfiguration(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("Configuration file not found", Path.GetFullPath(path));
            }

            return serialiser.Deserialize<RelatedJsonConfiguration>(new JsonTextReader(new StreamReader(path)));
        }
    }
}
