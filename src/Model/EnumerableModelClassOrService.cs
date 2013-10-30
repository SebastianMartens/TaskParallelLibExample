using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WpfApplication1.Infrastructure;

namespace WpfApplication1.Model
{
    /// <summary>
    /// placeholder for any entry point to the model (e.g. aggregate root) or service.
    /// </summary>    
    public class EnumerableModelClassOrService
    {
        private const int MaxItems = 3;

        /// <summary>
        /// possibly very long list of data that comes slowly from anywhere in the cloud...
        /// </summary>
        public async Task<List<string>> LongRunningGetDataMethodAsync()
        {
            // async methoden können direkt "awaitet" werden:
            // return await GetDataAsync();
            
            // Für "normale" Methoden muss ein neuer Task eröffnet werden:
            return await Task.Factory.StartNew(() => LongRunningGetDataMethod());
        }


        private static List<string> LongRunningGetDataMethod()
        {
            var list = new List<string>();
            for (var i = 0; i < MaxItems; i++)
            {
                Logger.Log("Waiting for data item...");
                Thread.Sleep(new TimeSpan(0, 0, 1));
                Logger.Log("Received a data item...");

                list.Add(String.Format("neue Daten erhalten ({0})", Guid.NewGuid()));
            }
            return list;
        }
    }
}
