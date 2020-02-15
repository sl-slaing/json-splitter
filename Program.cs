using CommandLine;
using Newtonsoft.Json;
using System;

namespace json_splitter
{
    class Program
    {
        static void Main(string[] commandLineArgs)
        {
            Parser.Default.ParseArguments<Arguments>(commandLineArgs)
                   .WithParsed(args =>
                   {
                       var serialiser = new JsonSerializer();

                       var processor = new Processor(
                           args.Quiet
                                ? (IProgressReporter)new NoOpProgressReporter()
                                : new ProgressReporter(Console.Out),
                           serialiser,
                           new ConfigurationRepository(serialiser),
                           new DataProcessorFactory(new RelationalObjectReader()),
                           new DataSenderFactory(args, serialiser));
                       try
                       {
                           processor.Execute(args);
                       }
                       catch (Exception exc)
                       {
                           Console.Error.WriteLine(exc.ToString());
                       }
                   });
        }
    }
}
