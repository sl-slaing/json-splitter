using CommandLine;

namespace json_splitter
{
    public class Arguments
    {
        [Option('c', "config", Required = true, HelpText = "The configuration file to use.")]
        public string ConfigFile { get; set; }

        [Option("out-to-files", Required = false, HelpText = "Send all data to individual JSON files")]
        public bool FileOutput { get; set; }

        [Option("input", Required = false, HelpText = "Read data from this filename")]
        public string File { get; set; }

        [Option("quiet", Required = false, HelpText = "Suppress progress reporting")]
        public bool Quiet { get; set; }
    }
}
