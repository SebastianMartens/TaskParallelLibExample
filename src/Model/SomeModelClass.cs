using System.Collections.Generic;
using Microsoft.Practices.Prism.ViewModel;

namespace WpfApplication1.Model
{
    /// <summary>
    /// A domain model is usually a set of interconnected entity instances ("object graph").    
    /// This one holds a collection of associated objects (here: simple strings).
    /// 
    /// It is used to show how we can work on the model objects from multiple threads.
    /// </summary>
    public class SomeModelClass: NotificationObject
    {
        private List<string> _referencedObjectsCollection;

        /// <summary>
        /// Id property is not in use but here to demonstrate that the class should represent any kind of entity.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// This entity holds a collection of associated objects (here: simple strings).
        /// </summary>
        public List<string> ReferencedObjectsCollection
        {
            get { return _referencedObjectsCollection ?? (_referencedObjectsCollection = new List<string>()); }
            set { _referencedObjectsCollection = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public async void LoadDataIntoSharedDomainModelAsync()
        {
            var myDataCollection = await SomeStatelessService.LongRunningGetDataMethodAsync();
            
            // write-access to resources must be thread-safe!
            lock (this)
            {
                ReferencedObjectsCollection.AddRange(myDataCollection);
            }
        }

    }
}
