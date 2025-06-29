//using System;
//using System.Collections.Generic;
//using System.Globalization;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Data;
//using System.Windows.Media;

//namespace PL;

//internal class ConvertUpdateToTrueKey : IValueConverter
//{
//    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
//    {
//        //BO.Year year = (BO.Year)value;
//        //switch (year)
//        //{
//        //    case BO.Year.FirstYear:
//        //        return Brushes.Yellow;
//        //    case BO.Year.SecondYear:
//        //        return Brushes.Orange;
//        //    case BO.Year.ThirdYear:
//        //        return Brushes.Green;
//        //    case BO.Year.ExtraYear:
//        //        return Brushes.PaleVioletRed;
//        //    case BO.Year.None:
//        //        return Brushes.White;
//        //    default:
//        //        return Brushes.White;
//        //}
//        return null;
//    }

//    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
//    {
//        throw new NotImplementedException();
//    }
//}
//public class NullToBoolConverter : IValueConverter
//{
//    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
//    {
//        bool invert = parameter?.ToString() == "invert";
//        bool isNull = value == null;
//        return invert ? isNull : !isNull;
//    }
//    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
//}

//// האם הערך null? מחזיר Collapsed/Visible
//public class NullToVisibilityConverter : IValueConverter
//{
//    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
//    {
//        return value == null ? Visibility.Collapsed : Visibility.Visible;
//    }
//    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
//}




using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using BO;

namespace PL
{
    public class ConvertUpdateToTrue : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.ToString() == "Update";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (Visibility)value == Visibility.Visible;
        }
    }

    public class ConvertUpdateToVisible : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.ToString() == "Update" ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class RoleEditingPermissionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // המשתמש הנוכחי חייב להיות מנהל כדי שהתיבה תיפתח לעריכה
            return value != null && value.ToString().Equals("Admin", StringComparison.OrdinalIgnoreCase);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    // האם הערך null? מחזיר false/true (אפשר גם הפוך עם parameter)
    public class NullToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool invert = parameter?.ToString() == "invert";
            bool isNull = value == null;
            return invert ? isNull : !isNull;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }

    // האם הערך null? מחזיר Collapsed/Visible
    public class NullToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? Visibility.Collapsed : Visibility.Visible;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }



}
