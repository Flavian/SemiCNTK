using System;
using System.Globalization;
using System.Windows.Data;

namespace SzabolcsLecke1.Converters
{
    public class CoordinateScaleXConverter :  IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double)
            {
                var dValue = (double)value;
                return dValue * ConverterConstants.CoordinateMultiplyValue;
            }

            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
