using System;
using System.IO;

namespace json_splitter
{
    public class InputFactory : IInputFactory
    {
        public TextReader GetInput(Arguments args)
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
