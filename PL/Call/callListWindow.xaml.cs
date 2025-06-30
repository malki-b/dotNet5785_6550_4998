using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.ComponentModel;

namespace PL.Call
{
    public partial class CallListWindow : Window, INotifyPropertyChanged
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public CallInListFields CallFilter { get; set; } = CallInListFields.None;
        public CallInList? SelectedCall { get; set; }
        public CallListWindow()
        {
            InitializeComponent();
            Loaded += CallWindow_Loaded;
            Closed += CallWindow_Closed;
            queryCallList();
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public IEnumerable<CallInList> CallList
        {
            get { return (IEnumerable<CallInList>)GetValue(CallListProperty); }
            set { SetValue(CallListProperty, value); }
        }

        public static readonly DependencyProperty CallListProperty =
            DependencyProperty.Register(
                nameof(CallList),
                typeof(IEnumerable<CallInList>),
                typeof(CallListWindow),
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
                typeof(CallListWindow),
                new PropertyMetadata(string.Empty, OnFilterTextChanged));

        private static void OnFilterTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var window = d as CallListWindow;
            try
            {
                window?.ApplyFilter();
            }
            catch (Exception ex)
            {
                MessageBox.Show("אירעה שגיאה בעת עדכון הפילטר. אנא נסה שוב מאוחר יותר.", "שגיאת פילטר", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private List<CallInList> AllCalls = new();

        private void ApplyFilter()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(FilterText))
                {
                    CallList = AllCalls.ToList();
                }
                else
                {
                   CallList = string.IsNullOrWhiteSpace(FilterText)
                  ? s_bl?.Call.ReadAll()!
                  : s_bl?.Call.GetFilteredAndSortedCalls(filterBy: CallFilter, filterValue: FilterText)!;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("סינון" + ex.Message);
            }
        }

        private void queryCallList()
        {
            try
            {
                var calls = (CallFilter == CallInListFields.None) ?
                    s_bl?.Call.ReadAll()! :
                    s_bl?.Call.ReadAll(null, null)!;

                AllCalls = calls.ToList();
                ApplyFilter();
            }
            catch (Exception ex)
            {
                MessageBox.Show("טעינת הרשימה נכשלה. אנא נסה שוב מאוחר יותר.", "שגיאת טעינה", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void callListObserver() => queryCallList();

        private void CallWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                s_bl.Call.AddObserver(callListObserver);
                queryCallList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("אירעה שגיאה במהלך טעינת החלון. אנא נסה שוב מאוחר יותר.", "שגיאת טעינה", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CallWindow_Closed(object? sender, EventArgs e)
        {
            try
            {
                s_bl.Call.RemoveObserver(callListObserver);
            }
            catch (Exception ex)
            {
                MessageBox.Show("אירעה שגיאה במהלך סגירת החלון. אנא נסה שוב.", "שגיאת סגירה", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Call_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (SelectedCall != null)
                {
                    new CallWindow(SelectedCall.CallId).Show();
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
                CallList = string.IsNullOrWhiteSpace(FilterText)
                    ? s_bl?.Call.ReadAll()!
                    : s_bl?.Call.GetFilteredAndSortedCalls(filterBy: CallFilter, filterValue: FilterText)!;
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
                new CallWindow().Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("אירעה שגיאה בעת הוספת מתנדב. אנא נסה שוב.", "שגיאת הוספה", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void DeleteCall_click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is CallInList call)
            {
                var result = MessageBox.Show("האם אתה בטוח שברצונך למחוק את המתנדב?", "אישור מחיקה", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        s_bl.Call.Delete(call.CallId);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("המחיקה נכשלה: " + ex.Message);
                    }
                }
            }
        }
    }
}


