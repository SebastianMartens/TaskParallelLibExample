using System.Collections.Generic;
using Microsoft.Practices.Prism.ViewModel;

namespace WpfApplication1.Model
{
    /// <summary>    
    /// In contrast to the other example (using "SomeStatelessService.cs"), here we use the "Object Map" pattern.
    /// This means that the repo provides access to entity instances that may be used by multiple consumers and threads independently!        
    /// </summary>
    public class SomeRepository: NotificationObject
    {
        private List<object> _loadedObjectsOfThisWorkItemContext; // in real world apps this can be a Entity Framework- or NHibernate-datacontext.
        
        /// <summary>
        /// This entity holds a collection of associated objects (here: simple strings).
        /// </summary>
        public List<object> LoadedObjectsOfThisWorkItemContext
        {
            get { return _loadedObjectsOfThisWorkItemContext ?? (_loadedObjectsOfThisWorkItemContext = new List<object>()); }
        }

        /// <summary>
        ///
        /// </summary>
        public async void LoadDataIntoSharedDomainModelAsync()
        {
            var myDataCollection = await SomeStatelessService.LongRunningGetDataMethodAsync();
            
            // write-access to resources must be locked to be thread-safe! - Even simple assignments like "_a = 2".
            // Do only lock your classes' own resources.
            // Try not to call any other objects's method inside of the lock in order to prevent deadlocks (except side effect free ones).
            
            /* 
             Sometimes a shared resource is only written in one thread but read in multiple threads.
             An example could be a data cache that is filled by one "data load task" and that is used by multiple threads.
             Or image a queue of workItems that are processed by multiple workers.
             
             see http://www.nullskull.com/a/1464/producerconsumer-queue-and-blockingcollection-in-c-40.aspx):
             System.Collections.Concurrent.IProducerConsumerCollection<T>
                System.Collections.Concurrent.ConcurrentBag<T>
                System.Collections.Concurrent.ConcurrentQueue<T>
                System.Collections.Concurrent.ConcurrentStack<T>
            
                System.Collections.Concurrent.BlockingCollection<T>
                    System.Collections.Concurrent.ConcurrentDictionary<T>
            */

            // => see "producer-consumer" pattern and reader/writer locks.

            // Don't use:
            // Monitor, Mutex, Semaphore, Pulse-Method, manualResetEvent, ThreadPool. cancellatioins etc. pp

            // Remark: When reading from the collection, you'll have to catch an exception that occurs if the list is changed during enumeration!                        
            lock (_loadedObjectsOfThisWorkItemContext)
            {
                LoadedObjectsOfThisWorkItemContext.AddRange(myDataCollection);
            }
        }

    }
}
