using System;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;

namespace BloodBowlPOC.Views.Converters
{
    public class ProbabilityToBrushConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            // Values[0]: probability
            // Values[1]: Min
            // Values[2]: Max
            if (values == null)
                return null;
            if (values.Length != 3)
                return null;
            if (values.Any(x => !(x is double)))
                return null;
            double probability = (double)values[0];
            double min = (double)values[1];
            double max = (double)values[2];
            double red = 255 - (probability - min) * (255 - 0) / (max - min);
            double green = 0 + (probability - min) * (255 - 0) / (max - min);
            return new SolidColorBrush(Color.FromRgb((byte) red, (byte) green, 0));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
