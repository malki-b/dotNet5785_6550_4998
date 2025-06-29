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
//    /// Interaction logic for AvailableCallsWindow.xaml
//    /// </summary>
//    public partial class AvailableCallsWindow : Window
//    {
//        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
//        public AvailableCallsWindow(int volunteerId)
//        {
//            InitializeComponent();
//            VolunteerId = volunteerId;
//            OpenCallList = s_bl.Call.GetOpenCallsForSelection(volunteerId);
//        }

//        public int VolunteerId { get;}
//        public IEnumerable<BO.OpenCallInList> OpenCallList
//        {
//            get { return (IEnumerable<BO.OpenCallInList>)GetValue(OpenCallListProperty); }
//            set { SetValue(OpenCallListProperty, value); }
//        }
//        public static readonly DependencyProperty OpenCallListProperty =
//        DependencyProperty.Register("OpenCallList", typeof(IEnumerable<BO.OpenCallInList>), typeof(AvailableCallsWindow), new PropertyMetadata(null));

//        public BO.OpenCallField CallSortProp { get; set; } = BO.OpenCallField.None;
//        public BO.TypeOfCall TypeOfCallFilterProp { get; set; } = BO.TypeOfCall.None;

//        //public string? FilterValue
//        //{
//        //    get => (string?)GetValue(FilterValueProperty);
//        //    set => SetValue(FilterValueProperty, value);
//        //}

//        //public static readonly DependencyProperty FilterValueProperty =
//        //    DependencyProperty.Register(nameof(FilterValue),typeof(string),typeof(AvailableCallsWindow),new PropertyMetadata(null));

//        private void SelectionChangedInOpenCallListProp(object sender, RoutedEventArgs e)
//        {
//            try { queryOpenCallList(); }
//            catch (Exception ex) { MessageBox.Show($"Failed to load the OpenCallList: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
//        }
//        private void queryOpenCallList()
//        {
//            if (TypeOfCallFilterProp == BO.TypeOfCall.None)
//            {
//                OpenCallList = s_bl?.Call.GetOpenCallsForSelection(VolunteerId, null,
//                    CallSortProp == BO.OpenCallField.None ? null : CallSortProp
//                )!;
//            }
//            else
//            {
//                OpenCallList = s_bl?.Call.GetOpenCallsForSelection(VolunteerId, TypeOfCallFilterProp,
//                       CallSortProp == BO.OpenCallField.None ? null : CallSortProp
//                   )!;
//            }
//        }
//        private void OpenCallListObserver() => queryOpenCallList();
//        private void Window_Loaded(object sender, RoutedEventArgs e) => s_bl.Volunteer.AddObserver(OpenCallListObserver);
//        private void Window_Closed(object sender, EventArgs e) => s_bl.Volunteer.RemoveObserver(OpenCallListObserver);
//        public BO.OpenCallInList? SelectedCall { get; set; }
//        private void lsvVolunteerList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
//        {
//            try
//            {
//                if (SelectedCall != null)
//                {
//                    s_bl.Call.ChooseCallForHandling(VolunteerId, SelectedCall.Id);
//                    MessageBox.Show($"A call {SelectedCall.Id} will be selected");
//                }
//            }
//            catch (Exception ex) { MessageBox.Show($"Failed to load the VolunteerWindow: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
//        }

//        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
//        {

//        }
//    }
//}


using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PL.Volunteer
{
    public partial class Available : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();



        public Available(int volunteerId)
        {
            MessageBox.Show($"{volunteerId}");
            _volunteerId = volunteerId;
            InitializeComponent();
            MessageBox.Show($"{_volunteerId}");


            OpenCallList = s_bl.Call.RequestOpenCallsForSelection(volunteerId);
        }
        public int _volunteerId { get; set; }

        public IEnumerable<BO.OpenCallInList> OpenCallList
        {
            get { return (IEnumerable<BO.OpenCallInList>)GetValue(OpenCallListProperty); }
            set { SetValue(OpenCallListProperty, value); }
        }

        public static readonly DependencyProperty OpenCallListProperty =
            DependencyProperty.Register("OpenCallList", typeof(IEnumerable<BO.OpenCallInList>), typeof(Available), new PropertyMetadata(null));

        public BO.CallField CallSortProp { get; set; } = BO.CallField.None;
        public BO.TypeOfReading TypeOfCallFilterProp { get; set; } = BO.TypeOfReading.None;

        private void SelectionChangedInOpenCallListProp(object sender, RoutedEventArgs e)
        {
            try { queryOpenCallList(); }
            catch (Exception ex) { MessageBox.Show($"Failed to load the OpenCallList: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        private void queryOpenCallList()
        {
            if (TypeOfCallFilterProp == BO.TypeOfReading.None)
            {
                OpenCallList = s_bl.Call.RequestOpenCallsForSelection(_volunteerId, null,
                    CallSortProp == BO.CallField.None ? null : CallSortProp);
            }
            else
            {
                OpenCallList = s_bl.Call.RequestOpenCallsForSelection(_volunteerId, TypeOfCallFilterProp,
                    CallSortProp == BO.CallField.None ? null : CallSortProp);
            }
        }

        private void OpenCallListObserver() => queryOpenCallList();

        private void Window_Loaded(object sender, RoutedEventArgs e) => s_bl.Volunteer.AddObserver(OpenCallListObserver);
        private void Window_Closed(object sender, EventArgs e) => s_bl.Volunteer.RemoveObserver(OpenCallListObserver);

        public BO.OpenCallInList? SelectedCall { get; set; }

        private void lsvVolunteerList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (SelectedCall != null)
                {
                    s_bl.Call.SelectCallForTreatment(_volunteerId, SelectedCall.Id);
                    MessageBox.Show($"A call {SelectedCall.Id} will be selected");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load the VolunteerWindow: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // ריק (אפשר להסיר אם לא צריך)
        }
    }
}
