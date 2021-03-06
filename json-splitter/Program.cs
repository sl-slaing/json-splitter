﻿using CommandLine;
using Newtonsoft.Json;
using System;

namespace json_splitter
{
    public class Program
    {
        public static void Main(string[] commandLineArgs)
        {
            Parser.Default.ParseArguments<Arguments>(commandLineArgs)
                   .WithParsed(args =>
                   {
                       var serialiser = new JsonSerializer();

                       using (var senderFactory = new DataSenderFactory(serialiser, new StreamFactory()))
                       {
                           var processor = new Processor(
                               args.Quiet
                                    ? (IProgressReporter)new NoOpProgressReporter()
                                    : new ProgressReporter(Console.Out),
                               serialiser,
                               new ConfigurationRepository(serialiser),
                               new DataProcessor(senderFactory, new RelationalObjectReader()),
                               new InputFactory());
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
