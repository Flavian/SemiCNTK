using System;
using System.Globalization;
using System.Windows.Data;

namespace SzabolcsLecke1.Converters
{
    public class LineYCoordConverter : IMultiValueConverter
    {
       
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values?.Length == 3 && values[0] is double && values[1] is double && values[2] is double)
            {
                var x = (double)values[0];
                var theta0 = (double)values[1];
                var theta1 = (double)values[2];

                var y = (theta1 * x + theta0) * ConverterConstants.CoordinateMultiplyValue;
                return -y;
            }


            return Binding.DoNothing;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
