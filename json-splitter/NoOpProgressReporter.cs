using System.Diagnostics.CodeAnalysis;

namespace json_splitter
{
    [ExcludeFromCodeCoverage]
    public class NoOpProgressReporter : IProgressReporter
    {
        public void ReportEnd(int totalLines)
        { }

        public void ReportProgress(int lineNumber)
        { }
    }
}