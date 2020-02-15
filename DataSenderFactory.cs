using Newtonsoft.Json;
using System;

namespace json_splitter
{
    public class DataSenderFactory
    {
        private readonly Arguments args;
        private readonly JsonSerializer serialiser;

        public DataSenderFactory(Arguments args, JsonSerializer serialiser)
        {
            this.args = args;
            this.serialiser = serialiser;
        }

        public IDataSender GetDataSender(IDataConfiguration configuration)
        {
            if (args.FileOutput)
            {
                return new FileDataSender(serialiser);
            }

            if (configuration.Process != null)
            {
                return new ProcessDataSender(serialiser);
            }

            if (configuration.Sql != null)
            {
                return new SqlDataSender();
            }

            throw new InvalidOperationException("No output configured, provide either 'process' or 'sql' in the config file or use the `--out-to-files` switch to output data to individual files");
        }
    }
}
