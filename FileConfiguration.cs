namespace json_splitter
{
    public class FileConfiguration : IBindingConfiguration
    {
        public string FileName { get; set; }
        public RowFormat Format { get; set; } = RowFormat.Csv;
        public bool ColumnHeaders { get; set; } = true;

        public string ForeignKeyColumnName { get; set; }
        public string ForeignKeyPropertyName { get; set; }
    }
}