using System;

namespace json_splitter
{
    public interface IDataSenderFactory : IDisposable
    {
        IDataSender GetDataSender(IDataConfiguration configuration);
    }
}