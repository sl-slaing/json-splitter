namespace json_splitter
{
    public interface IBindingConfiguration
    {
        string ForeignKeyColumnName { get; set; }
        string ForeignKeyPropertyName { get; set; }
    }
}
