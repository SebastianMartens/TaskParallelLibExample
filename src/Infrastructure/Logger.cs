using System.Diagnostics;
using System.Threading;

namespace WpfApplication1.Infrastructure
{
    /// <summary>
    /// Auxiliasry class to add thread infos to debug messages...
    /// </summary>
    public static class Logger
    {
        public static void Log(string message)
        {                  
            Debug.WriteLine(message + " (Thread=" + Thread.CurrentThread.ManagedThreadId + ")");
        }
    }
}
