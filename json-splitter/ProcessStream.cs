using System;
using System.Diagnostics;
using System.Text;

namespace json_splitter
{
    public class ProcessStream : IDisposable
    {
        private readonly ProcessConfiguration config;
        private readonly IOutputStreamFactory outputFactory;
        private IOutputStream output;
        private Process process;

        public ProcessStream(ProcessConfiguration config, IOutputStreamFactory outputFactory)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            if (outputFactory == null)
            {
                throw new ArgumentNullException(nameof(outputFactory));
            }

            if (string.IsNullOrEmpty(config.FileName))
            {
                throw new ArgumentNullException("fileName", "Process filename must be supplied");
            }

            this.config = config;
            this.outputFactory = outputFactory;
        }

        public void Dispose()
        {
            output?.Dispose();

            if (process?.HasExited == false)
            {
                process?.WaitForExit();
            }

            process = null;
        }

        public void SendData(IRelationalObject relationalObject)
        {
            if (relationalObject == null)
            {
                throw new ArgumentNullException(nameof(relationalObject));
            }

            if (process == null)
            {
                StartProcess();
            }

            if (process.HasExited)
            {
                throw new InvalidOperationException($"Target process has exited: {process.Id} ({config.FileName} {config.Arguments})");
            }

            output.Write(relationalObject);
        }

        private void StartProcess()
        {
            var process = new Process
            {
                StartInfo =
                {
                    FileName = config.FileName,
                    Arguments = config.Arguments,
                    WorkingDirectory = config.WorkingDirectory,
                    CreateNoWindow = true,
                    RedirectStandardInput = true,
                    UseShellExecute = false,
                    StandardInputEncoding = Encoding.UTF8
                }
            };

            if (!process.Start())
            {
                throw new InvalidOperationException($"Could not start process: {config.FileName} {config.Arguments}");
            }

            output = outputFactory.Create(process.StandardInput);
            this.process = process;
        }
    }
}