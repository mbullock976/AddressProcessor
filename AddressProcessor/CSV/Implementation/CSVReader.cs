namespace AddressProcessing.CSV.Implementation
{
    using System;
    using System.IO;

    using AddressProcessing.CSV.Interfaces;

    /*
        2) Refactor this class into clean, elegant, rock-solid & well performing code, without over-engineering.
           Assume this code is in production and backwards compatibility must be maintained.
    */

    public class CSVReader : ICSVReader
    {
        private StreamReader reader;
        private bool disposed = false;

        public CSVReader(string fileName)
        {
            this.FileName = fileName;

            Initialise();
        }

        public string FileName { get; private set; }

        public bool Read(out string[] outputColumns, char[] separator)
        {
            var line = this.reader.ReadLine();
            if (line == null)
            {
                outputColumns = null;
                return false;
            }
            
            outputColumns = line.Split(separator);
            return true;
        }

        public void Close()
        {
            this.reader?.Close();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Initialise()
        {
            var stream = File.OpenRead(this.FileName);
            this.reader = new StreamReader(stream);
        }

        private void Dispose(bool isDisposing)
        {
            if (!this.disposed)
            {
                if (isDisposing)
                {
                    this.reader.Dispose();
                }

                this.disposed = true;
            }
        }
    }
}
