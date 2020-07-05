using System;

namespace json_splitter
{
    public interface IOutputStream : IDisposable
    {
        void Write(IRelationalObject data);
    }
}
