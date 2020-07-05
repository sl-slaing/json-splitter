using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace json_splitter
{
    public class FileDataSender : IDataSender
    {
        private readonly Dictionary<FileConfiguration, FileStream> streams = new Dictionary<FileConfiguration, FileStream>();
        private readonly JsonSerializer serialiser;
        private readonly FileConfiguration configuration;

        public FileDataSender(JsonSerializer serialiser, FileConfiguration configuration)
        {
            if (serialiser == null)
            {
                throw new ArgumentNullException(nameof(serialiser));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            this.serialiser = serialiser;
            this.configuration = configuration;
        }

        public void Dispose()
        {
            foreach (var stream in streams.Values)
            {
                stream.Dispose();
            }
        }

        public void SendData(IRelationalObject relationalObject)
        {
            if (relationalObject == null)
            {
                throw new ArgumentNullException(nameof(relationalObject));
            }

            if (!streams.ContainsKey(configuration))
            {
                var fileName = configuration.FileName;
                var factory = new OutputStreamFactory(configuration.Format, configuration.ColumnHeaders, serialiser);
                var writer = new StreamWriter(fileName, false);
                streams.Add(configuration, new FileStream(factory.Create(writer)));
            }

            var stream = streams[configuration];

            var bindingConfig = configuration as IBindingConfiguration;
            if (bindingConfig != null)
            {
                relationalObject = relationalObject.WithForeignKey(bindingConfig);
            }

            stream.SendData(relationalObject);
        }
    }
}
