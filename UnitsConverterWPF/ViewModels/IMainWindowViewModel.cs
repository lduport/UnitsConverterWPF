using Prism.Commands;
using System.Collections.ObjectModel;
using System.Windows.Input;
using UnitsNet;

namespace UnitsConverterWPF.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    interface IMainWindowViewModel
    {
        string Title { get; set; }

        bool ShowHistory { get; set; }

        string SearchQuery { get; set; }

        ObservableCollection<string> ConversionHistory { get; set; }

        ReadOnlyObservableCollection<QuantityInfo> Measures { get; }

        ReadOnlyObservableCollection<UnitInfo> Units { get; }

        QuantityInfo SelectedMeasure { get; set; }

        UnitInfo SelectedUnitFrom { get; set; }

        UnitInfo SelectedUnitTo { get; set; }

        DelegateCommand ToggleHistoryCommand { get;}

        DelegateCommand InvertCommand { get; }

        DelegateCommand CopyToClipboardCommand { get; }

        DelegateCommand ClearHistoryCommand { get; }

        string FromHeader { get; }
        string ToHeader { get; }
        decimal FromValue { get; set; }
        decimal ToValue { get; }
    }
}
