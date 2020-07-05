using CommandLine;
using System.Diagnostics.CodeAnalysis;

namespace json_splitter
{
    [ExcludeFromCodeCoverage]
    public class Arguments
    {
        [Option('c', "config", Required = true, HelpText = "The configuration file to use.")]
        public string ConfigFile { get; set; }

        [Option("input", Required = false, HelpText = "Read data from this filename")]
        public string File { get; set; }

        [Option("quiet", Required = false, HelpText = "Suppress progress reporting")]
        public bool Quiet { get; set; }
    }
}
