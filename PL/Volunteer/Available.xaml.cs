using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
            // CallHistoryList = s_bl.Call.RequestCallsByVolunteer(volunteerId);
            CallAvailableList = s_bl.Call.RequestOpenCallsForSelection(volunteerId);

            //queryCallAvailableList();
        }

        public int VolunteerId { get; }
        public IEnumerable<BO.OpenCallInList> CallAvailableList
        {
            get { return (IEnumerable<BO.OpenCallInList>)GetValue(CallAvailableListProperty); }
            set { SetValue(CallAvailableListProperty, value); }
        }
        public static readonly DependencyProperty CallAvailableListProperty =
        DependencyProperty.Register("CallAvailableList", typeof(IEnumerable<BO.OpenCallInList>), typeof(Available), new PropertyMetadata(null));

        public BO.OpenCallField CallSortProp { get; set; } = BO.OpenCallField.None; // Changed from CallField to OpenCallField
        public BO.TypeOfReading TypeOfCallFilterProp { get; set; } = BO.TypeOfReading.None;

        private void SelectionChangedInCallAvailableListProp(object sender, RoutedEventArgs e)
        {
            try { queryCallAvailableList(); }
            catch (Exception ex) { MessageBox.Show($"Failed to load the CallAvailableList: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
        }
        private void queryCallAvailableList()
        {
            if (TypeOfCallFilterProp == BO.TypeOfReading.None)
            {
                CallAvailableList = s_bl?.Call.RequestOpenCallsForSelection(VolunteerId, null,
                    CallSortProp == BO.OpenCallField.None ? null : CallSortProp // Changed from CallField to OpenCallField
                )!;
            }
            else
            {
                CallAvailableList = s_bl?.Call.RequestOpenCallsForSelection(VolunteerId, TypeOfCallFilterProp,
                       CallSortProp == BO.OpenCallField.None ? null : CallSortProp // Changed from CallField to OpenCallField
                   )!;
            }
        }

        private void CallAvailableListObserver() => queryCallAvailableList();
        private void Window_Loaded(object sender, RoutedEventArgs e) => s_bl.Volunteer.AddObserver(CallAvailableListObserver);
        private void Window_Closed(object sender, EventArgs e) => s_bl.Volunteer.RemoveObserver(CallAvailableListObserver);

        private void SelectionChangedInCallAvailableListProp(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void SelectionChangedInOpenCallListProp(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
