namespace AddressProcessing.CSV.Implementation
{
    using AddressProcessing.CSV.Interfaces;

    public class CsvFileProviderFactory : ICSVFileProviderFactory
    {
        public ICSVReader CreateReader(string fileName)
        {
            return new CSVReader(fileName);
        }

        public ICSVWriter CreateWriter(string fileName)
        {
            return new CSVWriter(fileName);
        }
    }
}