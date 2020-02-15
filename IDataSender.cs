using System;

namespace json_splitter
{
    public interface IDataSender : IDisposable
    {
        void SendData(IRelatedDataConfiguration config, IRelationalObject relationalObject);
    }
}
