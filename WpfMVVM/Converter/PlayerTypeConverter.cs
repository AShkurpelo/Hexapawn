using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using WpfMVVM.Model;

namespace WpfMVVM.Converter
{
    class PlayerTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is PlayerType plType)
            {
                switch (plType)
                {
                    case PlayerType.None:
                        return String.Empty;
                    case PlayerType.One:
                        return Properties.Resources.Player1;
                    case PlayerType.Two:
                        return Properties.Resources.Player2;
                }
            }
            return String.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
