using System.IO;

namespace json_splitter
{
    public class ProgressReporter : IProgressReporter
    {
        private readonly TextWriter writer;

        public ProgressReporter(TextWriter writer)
        {
            this.writer = writer;
        }

        public void ReportProgress(int lineNumber)
        {
            writer.WriteLine($"Reading line {lineNumber}...");
        }

        public void ReportEnd(int totalLines)
        {
            writer.WriteLine($"Processed {totalLines} line/s");
        }
    }
}
