namespace AddressProcessing.Tests.CSV.IntegrationTests
{
    using System.IO;

    using AddressProcessing.CSV;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CSVReaderWriterShould
    {
        private CSVReaderWriter csvReaderWriter;

        [TestInitialize]
        public void Setup()
        {
            this.csvReaderWriter = new CSVReaderWriter();
        }

        [TestMethod]
        public void ReadFromCSVFile()
        {
            this.csvReaderWriter.Open(@"..\..\test_data\contacts.csv", CSVReaderWriter.Mode.Read);

            string column1, column2;
            this.csvReaderWriter.Read(out column1, out column2);

            Assert.IsNotNull(column1);
            Assert.IsNotNull(column2);
        }


        [TestMethod]
        public void WriteToCSVFile()
        {
            var filename = @"..\..\test_data\contacts.csv";
            char[] separator = { '\t' };

            using (this.csvReaderWriter)
            {
                this.csvReaderWriter.Open(filename, CSVReaderWriter.Mode.Write);

                string column1 = "Michael Bullock", column2 = "976 Luton Street";
                this.csvReaderWriter.Write(column1, column2);              
            }

            var found = false;
            using (var streamReader = new StreamReader(new FileStream(filename, FileMode.Open)))
            {
                while (!streamReader.EndOfStream)
                {
                    var line = streamReader.ReadLine();
                    var columns = line.Split(separator);
                    if (columns.Length == 0)
                    {
                        break;
                    }

                    if (columns[0] == "Michael Bullock")
                    {
                        found = true;
                    }
                }
            }        
            
            Assert.IsTrue(found);                                  
        }


        [TestCleanup]
        public void CleanUp()
        {
            this.csvReaderWriter.Close();
            this.csvReaderWriter.Dispose();   
        }
    }
}