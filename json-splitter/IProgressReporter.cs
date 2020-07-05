namespace json_splitter
{
    public interface IProgressReporter
    {
        void ReportEnd(int totalLines);
        void ReportProgress(int lineNumber);
    }
}