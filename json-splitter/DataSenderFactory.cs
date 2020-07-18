using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace json_splitter
{
    public class DataSenderFactory : IDataSenderFactory
    {
        private readonly JsonSerializer serialiser;
        private readonly IStreamFactory streamFactory;
        private readonly Dictionary<IDataConfiguration, IDataSender> senders = new Dictionary<IDataConfiguration, IDataSender>();

        public DataSenderFactory(JsonSerializer serialiser, IStreamFactory streamFactory)
        {
            if (serialiser == null)
            {
                throw new ArgumentNullException(nameof(serialiser));
            }

            this.serialiser = serialiser;
            this.streamFactory = streamFactory;
        }

        public IDataSender GetDataSender(IDataConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (!senders.ContainsKey(configuration))
            {
                senders.Add(configuration, CreateDataSender(configuration));
            }

            return senders[configuration];
        }

        public void Dispose()
        {
            foreach (var sender in senders.Values)
            {
                sender.Dispose();
            }
        }

        private IDataSender CreateDataSender(IDataConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (configuration.File != null)
            {
                return new FileDataSender(serialiser, configuration.File, streamFactory);
            }

            if (configuration.Process != null)
            {
                return new ProcessDataSender(serialiser, configuration.Process);
            }

            throw new InvalidOperationException("No output configured, provide either 'file' or 'process' in the config file or use the `--out-to-files` switch to output data to individual files");
        }
    }
}
