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

                       using (var senderFactory = new DataSenderFactory(args, serialiser))
                       {
                           var processor = new Processor(
                               args.Quiet
                                    ? (IProgressReporter)new NoOpProgressReporter()
                                    : new ProgressReporter(Console.Out),
                               serialiser,
                               new ConfigurationRepository(serialiser),
                               new DataProcessor(senderFactory, new RelationalObjectReader()));
                           try
                           {
                               processor.Execute(args);
                           }
                           catch (Exception exc)
                           {
                               Console.Error.WriteLine(exc.ToString());
                           }
                       }
                   });
        }
    }
}
