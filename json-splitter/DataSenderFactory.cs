using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace json_splitter
{
    public class DataSenderFactory : IDataSenderFactory
    {
        private readonly Arguments args;
        private readonly JsonSerializer serialiser;
        private readonly Dictionary<IDataConfiguration, IDataSender> senders = new Dictionary<IDataConfiguration, IDataSender>();

        public DataSenderFactory(Arguments args, JsonSerializer serialiser)
        {
            this.args = args;
            this.serialiser = serialiser;
        }

        public IDataSender GetDataSender(IDataConfiguration configuration)
        {
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
            if (configuration.File != null)
            {
                return new FileDataSender(serialiser, configuration.File);
            }

            if (configuration.Process != null)
            {
                return new ProcessDataSender(serialiser, configuration.Process);
            }

            throw new InvalidOperationException("No output configured, provide either 'file' or 'process' in the config file or use the `--out-to-files` switch to output data to individual files");
        }
    }
}
