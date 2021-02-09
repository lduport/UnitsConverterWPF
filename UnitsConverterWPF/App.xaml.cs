using Prism.Ioc;
using Prism.Mvvm;
using System.Windows;
using UnitsConverterWPF.Views;
using UnitsConverterWPF.ViewModels;
using UnitsConverterWPF.Services;

namespace UnitsConverterWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();

            //ViewModelLocationProvider.Register(typeof(MainWindow).ToString(), typeof(MainWindowViewModel));
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IMeasureService, MeasureService>();
        }
    }
}
