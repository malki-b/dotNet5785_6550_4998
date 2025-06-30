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
        private readonly int _volunteerId;

        public Available(int volunteerId)
        {
            InitializeComponent();
            _volunteerId = volunteerId;
            LoadOpenCallList();
        }

        public IEnumerable<BO.OpenCallInList> OpenCallList
        {
            get { return (IEnumerable<BO.OpenCallInList>)GetValue(OpenCallListProperty); }
            set { SetValue(OpenCallListProperty, value); }
        }

        public static readonly DependencyProperty OpenCallListProperty =
            DependencyProperty.Register("OpenCallList", typeof(IEnumerable<BO.OpenCallInList>), typeof(Available), new PropertyMetadata(null));

        public BO.CallField CallSortProp { get; set; } = BO.CallField.None;
        public BO.TypeOfReading TypeOfCallFilterProp { get; set; } = BO.TypeOfReading.None;

        private void LoadOpenCallList()
        {
            try
            {
                OpenCallList = s_bl.Call.RequestOpenCallsForSelection(_volunteerId, TypeOfCallFilterProp, CallSortProp);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load the OpenCallList: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SelectionChangedInOpenCallListProp(object sender, RoutedEventArgs e)
        {
            LoadOpenCallList();
        }

        private void OpenCallListObserver() => LoadOpenCallList();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            s_bl.Volunteer.AddObserver(OpenCallListObserver);
            LoadOpenCallList();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            s_bl.Volunteer.RemoveObserver(OpenCallListObserver);
        }

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
