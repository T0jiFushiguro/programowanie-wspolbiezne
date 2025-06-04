using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Subjects;
using System.Reactive.Linq;

namespace TP.ConcurrentProgramming.Data
{
    internal class ReactiveDiagnosticsLogger : IDisposable
    {
        private readonly Subject<string> loggerSubject = new Subject<string>();
        private readonly IDisposable subscription;
        private readonly SemaphoreSlim fileLock = new SemaphoreSlim(1, 1);

        private bool disposed = false;
        public ReactiveDiagnosticsLogger(string filePath)
        {
            subscription = loggerSubject.Buffer(TimeSpan.FromMilliseconds(100), 100)
                .Sample(TimeSpan.FromMilliseconds(500))
                .Where(x => x.Count > 0)
                .Select(x => string.Join(Environment.NewLine, x))
                .Subscribe(async text => {

                    await fileLock.WaitAsync();
                    try
                    {
                        await File.AppendAllTextAsync(filePath, text + Environment.NewLine, Encoding.ASCII);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                    finally 
                    {
                        fileLock.Release();
                    }

                
                });
        }

        public void Log(object dataLog)
        {
            string serialized = SerializeToAscii(dataLog);
            loggerSubject.OnNext(serialized);
        }

        private string SerializeToAscii(object data)
        {
            string text = data.ToString() ?? string.Empty;
            var asciiBytes = Encoding.ASCII.GetBytes(text);
            return Encoding.ASCII.GetString(asciiBytes);
        }


        public void Dispose()
        {
            if (disposed) return;

            loggerSubject.OnCompleted();
            subscription.Dispose();
            loggerSubject.Dispose();
            fileLock.Dispose();

            disposed = true;
        }
    }
}
