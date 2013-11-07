using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WpfApplication1.Infrastructure;

namespace WpfApplication1.Model
{
    /// <summary>
    /// This service delivers data. Nevertheless it's stateless (data is not stored inside of this class).
    /// 
    /// This example service is "static". But be aware that this is dangerous in bigger classes as all static fields are shared among threads and therefore are targets of concurrency issues!
    /// 
    /// You are save in therms of thread safety as long as each thread uses its own instances of objects and each object only changes it's own fields. Try not to access shared resources at all!
    /// 
    /// See concepts => Immutable objects (can be passed around safely between threads. They have no state that must be locked for concurrent access!)
    ///              => Side Effect Free Functions (does not change anything outside of it - even if you have to lock something, you have to know what to lock...)
    ///              => Mathematical Functions / Functional Programming (F#) ("convert input item from domain into output item from range"; 
    ///                 A function always gives the same output value for a given input value ("idempotence").)
    ///                 Benefits:
    ///                 - full parallelizable
    ///                 - lazy
    ///                 - only evaluate once
    ///                 - execution order can be ignored
    ///                 - automatically reentrant (can be interrupted an re-entered)
    /// </summary>    
    public static class SomeStatelessService
    {
        private const int MaxItems = 3;

        /// <summary>
        /// possibly very long list of data that comes slowly from anywhere in the cloud...
        /// </summary>
        public static async Task<List<string>> LongRunningGetDataMethodAsync()
        {
            // async methods can be "awaited" directly:
            // return await GetDataAsync();
            
            // Spawn new task to start non-async methods: 
            return await Task.Factory.StartNew(() => LongRunningGetDataMethod());
        }


        private static List<string> LongRunningGetDataMethod()
        {
            var list = new List<string>();
            for (var i = 0; i < MaxItems; i++)
            {
                Logger.Log("Waiting for data item...");
                Thread.Sleep(new TimeSpan(0, 0, 4));
                Logger.Log("Received a data item...");

                list.Add(String.Format("neue Daten erhalten ({0})", Guid.NewGuid()));
            }
            return list;
        }
    }
}
