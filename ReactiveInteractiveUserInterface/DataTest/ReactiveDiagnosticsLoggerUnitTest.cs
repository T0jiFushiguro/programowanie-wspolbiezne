using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP.ConcurrentProgramming.Data;
using static System.Net.Mime.MediaTypeNames;

namespace TP.ConcurrentProgramming.DataTest
{
    [TestClass]
    public class ReactiveDiagnosticsLoggerUnitTest
    {
        private string tempFilePath = string.Empty;

        [TestInitialize]
        public void TestInitialize()
        {
            tempFilePath = "logTest.txt";
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (File.Exists(tempFilePath))
            {
                File.Delete(tempFilePath);
            }
        }

        [TestMethod]
        public async Task Constructor_ShouldInitializeLogger()
        {
            using (var logger = new ReactiveDiagnosticsLogger(tempFilePath))
            {
                Assert.IsNotNull(logger);
            }
        }

        [TestMethod]
        public async Task Log()
        {
            var logger = new ReactiveDiagnosticsLogger(tempFilePath);
            Assert.IsNotNull(logger);
            logger.Log("Tekst");
            logger.Dispose();
            Assert.IsNotNull(logger);

        }

        [TestMethod]
        public void Dispose_ShouldBeIdempotent()
        {
            var logger = new ReactiveDiagnosticsLogger(tempFilePath);
            logger.Dispose();

            // Drugie wywołanie Dispose nie powinno rzucać wyjątkiem
            try
            {
                logger.Dispose();
            }
            catch (Exception ex)
            {
                Assert.Fail("Dispose thrown exception on second call: " + ex.Message);
            }
        }
    }
}
