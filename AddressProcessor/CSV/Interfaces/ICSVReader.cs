namespace AddressProcessing.CSV.Interfaces
{
    using System;
    public interface ICSVReader : IDisposable
    {
        bool Read(out string[] outputColumns, char[] separator);

        void Close();

        string FileName { get; }
    }
}