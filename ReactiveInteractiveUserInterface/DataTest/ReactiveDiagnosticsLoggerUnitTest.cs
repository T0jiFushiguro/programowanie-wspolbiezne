using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
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
            tempFilePath = Path.Combine(Path.GetTempPath(), "logTest.txt");
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
        public void Constructor_ShouldInitializeLogger()
        {
            using (var logger = new ReactiveDiagnosticsLogger(tempFilePath))
            {
                Assert.IsNotNull(logger);
            }
        }

        [TestMethod]
        public void Log_ShouldEmitToInternalSubject()
        {
            // Arrange
            var logger = new ReactiveDiagnosticsLogger(tempFilePath);
            var subjectField = typeof(ReactiveDiagnosticsLogger)
                .GetField("loggerSubject", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            Assert.IsNotNull(subjectField, "Nie znaleziono pola loggerSubject przez refleksję.");

            var subject = subjectField.GetValue(logger) as ISubject<string>;
            Assert.IsNotNull(subject, "Nie można odczytać Subject.");

            string? received = null;
            var subscription = subject.Subscribe(value => received = value);

            // Act
            logger.Log("TEST_REFLECTION");

            // Assert
            Assert.AreEqual("TEST_REFLECTION", received, "Subject nie otrzymał wartości.");

            subscription.Dispose();
        }

        [TestMethod]
        public void Dispose_ShouldBeIdempotent()
        {
            var logger = new ReactiveDiagnosticsLogger(tempFilePath);
            logger.Dispose();

            try
            {
                logger.Dispose(); // powinno działać bez wyjątku
            }
            catch (Exception ex)
            {
                Assert.Fail("Dispose thrown exception on second call: " + ex.Message);
            }
        }


    }
}
