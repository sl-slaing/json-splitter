using Newtonsoft.Json;
using System.Collections.Generic;

namespace json_splitter
{
    public class ProcessDataSender : IDataSender
    {
        private readonly Dictionary<ProcessConfiguration, ProcessStream> streams = new Dictionary<ProcessConfiguration, ProcessStream>();
        private readonly JsonSerializer serialiser;

        public ProcessDataSender(JsonSerializer serialiser)
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
            if (!streams.ContainsKey(config.Process))
            {
                streams.Add(config.Process, new ProcessStream(config.Process, serialiser));
            }

            var stream = streams[config.Process];

            stream.SendData(relationalObject);
        }
    }
}
