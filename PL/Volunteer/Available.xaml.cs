


using BO;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace PL.Volunteer
{
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

        private volatile bool _observerWorking = false; // ✅ שלב 7 – דגל למניעת עומס על Dispatcher

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
                window.queryCallAvailableList(); // פה לא נוגע, כי זו לא מתודת המשקיף
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

        private void CallAvailableListObserver()
        {
            if (_observerWorking) return; // אל תפעיל שוב אם כבר רץ

            _observerWorking = true;
            _ = Dispatcher.BeginInvoke(() =>
            {
                queryCallAvailableList();
                _observerWorking = false;
            });
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            s_bl.Volunteer.AddObserver(CallAvailableListObserver);
            s_bl.Call.AddObserver(CallAvailableListObserver);
            queryCallAvailableList();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            s_bl.Volunteer.RemoveObserver(CallAvailableListObserver);
            s_bl.Call.RemoveObserver(CallAvailableListObserver);
        }

        private void SelectCall_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is OpenCallInList call)
            {
                try
                {
                    s_bl.Call.SelectCallForTreatment(VolunteerId, call.Id);
                    MessageBox.Show("The call was successfully selected for treatment.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"המחיקה נכשלה: {ex.Message}", "שגיאת מחיקה", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void DeleteVolunteer_click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is OpenCallInList call)
            {
                var result = MessageBox.Show("האם אתה בטוח שברצונך למחוק ?", "אישור מחיקה", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        s_bl.Call.Delete(call.Id);
                        queryCallAvailableList();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"המחיקה נכשלה: {ex.Message}", "שגיאת מחיקה", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void SelectionChangedInOpenCallListProp(object sender, SelectionChangedEventArgs e)
        {
        }
    }
}
