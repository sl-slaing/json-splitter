namespace json_splitter
{
    public interface IRelatedDataConfiguration : IDataConfiguration
    {
        string ForeignKeyColumnName { get; }
        string ForeignKeyPropertyName { get; }
    }
}
