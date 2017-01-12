namespace AddressProcessing.Tests.CSV.UnitTests
{
    using System.Configuration;

    using AddressProcessing.CSV;
    using AddressProcessing.CSV.Interfaces;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using NSubstitute;
    using NSubstitute.Core;
    using NSubstitute.Core.Arguments;

    [TestClass]
    public class CSVReaderWriterShould
    {
        private CSVReaderWriter csvReaderWriter;
        private ICSVFileProviderFactory csvFileProviderFactory;

        //test overloaded reads that csvReader.Read was called and columns came out and true
        //negative test overloaded reads columns back out as null and false

        //test write method that csvWriter.Write was called and columns came out and true

        [TestInitialize]
        public void Setup()
        {
            this.csvFileProviderFactory = Substitute.For<ICSVFileProviderFactory>();
            this.csvReaderWriter = new CSVReaderWriter(this.csvFileProviderFactory);
        }

        [TestMethod]
        public void OpenCSVFileToRead()
        {
            //Arrange
            var fileName = "fileToReadPath";
            //Act
            this.csvReaderWriter.Open(fileName, CSVReaderWriter.Mode.Read);

            //Assert
            this.csvFileProviderFactory.Received(1).CreateReader(fileName);
            this.csvFileProviderFactory.DidNotReceive().CreateWriter(fileName);
        }

        [TestMethod]
        public void OpenCSVFileToWrite()
        {
            //Arrange
            var fileName = "fileToWritePath";

            //Act
            this.csvReaderWriter.Open(fileName, CSVReaderWriter.Mode.Write);
            

            //Assert
            this.csvFileProviderFactory.Received(1).CreateWriter(fileName);
            this.csvFileProviderFactory.DidNotReceive().CreateReader(fileName);
        }

        [TestMethod]
        public void CloseCSVReaderAndWriter()
        {
            //Arrange
            var fileName = "fileToWritePath";
            var csvReader = AssumeCSVReaderIsCreated(fileName);
            var csvWriter = AssumeCSVWriterIsCreated(fileName);

            this.csvReaderWriter.Open(fileName, CSVReaderWriter.Mode.Read);
            this.csvReaderWriter.Open(fileName, CSVReaderWriter.Mode.Write);

            //Act
            this.csvReaderWriter.Close();

            //Assert
            csvReader.Received(1).Close();
            csvWriter.Received(1).Close();
        }


        [TestMethod]
        public void DisposeReaderAndWriter()
        {
            var fileName = "fileToWritePath";
            var csvReader = AssumeCSVReaderIsCreated(fileName);
            var csvWriter = AssumeCSVWriterIsCreated(fileName);

            this.csvReaderWriter.Open(fileName, CSVReaderWriter.Mode.Read);
            this.csvReaderWriter.Open(fileName, CSVReaderWriter.Mode.Write);

            this.csvReaderWriter.Dispose();

            csvReader.Received(1).Dispose();
            csvWriter.Received(1).Dispose();
        }

        [TestMethod]
        public void NotReadColumnsFromInvalidCSVFile()
        {
            var fileName = "fileToWritePath";
            var csvReader = AssumeCSVReaderIsCreated(fileName);

            this.csvReaderWriter.Open(fileName, CSVReaderWriter.Mode.Read);

            string column1 = null;
            string column2 = null;
            var result = this.csvReaderWriter.Read(out column1, out column2);

            Assert.AreEqual(false, result);
            Assert.AreEqual(null, column1);
            Assert.AreEqual(null, column2);

        }        

        [TestCleanup]
        public void TearDown()
        {
            this.csvReaderWriter.Close();
            this.csvReaderWriter.Dispose();
        }

        private ICSVReader AssumeCSVReaderIsCreated(string fileName)
        {
            var csvReader = Substitute.For<ICSVReader>();

            this.csvFileProviderFactory
             .CreateReader(fileName)
             .Returns(csvReader);

            return csvReader;
        }

        private ICSVWriter AssumeCSVWriterIsCreated(string fileName)
        {
            var csvWriter = Substitute.For<ICSVWriter>();

            this.csvFileProviderFactory
                .CreateWriter(fileName)
                .Returns(csvWriter);

            return csvWriter;
        }
    }
}
