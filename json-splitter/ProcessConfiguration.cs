namespace json_splitter
{
    public class ProcessConfiguration : IBindingConfiguration
    {
        public string FileName { get; set; }
        public string Arguments { get; set; }
        public string WorkingDirectory { get; set; }
        public RowFormat Format { get; set; } = RowFormat.Csv;
        public bool ColumnHeaders { get; set; } = true;

        public string ForeignKeyColumnName { get; set; }
        public string ForeignKeyPropertyName { get; set; }
    }
}