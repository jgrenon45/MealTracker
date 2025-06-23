using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealTracker.Converters
{
    public class BoolToImageConverter : IValueConverter
    {
        public ImageSource TrueImage { get; set; }
        public ImageSource FalseImage { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isTrue = value is bool b && b;
            return isTrue ? TrueImage : FalseImage;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Typically not needed
            throw new NotImplementedException();
        }
    }
}
