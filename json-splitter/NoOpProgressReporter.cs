namespace json_splitter
{
    public class NoOpProgressReporter : IProgressReporter
    {
        public void ReportEnd(int totalLines)
        { }

        public void ReportProgress(int lineNumber)
        { }
    }
}