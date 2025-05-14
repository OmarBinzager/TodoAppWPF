using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ToDoProject.Constant.Converters
{
    internal class IndexMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            //var collection = value as IEnumerable<object>;
            //var item = parameter;
            //if (collection != null && item != null)
            //{
            //    int index = collection.ToList().IndexOf(item) + 1;
            //    return index.ToString();
            //}
            //return "?";
            if (values.Length < 2 || !(values[0] is IEnumerable<object> collection ) || !( values[1] is object item))
            {
                return "?";
            }
            int index = collection.Cast<object>().ToList().IndexOf(item) + 1;
            return index.ToString();
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException(); // ConvertBack is not needed
        }
    }
}
