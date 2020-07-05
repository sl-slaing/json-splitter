using System;
using System.IO;

namespace json_splitter
{
    public class ProgressReporter : IProgressReporter
    {
        private readonly TextWriter writer;

        public ProgressReporter(TextWriter writer)
        {
            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            this.writer = writer;
        }

        public void ReportProgress(int lineNumber)
        {
            if (lineNumber % 10 == 0)
            {
                writer.WriteLine($"Reading line {lineNumber}...");
            }
        }

        public void ReportEnd(int totalLines)
        {
            writer.WriteLine($"Processed {totalLines} line/s");
        }
    }
}
