using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using WpfApplication1.Infrastructure;
using WpfApplication1.Model;

namespace WpfApplication1.UI
{
    /// <summary>
    /// Using https://www.nuget.org/packages/Microsoft.Bcl.Async for async/await support in .Net 4.0!!!
    /// > Install-Package Microsoft.Bcl.Async
    /// </summary>
    public class MainWindowViewModel: NotificationObject
    {
        // public "interface" to the view:
        public ICommand LoadDataIntoViewModelCommand { get; set; }                
        public CollectionViewSource DataCollectionView { get; private set; }

        // can be used to show/ hide busy indicator...
        // value types can be changed from any thread and will be updated correctly in the UI with databinding!
        public bool IsBusy
        {
            get { return _isBusy; }
            set 
            {
                _isBusy = value;
                RaisePropertyChanged(() => IsBusy);
            }
        }


        private readonly ObservableCollection<string> _collection;                         
        private bool _isBusy;


        /// <summary>
        /// 
        /// </summary>
        public MainWindowViewModel()
        {
            // wire up commands:
            LoadDataIntoViewModelCommand = new DelegateCommand(LoadDataIntoViewModel);

            // init members:            
            _collection = new ObservableCollection<string>();
            DataCollectionView = new CollectionViewSource { Source = _collection };
            IsBusy = false;                     
        }            

        /// <summary>
        /// In this snippet we use two seperate tasks to load data async. in the background.
        /// Data is NOT "stored" in any centralized domain model, instead it's only retrieved to be shown in the UI.
        ///
        /// => data synchronization with the main thread is done in the view model instance!
        /// </summary>
        private async void LoadDataIntoViewModel()
        {            
            Logger.Log("Command im ViewModel empfangen. Laden wird jetzt gestartet...");
            IsBusy = true;                

            var getDataTask = SomeStatelessService.LongRunningGetDataMethodAsync(); // task is spawn and started automatically...                         
            Logger.Log("Laden wurde gestartet. Main Thread läuft weiter und wartet auf Ergebnisse...");

            // it's Ok to have multiple tasks running at the same time:
            var getDataTask2 = SomeStatelessService.LongRunningGetDataMethodAsync(); 
            Logger.Log("Laden wurde ein zweites Mal gestartet. Main Thread läuft weiter und wartet auf Ergebnisse...");


            //------------------
            var list = await getDataTask;
            Logger.Log("Ergebnisse von Task 1 sind erfolgreich berechnet. Hier bin ich wieder im Main-Thread!");
            DataCollectionView.Source = list;   // remark: we do NOT change the collection in the background task. Instead we "pull" the data out of the task and use it in the UI-thread!
            

            //------------------
            list.AddRange(await getDataTask2);
            //var list2 = await getDataTask2;
            Logger.Log("Ergebnisse von Task 2 sind erfolgreich berechnet. Hier bin ich wieder im Main-Thread!");
            IsBusy = false;
            DataCollectionView.View.Refresh();
        }
      
    }
}