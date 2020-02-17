using System;

namespace json_splitter
{
    public interface IDataSender : IDisposable
    {
        void SendData(IRelationalObject relationalObject);
    }
}
