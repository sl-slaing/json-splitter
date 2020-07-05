using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace json_splitter
{
    public class Processor
    {
        private readonly IConfigurationRepository configRepository;
        private readonly IDataProcessor processor;
        private readonly IProgressReporter progress;
        private readonly JsonSerializer serialiser;

        public Processor(
            IProgressReporter progress,
            JsonSerializer serialiser,
            IConfigurationRepository configRepository,
            IDataProcessor processor)
        {
            if (serialiser == null)
            {
                throw new ArgumentNullException(nameof(serialiser));
            }

            if (progress == null)
            {
                throw new ArgumentNullException(nameof(progress));
            }

            if (configRepository == null)
            {
                throw new ArgumentNullException(nameof(configRepository));
            }

            if (processor == null)
            {
                throw new ArgumentNullException(nameof(processor));
            }

            this.serialiser = serialiser;
            this.configRepository = configRepository;
            this.processor = processor;
            this.progress = progress;
        }

        public void Execute(Arguments args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            var config = configRepository.ReadConfiguration(args.ConfigFile);

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

        private static bool IsNdJson(string trimmedLine)
        {
            return trimmedLine.StartsWith("{") && trimmedLine.EndsWith("}");
        }

        private static TextReader GetInput(Arguments args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

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
