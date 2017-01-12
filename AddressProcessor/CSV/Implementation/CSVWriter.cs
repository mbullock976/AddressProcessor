namespace AddressProcessing.CSV.Implementation
{
    using System;
    using System.IO;

    using AddressProcessing.CSV.Interfaces;

    public class CSVWriter : ICSVWriter
    {
        private readonly string fileName;
        private StreamWriter writer;
        private bool disposed = false;

        public CSVWriter(string fileName)
        {
            this.fileName = fileName;

            Initialise();
        }
    
        public void Write(string[] columns, string separator)
        {
            var output = string.Join(separator, columns);
            this.writer.WriteLine(output);
        }

        public void Close()
        {
            this.writer?.Close();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Initialise()
        {
            var stream = File.OpenWrite(this.fileName);
            this.writer = new StreamWriter(stream);
        }

        private void Dispose(bool isDisposing)
        {
            if (!this.disposed)
            {
                if (isDisposing)
                {
                    this.writer.Dispose();
                }

                this.disposed = true;
            }
        }
    }
}