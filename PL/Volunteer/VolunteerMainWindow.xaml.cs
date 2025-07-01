using System;
using System.Windows;
using Helpers;

namespace PL.Volunteer
{
    public partial class VolunteerMainWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        private bool _canUpdatePassword = false;

        public VolunteerMainWindow(int volunteerId)
        {
            InitializeComponent();
            VolunteerId = volunteerId;

            try
            {
                LoadVolunteer();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"אירעה שגיאה בעת טעינת המתנדב: {ex.Message}", "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public BO.Volunteer CurrentVolunteer
        {
            get => (BO.Volunteer)GetValue(CurrentVolunteerProperty);
            set => SetValue(CurrentVolunteerProperty, value);
        }

        public static readonly DependencyProperty CurrentVolunteerProperty =
            DependencyProperty.Register(nameof(CurrentVolunteer), typeof(BO.Volunteer), typeof(VolunteerMainWindow));

        public string CurrentPassword
        {
            get => (string)GetValue(CurrentPasswordProperty);
            set => SetValue(CurrentPasswordProperty, value);
        }

        public static readonly DependencyProperty CurrentPasswordProperty =
            DependencyProperty.Register(nameof(CurrentPassword), typeof(string), typeof(VolunteerMainWindow));

        public string NewPassword
        {
            get => (string)GetValue(NewPasswordProperty);
            set => SetValue(NewPasswordProperty, value);
        }

        public static readonly DependencyProperty NewPasswordProperty =
            DependencyProperty.Register(nameof(NewPassword), typeof(string), typeof(VolunteerMainWindow));

        public int VolunteerId { get; }

        private void LoadVolunteer()
        {
            try
            {
                CurrentVolunteer = s_bl.Volunteer.Read(VolunteerId);
                //CurrentVolunteer.Password = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"אירעה שגיאה בעת טעינת המתנדב: {ex.Message}", "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void update_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (_canUpdatePassword && !string.IsNullOrWhiteSpace(NewPassword))
                //{
                //    if (NewPassword.Length < 8)
                //    {
                //        MessageBox.Show("הסיסמא חייבת להיות לפחות 8 תווים.");
                //        return;
                //    }

                //    if (!Helpers.VolunteerManager.IsStrongPassword(NewPassword))
                //    {
                //        MessageBox.Show("הסיסמא החדשה אינה חזקה מספיק (חייבת להכיל אות גדולה, אות קטנה, מספר ותו מיוחד)");
                //        return;
                //    }

                //    CurrentVolunteer.Password = NewPassword;
                //}

                s_bl.Volunteer.Update(VolunteerId, CurrentVolunteer);
                MessageBox.Show($"המתנדב {CurrentVolunteer.Id} עודכן בהצלחה");

                CurrentPassword = string.Empty;
                NewPassword = string.Empty;
                _canUpdatePassword = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"אירעה שגיאה בעת עדכון המתנדב: {ex.Message}", "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void volunteerObserver() => LoadVolunteer();

        private void Window_Loaded(object sender, RoutedEventArgs e) => s_bl.Volunteer.AddObserver(VolunteerId, volunteerObserver);

        private void Window_Closed(object sender, EventArgs e) => s_bl.Volunteer.RemoveObserver(VolunteerId, volunteerObserver);

        //private void ApproveCall_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        if (CurrentVolunteer?.CurrentCallInProgress != null)
        //            s_bl.Call.UpdateCallCompletion(VolunteerId, CurrentVolunteer.CurrentCallInProgress.Id);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"אירעה שגיאה בעת אישור השיחה: {ex.Message}", "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}

        //private void CancelCall_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        if (CurrentVolunteer?.CurrentCallInProgress != null)
        //            s_bl.Call.UpdateCallCancellation(VolunteerId, CurrentVolunteer.CurrentCallInProgress.Id);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"אירעה שגיאה בעת ביטול השיחה: {ex.Message}", "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}

        private void ApproveCall_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CurrentVolunteer?.CurrentCallInProgress != null)
                {
                    s_bl.Call.UpdateCallCompletion(VolunteerId, CurrentVolunteer.CurrentCallInProgress.Id);
                    LoadVolunteer(); // עדכון המסך לאחר אישור השיחה
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"אירעה שגיאה בעת אישור השיחה: {ex.Message}", "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelCall_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CurrentVolunteer?.CurrentCallInProgress != null)
                {
                    s_bl.Call.UpdateCallCancellation(VolunteerId, CurrentVolunteer.CurrentCallInProgress.Id);
                    LoadVolunteer(); // עדכון המסך לאחר ביטול השיחה
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"אירעה שגיאה בעת ביטול השיחה: {ex.Message}", "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OpenHistory_Click(object sender, RoutedEventArgs e) =>
            new HistoryCall(VolunteerId).Show();

        private void OpenAvailableCalls_Click(object sender, RoutedEventArgs e) =>
            new Available(VolunteerId).Show();

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            new LoginWindow().Show();
            Close();
        }

        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            // אפשר להוסיף כאן לוגיקה אם יש צורך
        }
    }
}
