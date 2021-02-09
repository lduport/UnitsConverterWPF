using Prism.Commands;
using System.Collections.ObjectModel;
using System.Windows.Input;
using UnitsNet;

namespace UnitsConverterWPF.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    public class MainWindowDesignViewModel : IMainWindowViewModel
    {
        public MainWindowDesignViewModel()
        {
            Title = "Sample unit converter - DESIGN";
        }

        public string Title { get; set; }

        public bool ShowHistory { get; set; }

        public string SearchQuery { get; set; }

        public ObservableCollection<string> ConversionHistory { get; set; }
        public ReadOnlyObservableCollection<QuantityInfo> Measures { get; }

        public ReadOnlyObservableCollection<UnitInfo> Units => throw new System.NotImplementedException();

        public QuantityInfo SelectedMeasure { get; set; }

        public UnitInfo SelectedUnitFrom { get; set; }

        public UnitInfo SelectedUnitTo { get; set; }

        public DelegateCommand ToggleHistoryCommand { get; }

        public DelegateCommand InvertCommand { get; }

        public DelegateCommand CopyToClipboardCommand { get; }

        public DelegateCommand ClearHistoryCommand { get; }

        public string FromHeader => "From Header";

        public string ToHeader => "To Header";

        public decimal FromValue { get; set; }

        public decimal ToValue { get; }


    }
}
