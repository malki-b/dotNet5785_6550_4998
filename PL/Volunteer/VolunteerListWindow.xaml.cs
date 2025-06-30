using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.ComponentModel;

namespace PL.Volunteer
{
    public partial class VolunteerListWindow : Window, INotifyPropertyChanged
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public VolunteerInListFields VolunteerFilter { get; set; } = VolunteerInListFields.None;
        public VolunteerInList? SelectedVolunteer { get; set; }
        public VolunteerListWindow()
        {
            InitializeComponent();
            Loaded += VolunteerWindow_Loaded;
            Closed += VolunteerWindow_Closed;
            queryVolunteerList();
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public IEnumerable<VolunteerInList> VolunteerList
        {
            get { return (IEnumerable<VolunteerInList>)GetValue(VolunteerListProperty); }
            set { SetValue(VolunteerListProperty, value); }
        }

        public static readonly DependencyProperty VolunteerListProperty =
            DependencyProperty.Register(
                nameof(VolunteerList),
                typeof(IEnumerable<VolunteerInList>),
                typeof(VolunteerListWindow),
                new PropertyMetadata(null));

        public string FilterText
        {
            get { return (string)GetValue(FilterTextProperty); }
            set { SetValue(FilterTextProperty, value); }
        }

        public static readonly DependencyProperty FilterTextProperty =
            DependencyProperty.Register(
                nameof(FilterText),
                typeof(string),
                typeof(VolunteerListWindow),
                new PropertyMetadata(string.Empty, OnFilterTextChanged));

        private static void OnFilterTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var window = d as VolunteerListWindow;
            try
            {
                window?.ApplyFilter();
            }
            catch (Exception ex)
            {
                MessageBox.Show("אירעה שגיאה בעת עדכון הפילטר. אנא נסה שוב מאוחר יותר.", "שגיאת פילטר", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private List<VolunteerInList> AllVolunteers = new();

        private void ApplyFilter()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(FilterText))
                {
                    VolunteerList = AllVolunteers.ToList();
                }
                else
                {
                    VolunteerList = string.IsNullOrWhiteSpace(FilterText)
                  ? s_bl?.Volunteer.ReadAll()!
                  : s_bl?.Volunteer.GetFilteredAndSortedVolunteers(filterBy: VolunteerFilter, filterValue: FilterText)!;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("סינון" + ex.Message);
            }
        }

        private void queryVolunteerList()
        {
            try
            {
                var volunteers = (VolunteerFilter == VolunteerInListFields.None) ?
                    s_bl?.Volunteer.ReadAll()! :
                    s_bl?.Volunteer.ReadAll(null, null)!;

                AllVolunteers = volunteers.ToList(); 
                ApplyFilter(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show("טעינת הרשימה נכשלה. אנא נסה שוב מאוחר יותר.", "שגיאת טעינה", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void volunteerListObserver() => queryVolunteerList();

        private void VolunteerWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                s_bl.Volunteer.AddObserver(volunteerListObserver);
                queryVolunteerList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("אירעה שגיאה במהלך טעינת החלון. אנא נסה שוב מאוחר יותר.", "שגיאת טעינה", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void VolunteerWindow_Closed(object? sender, EventArgs e)
        {
            try
            {
                s_bl.Volunteer.RemoveObserver(volunteerListObserver);
            }
            catch (Exception ex)
            {
                MessageBox.Show("אירעה שגיאה במהלך סגירת החלון. אנא נסה שוב.", "שגיאת סגירה", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Volunteer_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (SelectedVolunteer != null)
                {
                    new VolunteerWindow(SelectedVolunteer.VolunteerId).Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("אירעה שגיאה בעת פתיחת חלון המתנדב. אנא נסה שוב.", "שגיאת פתיחה", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                VolunteerList = string.IsNullOrWhiteSpace(FilterText)
                    ? s_bl?.Volunteer.ReadAll()!
                    : s_bl?.Volunteer.GetFilteredAndSortedVolunteers(filterBy: VolunteerFilter, filterValue: FilterText)!;
            }
            catch (Exception ex)
            {
                MessageBox.Show("אירעה שגיאה בעת עדכון רשימת המתנדבים. אנא נסה שוב.", "שגיאת עדכון", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                new VolunteerWindow().Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("אירעה שגיאה בעת הוספת מתנדב. אנא נסה שוב.", "שגיאת הוספה", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void DeleteVolunteer_click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is VolunteerInList volunteer)
            {
                var result = MessageBox.Show("האם אתה בטוח שברצונך למחוק את המתנדב?", "אישור מחיקה", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        s_bl.Volunteer.Delete(volunteer.VolunteerId);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("המחיקה נכשלה: " + ex.Message);
                    }
                }
            }
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}


