using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using WpfMVVM.Model;

namespace WpfMVVM.Converter
{
    class CellToBackgroundConverter : IMultiValueConverter
    {
        static readonly SolidColorBrush WhiteBrush = new SolidColorBrush(Colors.White);
        static readonly SolidColorBrush GrayBrush = new SolidColorBrush(Colors.Gray);
        static readonly SolidColorBrush SelectedBrush = new SolidColorBrush(Colors.PaleGreen);

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is Cell cell)
            {
                if (values[1] is Cell selectedCell && selectedCell.Equals(cell))
                {
                    return SelectedBrush;
                }

                return (cell.Column + cell.Row) % 2 == 1 
                    ? WhiteBrush : GrayBrush;
            }
            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
