namespace AddressProcessing.CSV.Interfaces
{
    using System;
    public interface ICSVWriter : IDisposable
    {
        void Write(string[] columns, string separartor);

        void Close();
    }
}