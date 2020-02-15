using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace json_splitter
{
    public class Processor
    {
        private readonly ConfigurationRepository configRepository;
        private readonly DataProcessorFactory dataProcessorFactory;
        private readonly DataSenderFactory dataSenderFactory;
        private readonly IProgressReporter progress;
        private readonly JsonSerializer serialiser;

        public Processor(
            IProgressReporter progress,
            JsonSerializer serialiser,
            ConfigurationRepository configRepository,
            DataProcessorFactory dataProcessorFactory,
            DataSenderFactory dataSenderFactory)
        {
            this.serialiser = serialiser;
            this.configRepository = configRepository;
            this.dataProcessorFactory = dataProcessorFactory;
            this.dataSenderFactory = dataSenderFactory;
            this.progress = progress;
        }

        public void Execute(Arguments args)
        {
            var config = configRepository.ReadConfiguration(args.ConfigFile);
            using (var sender = dataSenderFactory.GetDataSender(config))
            {
                var processor = dataProcessorFactory.CreateProcessor(sender);

                string line;
                var inputData = GetInput(args);
                var lineNumber = 0;

                while ((line = inputData.ReadLine()) != null)
                {
                    progress.ReportProgress(++lineNumber);
                    if (!IsNdJson(line.Trim()))
                    {
                        throw new InvalidOperationException($"Received incompatible data in input stream.\nLine {lineNumber} isn't in NDJSON format\n`{line}`");
                    }

                    var jsonObject = (JObject)serialiser.Deserialize(new JsonTextReader(new StringReader(line)));
                    processor.ProcessData(config, jsonObject);
                }

                progress.ReportEnd(lineNumber);
            }
        }

        private static bool IsNdJson(string trimmedLine)
        {
            return trimmedLine.StartsWith("{") && trimmedLine.EndsWith("}");
        }

        private static TextReader GetInput(Arguments args)
        {
            if (Console.IsInputRedirected)
            {
                return Console.In;
            }

            if (args.File != null)
            {
                return new StreamReader(args.File);
            }

            throw new InvalidOperationException("No input provided. Use --input <file> or pipe input into this process\nInput data must be in NDJSON format.");
        }
    }
}
