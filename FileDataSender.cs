using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace json_splitter
{
    public class FileDataSender : IDataSender
    {
        private readonly Dictionary<IDataConfiguration, FileStream> streams = new Dictionary<IDataConfiguration, FileStream>();
        private readonly JsonSerializer serialiser;

        public FileDataSender(JsonSerializer serialiser)
        {
            this.serialiser = serialiser;
        }

        public void Dispose()
        {
            foreach (var stream in streams.Values)
            {
                stream.Dispose();
            }
        }

        public void SendData(IRelatedDataConfiguration config, IRelationalObject relationalObject)
        {
            if (!streams.ContainsKey(config))
            {
                streams.Add(config, new FileStream(config.TableName + ".json", serialiser));
            }

            var stream = streams[config];

            stream.SendData(relationalObject);
        }
    }
}
