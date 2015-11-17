using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend;
using Xamarin.Forms;
using CloudClubv1._2_;
using System.Globalization;

namespace FrontEnd
{
    public class ColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string colorString = (string)value;
            switch (colorString)
            {
                case "red":
                    return (Color.FromRgb(255, 0, 0));
                case "green":
                    return (Color.FromRgb(0, 234, 106));
                case "blue":
                    return (Color.FromRgb(0, 176, 240));
                case "navy":
                    return (Color.FromRgb(31, 78, 121));
                case "magenta":
                    return (Color.FromRgb(251, 33, 241));
                case "gray":
                    return (Color.FromRgb(127, 127, 127));
                case "purple":
                    return (Color.FromRgb(210, 61, 235));
                default:
                    return (Color.FromRgb(210, 61, 235));

            }



        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
