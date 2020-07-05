using Newtonsoft.Json;
using System;
using System.IO;

namespace json_splitter
{
    public class FileStream : IDisposable
    {
        private readonly IOutputStream output;

        public FileStream(IOutputStream output)
        {
            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            this.output = output;
        }

        public void Dispose()
        {
            output.Dispose();
        }

        public void SendData(IRelationalObject relationalObject)
        {
            if (relationalObject == null)
            {
                throw new ArgumentNullException(nameof(relationalObject));
            }

            output.Write(relationalObject);
        }
    }
}