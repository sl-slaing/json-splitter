namespace json_splitter
{
    public class SqlConfiguration : IBindingConfiguration
    {
        public string ForeignKeyColumnName { get; set; }
        public string ForeignKeyPropertyName { get; set; }
        public string ConnectionString { get; set; }
        public int BatchSize { get; set; }
        public string TableName { get; set; }
    }
}
