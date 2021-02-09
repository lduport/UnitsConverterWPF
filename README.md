# UnitsConverterWPF
A small unit of measure converter WPF application using Prism, mahApps, ReactiveUI...

The application has been designed to support the following features:

- Perform a simple conversion
- Reverse conversion settings
- Copy of the result to the clipboard
- Display an history of the last conversions
- Conversion in several units simultaneously -- NOT IMPLEMENTED
- Display of the formula used (like Google) -- NOT IMPLEMENTED
- Converting a file -- NOT IMPLEMENTED
- Support localization -- NOT IMPLEMENTED

Because this is a sample application, the solution contains only one project.

However, the application has been developed to be maintainable, testable and easily expandable. 

To achieve this goal I choosed to use the MVVM pattern to separate the view from the business logic (viewmodel and services).

The application also relies on an IoC container (Unity) to handle dependencies, thus allowing the creation of mocks for unit testing (among other advantages like interception)

There's also an exemple of InProc messaging using Prism EventAggregator which avoid strong coupling between ViewModels

NB: the wrapper (MeasureService) over Unitsnet library was totally unnecessary but was made as an example of business service.


![Alt text](/assets/SolutionView.PNG?raw=true)

# Screenshots

![Alt text](/assets/unitsconverter.png?raw=true)
![Alt text](/assets/unitsconverter2.png?raw=true)

# Dependencies
https://github.com/MahApps/MahApps.Metro - A framework that allows developers to cobble together a better UI for their own WPF applications with minimal effort.

https://github.com/MahApps/MahApps.Metro.IconPacks - Awesome icon packs for WPF and UWP in one library

https://github.com/PrismLibrary/Prism - Prism is a framework for building loosely coupled, maintainable, and testable XAML applications in WPF, Xamarin Forms, and Uno / Win UI Applications..

https://github.com/reactiveui/ReactiveUI - An advanced, composable, functional reactive model-view-viewmodel framework for all .NET platforms that is inspired by functional reactive programming. ReactiveUI allows you to abstract mutable state away from your user interfaces, express the idea around a feature in one readable place and improve the testability of your application.

https://github.com/unitycontainer/unity - The Unity Container (Unity) is a full featured, extensible dependency injection container. It facilitates building loosely coupled applications.

https://github.com/angularsen/UnitsNet - Makes life working with units of measurement just a little bit better.
