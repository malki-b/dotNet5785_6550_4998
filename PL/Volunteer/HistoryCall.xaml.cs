using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace PL.Volunteer
{
    public partial class HistoryCall : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public HistoryCall(int volunteerId)
        {
            InitializeComponent();
            VolunteerId = volunteerId;
            LoadCallHistory();
        }

        public int VolunteerId { get; }
        public IEnumerable<BO.ClosedCallInList> CallHistoryList
        {
            get { return (IEnumerable<BO.ClosedCallInList>)GetValue(CallHistoryListProperty); }
            set { SetValue(CallHistoryListProperty, value); }
        }
        public static readonly DependencyProperty CallHistoryListProperty =
            DependencyProperty.Register("CallHistoryList", typeof(IEnumerable<BO.ClosedCallInList>), typeof(HistoryCall), new PropertyMetadata(null));

        public BO.CallField CallSortProp { get; set; } = BO.CallField.None;
        public BO.TypeOfReading TypeOfCallFilterProp { get; set; } = BO.TypeOfReading.None;

        private void LoadCallHistory()
        {
            try
            {
                CallHistoryList = s_bl.Call.RequestClosedCallsByVolunteer(VolunteerId, TypeOfCallFilterProp, CallSortProp);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load the CallHistoryList: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SelectionChangedInCallHistoryListProp(object sender, RoutedEventArgs e)
        {
            LoadCallHistory();
        }

        private void CallHistoryListObserver() => LoadCallHistory();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            s_bl.Volunteer.AddObserver(CallHistoryListObserver);
            LoadCallHistory();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            s_bl.Volunteer.RemoveObserver(CallHistoryListObserver);
        }
    }
}
