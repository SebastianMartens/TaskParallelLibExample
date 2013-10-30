using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using WpfApplication1.Infrastructure;
using WpfApplication1.Model;

namespace WpfApplication1.UI
{
    /// <summary>
    /// Using https://www.nuget.org/packages/Microsoft.Bcl.Async for async/await support in .Net 4.5!!!
    /// > Install-Package Microsoft.Bcl.Async
    /// </summary>
    public class MainWindowViewModel: NotificationObject
    {
        // public "interface" to the view:
        public ICommand LoadDataCommand { get; set; }                
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
        private readonly EnumerableModelClassOrService _enumerableModelClassOrService;
        private bool _isBusy;


        /// <summary>
        /// 
        /// </summary>
        public MainWindowViewModel()
        {
            // wire up commands:
            LoadDataCommand = new DelegateCommand(LoadData);

            // init members:            
            _collection = new ObservableCollection<string>();
            DataCollectionView = new CollectionViewSource { Source = _collection };
            IsBusy = false;
            _enumerableModelClassOrService = new EnumerableModelClassOrService(); // would normally be injected into constructor...
            
        }            

        /// <summary>
        /// Variante, bei der das ViewModel das Laden der Daten an einen async. Hintergrundprozess delegiert.
        /// </summary>
        private async void LoadData()
        {            
            Logger.Log("Command im ViewModel empfangen. Laden wird jetzt gestartet...");
            IsBusy = true;                

            var getDataTask = _enumerableModelClassOrService.LongRunningGetDataMethodAsync(); // task is spawn and started automatically...                         
            Logger.Log("Laden wurde gestartet. Main Thread läuft weiter und wartet auf Ergebnisse...");


            //------------------
            var list = await getDataTask;
            Logger.Log("Ergebnisse sind erfolgreich berechnet. Hier bin ich wieder im Main-thread!");
            IsBusy = false;            
            DataCollectionView.Source = list;               
        }
      
    }
}