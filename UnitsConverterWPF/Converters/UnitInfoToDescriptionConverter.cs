using System;
using System.Globalization;
using System.Windows.Data;
using UnitsNet;

namespace UnitsConverterWPF.Converters
{
    public class UnitInfoToDescriptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is UnitInfo unitInfo))
            {
                throw new ArgumentException("Expected value of type UnitsNet.UnitInfo.", nameof(value));
            }

            var abbreviation = UnitAbbreviationsCache.Default.GetDefaultAbbreviation(unitInfo.Value.GetType(), System.Convert.ToInt32(unitInfo.Value));

            return $"{unitInfo.Value} [{abbreviation}]";

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
