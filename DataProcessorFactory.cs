namespace json_splitter
{
    public class DataProcessorFactory
    {
        private readonly RelationalObjectReader relationalObjectReader;

        public DataProcessorFactory(RelationalObjectReader relationalObjectReader)
        {
            this.relationalObjectReader = relationalObjectReader;
        }

        public IDataProcessor CreateProcessor(IDataSender sender)
        {
            return new DataProcessor(sender, relationalObjectReader);
        }
    }
}
