using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace json_splitter
{
    public class SqlDataSender : IDataSender
    {
        private readonly Dictionary<StreamKey, ISqlDataStream> dataStreams = new Dictionary<StreamKey, ISqlDataStream>();
        private readonly SqlConfiguration configuration;
        private readonly IDataConfiguration dataConfig;

        public SqlDataSender(SqlConfiguration configuration, IDataConfiguration dataConfig)
        {
            this.configuration = configuration;
            this.dataConfig = dataConfig;
        }
        
        public void SendData(IRelationalObject relationalObject)
        {
            var key = new StreamKey(configuration.ConnectionString, configuration.TableName);
            if (!dataStreams.ContainsKey(key))
            {
                dataStreams.Add(key, CreateStream(relationalObject, key));
            }

            var bindingConfig = configuration as IBindingConfiguration;
            if (bindingConfig != null)
            {
                relationalObject = relationalObject.WithForeignKey(bindingConfig);
            }

            var stream = dataStreams[key];
            stream.PushData(relationalObject);
        }

        public void Dispose()
        {
            foreach (var stream in dataStreams.Values)
            {
                stream.Dispose();
            }
        }

        private ISqlDataStream CreateStream(IRelationalObject relationalObject, StreamKey key)
        {
            //setup SqlBulkCopy for the given table
            //create and return an empty datareader
            var bulkCopy = new SqlBulkCopy(key.ConnectionString);
            bulkCopy.DestinationTableName = key.TableName;
            bulkCopy.EnableStreaming = true;
            AddColumnMappings(bulkCopy, relationalObject);
            bulkCopy.BatchSize = configuration.BatchSize;

            var stream = new SqlDataStream(dataConfig, bulkCopy);
            bulkCopy.WriteToServer(stream);

            return stream;
        }

        private void AddColumnMappings(SqlBulkCopy bulkCopy, IRelationalObject relationalObject)
        {
            var index = 0;
            foreach (var column in relationalObject.Data)
            {
                bulkCopy.ColumnMappings.Add(index++, column.Key);
            }
        }

        private class StreamKey
        {
            public string ConnectionString { get; }
            public string TableName { get; }

            public StreamKey(string connectionString, string tableName)
            {
                ConnectionString = connectionString;
                TableName = tableName;
            }

            public override bool Equals(object obj)
            {
                return obj is StreamKey key &&
                       ConnectionString == key.ConnectionString &&
                       TableName == key.TableName;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(ConnectionString, TableName);
            }
        }
    }
}
