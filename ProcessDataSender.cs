using Newtonsoft.Json;
using System.Collections.Generic;

namespace json_splitter
{
    public class ProcessDataSender : IDataSender
    {
        private readonly Dictionary<ProcessConfiguration, ProcessStream> streams = new Dictionary<ProcessConfiguration, ProcessStream>();
        private readonly JsonSerializer serialiser;
        private readonly ProcessConfiguration configuration;

        public ProcessDataSender(JsonSerializer serialiser, ProcessConfiguration configuration)
        {
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
            if (!streams.ContainsKey(configuration))
            {
                var outputStreamFactory = new OutputStreamFactory(configuration.Format, configuration.ColumnHeaders, serialiser);
                streams.Add(configuration, new ProcessStream(configuration, outputStreamFactory));
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
