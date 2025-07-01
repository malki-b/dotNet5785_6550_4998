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
//    /// <summary>
//    /// Interaction logic for CallAvailableWindow.xaml
//    /// </summary>
//    public partial class Available : Window
//    {
//        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
//        public Available(int volunteerId)
//        {
//            InitializeComponent();
//            VolunteerId = volunteerId;
//            // CallHistoryList = s_bl.Call.RequestCallsByVolunteer(volunteerId);
//            CallAvailableList = s_bl.Call.RequestOpenCallsForSelection(volunteerId);

//            //queryCallAvailableList();
//        }

//        public int VolunteerId { get; }
//        public IEnumerable<BO.OpenCallInList> CallAvailableList
//        {
//            get { return (IEnumerable<BO.OpenCallInList>)GetValue(CallAvailableListProperty); }
//            set { SetValue(CallAvailableListProperty, value); }
//        }
//        public static readonly DependencyProperty CallAvailableListProperty =
//        DependencyProperty.Register("CallAvailableList", typeof(IEnumerable<BO.OpenCallInList>), typeof(Available), new PropertyMetadata(null));

//        public BO.OpenCallField CallSortProp { get; set; } = BO.OpenCallField.None; // Changed from CallField to OpenCallField
//        public BO.TypeOfReading TypeOfCallFilterProp { get; set; } = BO.TypeOfReading.None;

//        private void SelectionChangedInCallAvailableListProp(object sender, RoutedEventArgs e)
//        {
//            try { queryCallAvailableList(); }
//            catch (Exception ex) { MessageBox.Show($"Failed to load the CallAvailableList: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
//        }
//        private void queryCallAvailableList()
//        {
//            if (TypeOfCallFilterProp == BO.TypeOfReading.None)
//            {
//                CallAvailableList = s_bl?.Call.RequestOpenCallsForSelection(VolunteerId, null,
//                    CallSortProp == BO.OpenCallField.None ? null : CallSortProp // Changed from CallField to OpenCallField
//                )!;
//            }
//            else
//            {
//                CallAvailableList = s_bl?.Call.RequestOpenCallsForSelection(VolunteerId, TypeOfCallFilterProp,
//                       CallSortProp == BO.OpenCallField.None ? null : CallSortProp // Changed from CallField to OpenCallField
//                   )!;
//            }
//        }

//        private void CallAvailableListObserver() => queryCallAvailableList();
//        private void Window_Loaded(object sender, RoutedEventArgs e) => s_bl.Volunteer.AddObserver(CallAvailableListObserver);
//        private void Window_Closed(object sender, EventArgs e) => s_bl.Volunteer.RemoveObserver(CallAvailableListObserver);

//        private void SelectionChangedInCallAvailableListProp(object sender, SelectionChangedEventArgs e)
//        {

//        }

//        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
//        {

//        }

//        private void SelectionChangedInOpenCallListProp(object sender, SelectionChangedEventArgs e)
//        {

//        }
//    }
//}



using BO;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace PL.Volunteer
{
    /// <summary>
    /// Interaction logic for CallAvailableWindow.xaml
    /// </summary>
    public partial class Available : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public Available(int volunteerId)
        {
            InitializeComponent();
            VolunteerId = volunteerId;
            CallAvailableList = s_bl.Call.RequestOpenCallsForSelection(volunteerId);
            Loaded += Window_Loaded;
            Closed += Window_Closed;
        }

        public int VolunteerId { get; }
        private OpenCallInList? SelectedCall;
        public IEnumerable<BO.OpenCallInList> CallAvailableList
        {
            get => (IEnumerable<BO.OpenCallInList>)GetValue(CallAvailableListProperty);
            set => SetValue(CallAvailableListProperty, value);
        }

        public static readonly DependencyProperty CallAvailableListProperty =
            DependencyProperty.Register(nameof(CallAvailableList), typeof(IEnumerable<BO.OpenCallInList>), typeof(Available), new PropertyMetadata(null));

        public BO.OpenCallField CallSortProp
        {
            get => (BO.OpenCallField)GetValue(CallSortPropProperty);
            set => SetValue(CallSortPropProperty, value);
        }

        public static readonly DependencyProperty CallSortPropProperty =
            DependencyProperty.Register(nameof(CallSortProp), typeof(BO.OpenCallField), typeof(Available), new PropertyMetadata(BO.OpenCallField.None, OnFilterOrSortChanged));

        public BO.TypeOfReading TypeOfCallFilterProp
        {
            get => (BO.TypeOfReading)GetValue(TypeOfCallFilterPropProperty);
            set => SetValue(TypeOfCallFilterPropProperty, value);
        }

        public static readonly DependencyProperty TypeOfCallFilterPropProperty =
            DependencyProperty.Register(nameof(TypeOfCallFilterProp), typeof(BO.TypeOfReading), typeof(Available), new PropertyMetadata(BO.TypeOfReading.None, OnFilterOrSortChanged));

        private static void OnFilterOrSortChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Available window)
                window.queryCallAvailableList();
        }

        private void queryCallAvailableList()
        {
            try
            {
                CallAvailableList = s_bl.Call.RequestOpenCallsForSelection(
                    VolunteerId,
                    TypeOfCallFilterProp == BO.TypeOfReading.None ? null : TypeOfCallFilterProp,
                    CallSortProp == BO.OpenCallField.None ? null : CallSortProp
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load the CallAvailableList: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CallAvailableListObserver() => queryCallAvailableList();
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            s_bl.Volunteer.AddObserver(CallAvailableListObserver); // ✅ חדש
            s_bl.Call.AddObserver(CallAvailableListObserver); // ✅ חדש
            queryCallAvailableList(); // ✅ חדש
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            s_bl.Volunteer.RemoveObserver(CallAvailableListObserver); // ✅ חדש
            s_bl.Call.RemoveObserver(CallAvailableListObserver); // ✅ חדש
        }

        //private void SelectCall_Click(object sender, RoutedEventArgs e)
        //{
        //    if (SelectedCall == null)
        //        return;

        //    try
        //    {
        //        s_bl.Call.SelectCallForTreatment(VolunteerId, SelectedCall.Id);
        //        OpenCalls.Remove(SelectedCall);
        //        SelectedCall = null;
        //        MessageBox.Show("The call was successfully selected for treatment.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        //        Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Error selecting call: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}
        private void SelectCall_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is OpenCallInList call)
            {
                try
                {
                    s_bl.Call.SelectCallForTreatment(VolunteerId, call.Id);

                    // Remove the selected call from the list and update the property
                    //if (CallAvailableList is IEnumerable<BO.OpenCallInList> currentList)
                    //{
                    //    // If your list is OpenCallInList, match by Id
                    //    CallAvailableList = currentList.Where(c => c.Id != call.Id).ToList();

                    //}

                    MessageBox.Show("The call was successfully selected for treatment.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"המחיקה נכשלה: {ex.Message}", "שגיאת מחיקה", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // אפשר להוסיף כאן קוד אם רוצים לטפל בבחירה ברשימה
        }

        private void SelectionChangedInOpenCallListProp(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
