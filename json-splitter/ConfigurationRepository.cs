using Newtonsoft.Json;
using System;
using System.IO;

namespace json_splitter
{
    public class ConfigurationRepository : IConfigurationRepository
    {
        private readonly JsonSerializer serialiser;

        public ConfigurationRepository(JsonSerializer serialiser)
        {
            if (serialiser == null)
            {
                throw new ArgumentNullException(nameof(serialiser));
            }

            this.serialiser = serialiser;
        }

        public RelatedJsonConfiguration ReadConfiguration(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (!File.Exists(path))
            {
                throw new FileNotFoundException("Configuration file not found", Path.GetFullPath(path));
            }

            return ReadConfiguration(new StreamReader(path));
        }

        public RelatedJsonConfiguration ReadConfiguration(TextReader reader)
        {
            return serialiser.Deserialize<RelatedJsonConfiguration>(new JsonTextReader(reader));
        }
    }
}
