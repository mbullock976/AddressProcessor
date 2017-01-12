namespace AddressProcessing.CSV.Interfaces
{
    public interface ICSVFileProviderFactory
    {
        ICSVReader CreateReader(string fileName);

        ICSVWriter CreateWriter(string fileName);
    }
}