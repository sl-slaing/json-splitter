namespace json_splitter
{
    public class ProcessConfiguration
    {
        public string FileName { get; set; }
        public string Arguments { get; set; }
        public string WorkingDirectory { get; set; }
        public RowFormat Format { get; set; } = RowFormat.Csv;
        public bool CsvHeader { get; set; } = true;
    }
}