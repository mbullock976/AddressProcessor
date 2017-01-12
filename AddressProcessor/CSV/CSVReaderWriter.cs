namespace AddressProcessing.CSV
{
    using System;
    using System.IO;

    using AddressProcessing.CSV.Implementation;
    using AddressProcessing.CSV.Interfaces;

    public class CSVReaderWriter : IDisposable
    {
        private readonly ICSVFileProviderFactory csvFileProviderFactory;

        private ICSVReader reader = null;
        private ICSVWriter writer = null;

        private bool disposed = false;

        public CSVReaderWriter()
            : this(new CsvFileProviderFactory())
        {            
        }

        public CSVReaderWriter(
            ICSVFileProviderFactory csvFileProviderFactory)
        {
            this.csvFileProviderFactory = csvFileProviderFactory;
        }

        [Flags]
        public enum Mode
        {
            Read = 1,
            Write = 2
        }

        public void Open(string fileName, Mode mode)
        {
            switch (mode)
            {
                case Mode.Read:
                    //if using IOC container Dependency Injection then we can remove the object dependency here
                    //and inject a factory interface that would be responsbile for returning a CSVReader or Writer for unit testing.
                    this.reader = this.csvFileProviderFactory.CreateReader(fileName);
                    break;

                case Mode.Write:                    
                    this.writer = this.csvFileProviderFactory.CreateWriter(fileName);
                    break;
            }
        }

        public void Write(params string[] columns)
        {
            var separator = "\t";

            //future resusable read to handle comma separator
            this.writer.Write(columns, separator); 
        }

        public bool Read(string column1, string column2)
        {
            //this method is misleading as this overloaded method does not give the caller 
            //any upfront information that this object will modify the values passed in.
            //Also no need to pass these in as we are trying to read out 2 columns from file.
            
            //removed repetitive code
            return Read(out column1, out column2);
        }

        public bool Read(out string column1, out string column2)
        {
            const int FIRST_COLUMN = 0;
            const int SECOND_COLUMN = 1;

            char[] separator = { '\t' };

            string[] outputColumns;
            this.reader.Read(out outputColumns, separator); //future resusable read to handle comma separator

            if (outputColumns == null || outputColumns.Length < 2)
            {
                column1 = null;
                column2 = null;

                return false;
            }

            column1 = outputColumns[FIRST_COLUMN];
            column2 = outputColumns[SECOND_COLUMN];

            return true;
        }

        public void Close()
        {
            this.writer?.Close();
            this.reader?.Close();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool isDisposing)
        {
            if (!this.disposed)
            {
                if (isDisposing)
                {
                    this.reader?.Dispose();
                    this.writer?.Dispose();
                }

                this.disposed = true;
            }
        }
    }
}