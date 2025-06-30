using BO;
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
    /// Interaction logic for CallHistoryWindow.xaml
    /// </summary>
    public partial class HistoryCall : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public HistoryCall(int volunteerId)
        {
            InitializeComponent();
            VolunteerId = volunteerId;
            CallHistoryList = s_bl.Call.RequestClosedCallsByVolunteer(volunteerId);
        }

        public int VolunteerId { get; }
        public IEnumerable<BO.ClosedCallInList> CallHistoryList
        {
            get { return (IEnumerable<BO.ClosedCallInList>)GetValue(CallHistoryListListProperty); }
            set { SetValue(CallHistoryListListProperty, value); }
        }
        public static readonly DependencyProperty CallHistoryListListProperty =
        DependencyProperty.Register("CallHistoryList", typeof(IEnumerable<BO.ClosedCallInList>), typeof(HistoryCall), new PropertyMetadata(null));

        public BO.CallField CallSortProp { get; set; } = BO.CallField.None;
        public BO.TypeOfReading TypeOfCallFilterProp { get; set; } = BO.TypeOfReading.None;

        //public string? FilterValue
        //{
        //    get => (string?)GetValue(FilterValueProperty);
        //    set => SetValue(FilterValueProperty, value);
        //}

        //public static readonly DependencyProperty FilterValueProperty =
        //    DependencyProperty.Register(nameof(FilterValue),typeof(string),typeof(CallHistoryWindow),new PropertyMetadata(null));

        private void SelectionChangedInCallHistoryListProp(object sender, RoutedEventArgs e)
        {
            try { queryCallHistoryList(); }
            catch (Exception ex) { MessageBox.Show($"Failed to load the CallHistoryList: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
        }
        private void queryCallHistoryList()
        {
            if (TypeOfCallFilterProp == BO.TypeOfReading.None)
            {
                CallHistoryList = s_bl?.Call.RequestClosedCallsByVolunteer(VolunteerId, null,
                    CallSortProp == BO.CallField.None ? null : CallSortProp
                )!;
            }
            else
            {
                CallHistoryList = s_bl?.Call.RequestClosedCallsByVolunteer(VolunteerId, TypeOfCallFilterProp,
                       CallSortProp == BO.CallField.None ? null : CallSortProp
                   )!;
            }
        }
        private void CallHistoryListObserver() => queryCallHistoryList();


        private void Window_Loaded(object sender, RoutedEventArgs e) => s_bl.Volunteer.AddObserver(CallHistoryListObserver);
        private void Window_Closed(object sender, EventArgs e) => s_bl.Volunteer.RemoveObserver(CallHistoryListObserver);

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        //private void SelectionChangedInCallHistoryListProp(object sender, SelectionChangedEventArgs e)
        //{
            
        //        try
        //        {
        //            VolunteerList = string.IsNullOrWhiteSpace(FilterText)
        //                ? s_bl?.Volunteer.ReadAll()!
        //                : s_bl?.Volunteer.GetFilteredAndSortedVolunteers(filterBy: VolunteerFilter, filterValue: FilterText)!;
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show($"אירעה שגיאה בעת עדכון רשימת המתנדבים: {ex.Message}", "שגיאת עדכון", MessageBoxButton.OK, MessageBoxImage.Error);
        //        }
            
        //}

        //private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{

        //}

        //private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{

        //}

        //private void OpenVolunteerWindow(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        new VolunteerWindow().Show();
        //    }
        //    catch (Exception ex) { MessageBox.Show($"Failed to load the VolunteerList: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
        //}
        //public BO.VolunteerInList? SelectedVolunteer { get; set; }
        //private void lsvVolunteerList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        //{
        //    try
        //    {
        //        if (SelectedVolunteer != null)
        //        {
        //            int id = SelectedVolunteer.VolunteerId ?? 0;
        //            new VolunteerWindow(id).Show();
        //        }
        //    }
        //    catch (Exception ex) { MessageBox.Show($"Failed to load the VolunteerWindow: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
        //}
        //private void DeleteVolunteer_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        var button = sender as Button;
        //        if (button == null) return;

        //        int volunteerId = (int)button.Tag;

        //        var result = MessageBox.Show(
        //            $"Are you sure you want to delete volunteer #{volunteerId}?",
        //            "Confirm Deletion",
        //            MessageBoxButton.YesNo,
        //            MessageBoxImage.Warning);

        //        if (result == MessageBoxResult.Yes)
        //        {
        //            s_bl.Volunteer.DeleteVolunteer(volunteerId);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Failed to delete the volunteer: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}

    }
}

