using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Windows;
using UnitsConverterWPF.Events;
using UnitsConverterWPF.Services;
using UnitsNet;

namespace UnitsConverterWPF.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    public class MainWindowViewModel : ReactiveObject, IMainWindowViewModel
    {
        private string _title;
        private QuantityInfo _selectedMeasure;
        private UnitInfo _selectedUnitFrom;
        private UnitInfo _selectedUnitTo;
        private bool _showHistory;
        private string _searchQuery;
        private decimal _fromValue;
        private decimal _toValue;
        readonly ObservableAsPropertyHelper<string> _toHeader;
        readonly ObservableAsPropertyHelper<string> _fromHeader;


        private DelegateCommand _toggleHistoryCommand;
        private DelegateCommand<string> _searchMeasuresCommand;
        private DelegateCommand<QuantityInfo> _searchUnitsCommand;
        private DelegateCommand _invertCommand;
        private DelegateCommand _copyToClipboardCommand;
        private DelegateCommand _clearHistory;

        private IMeasureService _measureService;
        private IEventAggregator _eventAggregator;

        public MainWindowViewModel(
            IMeasureService measureService, 
            IEventAggregator eventAggregator)
        {
            _measureService = measureService ?? throw new ArgumentNullException(nameof(measureService));
            _eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));

            ConversionHistory = new ObservableCollection<string>();

            // Subscribe to MeasureUnitValueConverted events so the conversion will be saved in the conversion history
            _eventAggregator.GetEvent<MeasureUnitValueConverted>().Subscribe(OnMeasureUnitConverted, ThreadOption.UIThread);

            Title = "Sample unit converter";

            // Loads all measure on creation
            SearchMeasuresCommand.Execute(null);

            // Whenever the SelectedUnitFrom change, we update the FromHeader property
            _fromHeader = this
                .WhenAnyValue(x => x.SelectedUnitFrom)
                .Where(x => x != null)
                .Select(unitInfo =>
                    {
                        var abbreviation = UnitAbbreviationsCache.Default.GetDefaultAbbreviation(unitInfo.Value.GetType(), Convert.ToInt32(unitInfo.Value));
                        return $"Value [{abbreviation}]";
                    })
                .ToProperty(this, x => x.FromHeader);

            // Whenever the SelectedUnitTo change, we update the ToHeader property
            _toHeader = this
                .WhenAnyValue(x => x.SelectedUnitTo)
                .Where(x => x != null)
                .Select(unitInfo =>
                {
                    var abbreviation = UnitAbbreviationsCache.Default.GetDefaultAbbreviation(unitInfo.Value.GetType(), Convert.ToInt32(unitInfo.Value));
                    return $"Value [{abbreviation}]";
                })
                .ToProperty(this, x => x.ToHeader);


            // A change on the SearchQuery will trigger the SearchMeasuresCommand
            this.WhenAnyValue(x => x.SearchQuery)
                .Throttle(TimeSpan.FromSeconds(0.8), RxApp.TaskpoolScheduler)
                .Select(query => query?.Trim())
                .DistinctUntilChanged()
                .ObserveOn(RxApp.MainThreadScheduler)
                .InvokeCommand(SearchMeasuresCommand);

            // Whenever the SelectedMeasure change, we set the FromValue back to zero
            this.WhenAnyValue(x => x.SelectedMeasure)
                .Subscribe(measure => FromValue = 0);

            // A change on the SelectedMeasure will trigger the SearchUnitCommand
            this.WhenAnyValue(x => x.SelectedMeasure)
                .DistinctUntilChanged()
                .ObserveOn(RxApp.MainThreadScheduler)
                .InvokeCommand(SearchUnitCommand);

            // Whenever a change occurs on SelectedUnitFrom, SelectedUnitTo or FromValue, we update the ToValue
            // Also a MeasureUnitValueConverted will be published
            this.WhenAnyValue(
                x => x.SelectedUnitFrom,
                x => x.SelectedUnitTo,
                x => x.FromValue)
                .Subscribe(values =>
                {

                    if (values.Item1 == null || values.Item2 == null) return;

                    double convertedValue = _measureService.Convert(values.Item3, values.Item1.Value,values.Item2.Value);

                    if (values.Item3 != decimal.Zero)
                    {
                        ToValue = Convert.ToDecimal(convertedValue);

                        var unitFromAbbreviation = UnitAbbreviationsCache.Default.GetDefaultAbbreviation(values.Item1.Value.GetType(), Convert.ToInt32(values.Item1.Value));
                        var unitToAbbreviation = UnitAbbreviationsCache.Default.GetDefaultAbbreviation(values.Item2.Value.GetType(), Convert.ToInt32(values.Item2.Value));

                        _eventAggregator.GetEvent<MeasureUnitValueConverted>().Publish($"{values.Item3}[{unitFromAbbreviation}] equals {ToValue}[{unitToAbbreviation}] ");
                    }
                    else
                    {
                        ToValue = decimal.Zero;
                    }
                });
        }

        private void OnMeasureUnitConverted(string message)
        {
            ConversionHistory.Add(message);
        }

        public string Title
        {
            get { return _title; }
            set { this.RaiseAndSetIfChanged(ref _title, value); }
        }

        public string SearchQuery
        {
            get { return _searchQuery; }
            set { this.RaiseAndSetIfChanged(ref _searchQuery, value); }
        }

        public bool ShowHistory
        {
            get { return _showHistory; }
            set { this.RaiseAndSetIfChanged(ref _showHistory, value); }
        }

        public string FromHeader => _fromHeader.Value;

        public string ToHeader => _toHeader.Value;

        public decimal FromValue
        {
            get { return _fromValue; }
            set { this.RaiseAndSetIfChanged(ref _fromValue, value); }
        }

        public decimal ToValue
        {
            get { return _toValue; }
            private set { this.RaiseAndSetIfChanged(ref _toValue, value); }
        }

        public ObservableCollection<string> ConversionHistory { get; set; }

        public ReadOnlyObservableCollection<QuantityInfo> Measures { get; set; }

        public ReadOnlyObservableCollection<UnitInfo> Units { get; set; }

        public QuantityInfo SelectedMeasure
        {
            get { return _selectedMeasure; }
            set { this.RaiseAndSetIfChanged(ref _selectedMeasure, value); }
        }

        public UnitInfo SelectedUnitFrom
        {
            get { return _selectedUnitFrom; }
            set { this.RaiseAndSetIfChanged(ref _selectedUnitFrom, value); }
        }

        public UnitInfo SelectedUnitTo
        {
            get { return _selectedUnitTo; }
            set { this.RaiseAndSetIfChanged(ref _selectedUnitTo, value); }
        }

        public DelegateCommand ToggleHistoryCommand
        {
            get
            {
                if (_toggleHistoryCommand == null)
                {
                    _toggleHistoryCommand = new DelegateCommand(() => ShowHistory = !ShowHistory);
                }

                return _toggleHistoryCommand;
            }
        }

        public DelegateCommand InvertCommand
        {
            get
            {
                if (_invertCommand == null)
                {
                    _invertCommand = new DelegateCommand(() => 
                    {
                        var oldUnitFrom = SelectedUnitFrom;
                        var oldToValue = ToValue;

                        SelectedUnitFrom = SelectedUnitTo;
                        SelectedUnitTo = oldUnitFrom;

                        FromValue = oldToValue;
                    });
                }

                return _invertCommand;
            }
        }

        public DelegateCommand CopyToClipboardCommand
        {
            get
            {
                if (_copyToClipboardCommand == null)
                {
                    _copyToClipboardCommand = new DelegateCommand(() => Clipboard.SetText(ToValue.ToString()));
                }

                return _copyToClipboardCommand;
            }
        }

        public DelegateCommand ClearHistoryCommand
        {
            get
            {
                if (_clearHistory == null)
                {
                    _clearHistory = new DelegateCommand(() => ConversionHistory.Clear());
                }

                return _clearHistory;
            }
        }

        protected DelegateCommand<string> SearchMeasuresCommand
        {
            get
            {
                if (_searchMeasuresCommand == null)
                {
                    _searchMeasuresCommand = new DelegateCommand<string>(s =>
                    {
                        SelectedMeasure = null;
                        SelectedUnitFrom = null;
                        SelectedUnitTo = null;
                        FromValue = 0;
                        Measures = new ReadOnlyObservableCollection<QuantityInfo>(new ObservableCollection<QuantityInfo>(_measureService.GetMeasures(s)));
                        this.RaisePropertyChanged(nameof(Measures));

                        if (Measures.Count > 0)
                        {
                            SelectedMeasure = Measures[0];
                        }
                    });
                }

                return _searchMeasuresCommand;
            }
        }

        protected DelegateCommand<QuantityInfo> SearchUnitCommand
        {
            get
            {
                if (_searchUnitsCommand == null)
                {
                    _searchUnitsCommand = new DelegateCommand<QuantityInfo>(quantityInfo =>
                    {
                        Units = new ReadOnlyObservableCollection<UnitInfo>(new ObservableCollection<UnitInfo>(_measureService.GetUnits(quantityInfo)));
                        this.RaisePropertyChanged(nameof(Units));

                        if (Units.Count > 0)
                        {
                            SelectedUnitFrom = Units[0];
                            SelectedUnitTo = Units[0];
                        }

                    });
                }

                return _searchUnitsCommand;
            }
        }
    }
}
