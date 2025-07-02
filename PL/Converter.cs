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

    //// האם הערך null? מחזיר false/true (אפשר גם הפוך עם parameter)
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
    public class NullToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // מחזיר false אם value אינו null, אחרת מחזיר true
            return value == null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
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


    //public class VolunteerStatusConverter : IMultiValueConverter
    //    {
    //        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    //        {
    //            if (values.Length < 2)
    //                return false;

    //            bool isActive = (bool)values[0];
    //            var currentCallInProgress = values[1];

    //            return isActive && currentCallInProgress == null;
    //        }

    //        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    //        {
    //            throw new NotImplementedException();
    //        }
    //    }
    public class VolunteerStatusConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2)
                return false;

            // בדוק אם הערך הראשון הוא bool
            if (values[0] is bool isActive)
            {
                var currentCallInProgress = values[1];
                return isActive && currentCallInProgress == null;
            }

            return false; // אם הערך הראשון אינו bool, החזר false
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BoolToStartStopConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "Stop Simulator" : "Start Simulator";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class BooleanInverseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return !boolValue;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert(value, targetType, parameter, culture);
        }
    }

}
