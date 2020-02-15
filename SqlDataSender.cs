using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace json_splitter
{
    public class SqlDataSender : IDataSender
    {
        private readonly Dictionary<StreamKey, IDataStream> dataStreams = new Dictionary<StreamKey, IDataStream>();
        
        public void SendData(IRelatedDataConfiguration config, IRelationalObject relationalObject)
        {
            var key = new StreamKey(config.Sql.ConnectionString, config.TableName);
            if (!dataStreams.ContainsKey(key))
            {
                dataStreams.Add(key, CreateStream(config, relationalObject));
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

        private IDataStream CreateStream(IRelatedDataConfiguration config, IRelationalObject relationalObject)
        {
            //setup SqlBulkCopy for the given table
            //create and return an empty datareader
            var bulkCopy = new SqlBulkCopy(config.Sql.ConnectionString);
            bulkCopy.DestinationTableName = config.TableName;
            bulkCopy.EnableStreaming = true;
            AddColumnMappings(bulkCopy, relationalObject);
            bulkCopy.BatchSize = config.Sql.BatchSize;

            var stream = new DataStream(config, bulkCopy);
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
