//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Shapes;

//namespace PL.Volunteer
//{
//    public partial class HistoryCall : Window
//    {
//        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

//        public HistoryCall(int volunteerId)
//        {
//            InitializeComponent();
//            VolunteerId = volunteerId;

//            CallHistoryList = s_bl.Call.RequestClosedCallsByVolunteer(volunteerId);
//            Loaded += Window_Loaded;
//            Closed += Window_Closed;
//        }

//        public int VolunteerId { get; }

//        public IEnumerable<BO.ClosedCallInList> CallHistoryList
//        {
//            get => (IEnumerable<BO.ClosedCallInList>)GetValue(CallHistoryListListProperty);
//            set => SetValue(CallHistoryListListProperty, value);
//        }

//        public static readonly DependencyProperty CallHistoryListListProperty =
//            DependencyProperty.Register(nameof(CallHistoryList), typeof(IEnumerable<BO.ClosedCallInList>), typeof(HistoryCall), new PropertyMetadata(null));

//        public BO.ClosedCallField CallSortProp
//        {
//            get => (BO.ClosedCallField)GetValue(CallSortPropProperty);
//            set => SetValue(CallSortPropProperty, value);
//        }

//        public static readonly DependencyProperty CallSortPropProperty =
//            DependencyProperty.Register(nameof(CallSortProp), typeof(BO.ClosedCallField), typeof(HistoryCall), new PropertyMetadata(BO.ClosedCallField.None, OnFilterOrSortChanged));

//        public BO.TypeOfReading TypeOfCallFilterProp
//        {
//            get => (BO.TypeOfReading)GetValue(TypeOfCallFilterPropProperty);
//            set => SetValue(TypeOfCallFilterPropProperty, value);
//        }

//        public static readonly DependencyProperty TypeOfCallFilterPropProperty =
//            DependencyProperty.Register(nameof(TypeOfCallFilterProp), typeof(BO.TypeOfReading), typeof(HistoryCall), new PropertyMetadata(BO.TypeOfReading.None, OnFilterOrSortChanged));

//        // כל שינוי במיון או בסינון יפעיל את הפונקציה הזו
//        private static void OnFilterOrSortChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
//        {
//            if (d is HistoryCall window)
//                window.QueryCallHistoryList();
//        }

//        private void QueryCallHistoryList()
//        {
//            try
//            {
//                CallHistoryList = s_bl.Call.RequestClosedCallsByVolunteer(
//                    VolunteerId,
//                    TypeOfCallFilterProp == BO.TypeOfReading.None ? null : TypeOfCallFilterProp,
//                    CallSortProp == BO.ClosedCallField.None ? null : CallSortProp
//                );
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Failed to load the CallHistoryList: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
//            }
//        }

//        private void Window_Loaded(object sender, RoutedEventArgs e)
//        {
//            s_bl.Volunteer.AddObserver(CallHistoryListObserver); // ✅ חדש
//            s_bl.Call.AddObserver(CallHistoryListObserver); // ✅ חדש

//            QueryCallHistoryList(); // ✅ חדש
//        }

//        private void Window_Closed(object sender, EventArgs e)
//        {
//            s_bl.Call.RemoveObserver(CallHistoryListObserver); // ✅ חדש
//            s_bl.Volunteer.RemoveObserver(CallHistoryListObserver); // ✅ חדש
//        }

//        private void CallHistoryListObserver() => QueryCallHistoryList();

//        // לא צריך יותר את SelectionChangedInCallHistoryListProp – הכל קורה דרך ה־Binding
//        // אם בכל זאת רוצים להפעיל ידנית:
//        // private void SelectionChangedInCallHistoryListProp(object sender, SelectionChangedEventArgs e) => QueryCallHistoryList();

//        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
//        {
//            // אפשר להוסיף כאן קוד אם רוצים לטפל בבחירה ברשימה
//        }

//        private void SelectionChangedInCallHistoryListProp(object sender, SelectionChangedEventArgs e)
//        {

//        }
//    }
//}


using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace PL.Volunteer
{
    public partial class HistoryCall : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public HistoryCall(int volunteerId)
        {
            InitializeComponent();
            VolunteerId = volunteerId;

            CallHistoryList = s_bl.Call.RequestClosedCallsByVolunteer(volunteerId);
            Loaded += Window_Loaded;
            Closed += Window_Closed;
        }

        public int VolunteerId { get; }

        private volatile bool _observerWorking = false; // ✅ דגל לפעולה תקינה עם Dispatcher

        public IEnumerable<BO.ClosedCallInList> CallHistoryList
        {
            get => (IEnumerable<BO.ClosedCallInList>)GetValue(CallHistoryListListProperty);
            set => SetValue(CallHistoryListListProperty, value);
        }

        public static readonly DependencyProperty CallHistoryListListProperty =
            DependencyProperty.Register(nameof(CallHistoryList), typeof(IEnumerable<BO.ClosedCallInList>), typeof(HistoryCall), new PropertyMetadata(null));

        public BO.ClosedCallField CallSortProp
        {
            get => (BO.ClosedCallField)GetValue(CallSortPropProperty);
            set => SetValue(CallSortPropProperty, value);
        }

        public static readonly DependencyProperty CallSortPropProperty =
            DependencyProperty.Register(nameof(CallSortProp), typeof(BO.ClosedCallField), typeof(HistoryCall), new PropertyMetadata(BO.ClosedCallField.None, OnFilterOrSortChanged));

        public BO.TypeOfReading TypeOfCallFilterProp
        {
            get => (BO.TypeOfReading)GetValue(TypeOfCallFilterPropProperty);
            set => SetValue(TypeOfCallFilterPropProperty, value);
        }

        public static readonly DependencyProperty TypeOfCallFilterPropProperty =
            DependencyProperty.Register(nameof(TypeOfCallFilterProp), typeof(BO.TypeOfReading), typeof(HistoryCall), new PropertyMetadata(BO.TypeOfReading.None, OnFilterOrSortChanged));

        private static void OnFilterOrSortChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is HistoryCall window)
                window.QueryCallHistoryList();
        }

        private void QueryCallHistoryList()
        {
            try
            {
                CallHistoryList = s_bl.Call.RequestClosedCallsByVolunteer(
                    VolunteerId,
                    TypeOfCallFilterProp == BO.TypeOfReading.None ? null : TypeOfCallFilterProp,
                    CallSortProp == BO.ClosedCallField.None ? null : CallSortProp
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load the CallHistoryList: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CallHistoryListObserver()
        {
            if (_observerWorking) return;

            _observerWorking = true;
            _ = Dispatcher.BeginInvoke(() =>
            {
                QueryCallHistoryList();
                _observerWorking = false;
            });
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            s_bl.Volunteer.AddObserver(CallHistoryListObserver);
            s_bl.Call.AddObserver(CallHistoryListObserver);

            QueryCallHistoryList();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            s_bl.Call.RemoveObserver(CallHistoryListObserver);
            s_bl.Volunteer.RemoveObserver(CallHistoryListObserver);
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // ניתן לטפל בבחירה כאן אם רוצים
        }

        private void SelectionChangedInCallHistoryListProp(object sender, SelectionChangedEventArgs e)
        {
        }
    }
}
