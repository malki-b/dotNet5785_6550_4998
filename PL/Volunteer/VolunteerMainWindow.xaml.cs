//using System.Windows;

//namespace PL.Volunteer
//{
//    public partial class VolunteerMainWindow : Window
//    {
//        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
//        private bool _canUpdatePassword = false;

//        public VolunteerMainWindow(int volunteerId)
//        {
//            InitializeComponent();
//            VolunteerId = volunteerId;
//            LoadVolunteer();
//        }
//        public BO.Volunteer CurrentVolunteer
//        {
//            get => (BO.Volunteer)GetValue(CurrentVolunteerProperty);
//            set => SetValue(CurrentVolunteerProperty, value);
//        }
//        public static readonly DependencyProperty CurrentVolunteerProperty =
//    DependencyProperty.Register(nameof(CurrentVolunteer), typeof(BO.Volunteer), typeof(VolunteerMainWindow), new PropertyMetadata(null));

//        public int VolunteerId { get; }
//        private void LoadVolunteer()
//        {
//            CurrentVolunteer = s_bl.Volunteer.GetVolunteerDetails(VolunteerId)!;
//            CurrentVolunteer.Password = "";
//        }

//        private void UpdateInfo_Click(object sender, RoutedEventArgs e)
//        {
//            if (_canUpdatePassword && !string.IsNullOrWhiteSpace(NewPasswordBox.Password))
//            {
//                // בדוק חוזק סיסמא לפני שליחה ל-BL
//                if (NewPasswordBox.Password.Length < 8)
//                {
//                    MessageBox.Show("הסיסמא חייבת להיות לפחות 8 תווים.");
//                    return;
//                }
//                if (!Helpers.VolunteerManager.IsStrongPassword(NewPasswordBox.Password))
//                {
//                    MessageBox.Show("הסיסמא החדשה אינה חזקה מספיק (חייבת להכיל אות גדולה, אות קטנה, מספר ותו מיוחד)");
//                    return;
//                }
//                // שלח את הסיסמא המקורית (לא מוצפנת) ל-BL
//                // בדוק חוזק סיסמא לפני שליחה ל-BL
//                if (NewPasswordBox.Password.Length < 8)
//                {
//                    MessageBox.Show("הסיסמא חייבת להיות לפחות 8 תווים.");
//                    return;
//                }
//                if (!Helpers.VolunteerManager.IsStrongPassword(NewPasswordBox.Password))
//                {
//                    MessageBox.Show("הסיסמא החדשה אינה חזקה מספיק (חייבת להכיל אות גדולה, אות קטנה, מספר ותו מיוחד)");
//                    return;
//                }
//                CurrentVolunteer.Password = NewPasswordBox.Password;
//            }
//            s_bl.Volunteer.UpdateVolunteerDetails(VolunteerId, CurrentVolunteer);
//            MessageBox.Show($"Update the Volunteer: {CurrentVolunteer.Id} successfully");
//            CurrentPasswordBox.Password = "";
//            NewPasswordBox.Password = "";
//            NewPasswordPanel.Visibility = Visibility.Collapsed;
//            _canUpdatePassword = false;
//        }

//        private void VerifyPassword_Click(object sender, RoutedEventArgs e)
//        {
//            var inputPassword = CurrentPasswordBox.Password;
//            if (Helpers.VolunteerManager.VerifyPassword(inputPassword, s_bl.Volunteer.GetVolunteerDetails(VolunteerId).Password))
//            {
//                MessageBox.Show("הכנס סיסמא חדשה לעדכון");
//                NewPasswordPanel.Visibility = Visibility.Visible;
//                _canUpdatePassword = true;
//            }
//            else
//            {
//                MessageBox.Show("סיסמא שגויה");
//                NewPasswordPanel.Visibility = Visibility.Collapsed;
//                _canUpdatePassword = false;
//            }
//        }

//        private void volunteerObserver() => LoadVolunteer();
//        private void Window_Loaded(object sender, RoutedEventArgs e) => s_bl.Volunteer.AddObserver(VolunteerId, volunteerObserver);
//        private void Window_Closed(object sender, EventArgs e) => s_bl.Volunteer.RemoveObserver(VolunteerId, volunteerObserver);

//        private void ApproveCall_Click(object sender, RoutedEventArgs e)
//        {
//            if (CurrentVolunteer?.CallInHandling != null)
//            {
//                s_bl.Call.UpdateCallCompletion(VolunteerId, CurrentVolunteer.CallInHandling.CallId);
//            }
//        }

//        private void CancelCall_Click(object sender, RoutedEventArgs e)
//        {
//            if (CurrentVolunteer?.CallInHandling != null)
//            {
//                s_bl.Call.UpdateCallCancellation(VolunteerId, CurrentVolunteer.CallInHandling.CallId);
//            }
//        }

//        private void OpenHistory_Click(object sender, RoutedEventArgs e)
//        {
//            new CallHistoryWindow(VolunteerId).Show();
//        }

//        private void OpenAvailableCalls_Click(object sender, RoutedEventArgs e)
//        {
//            new AvailableCallsWindow(VolunteerId).Show();
//        }

//        private void Logout_Click(object sender, RoutedEventArgs e)
//        {
//            new LoginWindow().Show();
//            this.Close();
//        }

//    }
//}

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
            LoadVolunteer();
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
            CurrentVolunteer = s_bl.Volunteer.Read(VolunteerId);
            CurrentVolunteer.Password = "";
        }

        private void UpdateInfo_Click(object sender, RoutedEventArgs e)
        {
            if (_canUpdatePassword && !string.IsNullOrWhiteSpace(NewPassword))
            {
                if (NewPassword.Length < 8)
                {
                    MessageBox.Show("הסיסמא חייבת להיות לפחות 8 תווים.");
                    return;
                }

                if (!Helpers.VolunteerManager.IsStrongPassword(NewPassword))
                {
                    MessageBox.Show("הסיסמא החדשה אינה חזקה מספיק (חייבת להכיל אות גדולה, אות קטנה, מספר ותו מיוחד)");
                    return;
                }

                CurrentVolunteer.Password = NewPassword;
            }

            s_bl.Volunteer.Update(VolunteerId, CurrentVolunteer);
            MessageBox.Show($"Update the Volunteer: {CurrentVolunteer.Id} successfully");

            CurrentPassword = string.Empty;
            NewPassword = string.Empty;
            NewPasswordPanel.Visibility = Visibility.Collapsed;
            _canUpdatePassword = false;
        }

        //private void VerifyPassword_Click(object sender, RoutedEventArgs e)
        //{
        //    string originalHashed = s_bl.Volunteer.Read(VolunteerId).Password;

        //    if (Helpers.VolunteerManager.VerifyPassword(CurrentPassword, originalHashed))
        //    {
        //        MessageBox.Show("הכנס סיסמא חדשה לעדכון");
        //        NewPasswordPanel.Visibility = Visibility.Visible;
        //        _canUpdatePassword = true;
        //    }
        //    else
        //    {
        //        MessageBox.Show("סיסמא שגויה");
        //        NewPasswordPanel.Visibility = Visibility.Collapsed;
        //        _canUpdatePassword = false;
        //    }
        //}

        private void volunteerObserver() => LoadVolunteer();
        private void Window_Loaded(object sender, RoutedEventArgs e) => s_bl.Volunteer.AddObserver(VolunteerId, volunteerObserver);
        private void Window_Closed(object sender, EventArgs e) => s_bl.Volunteer.RemoveObserver(VolunteerId, volunteerObserver);

        private void ApproveCall_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentVolunteer?.CurrentCallInProgress != null)
                s_bl.Call.UpdateCallCompletion(VolunteerId, CurrentVolunteer.CurrentCallInProgress.CallId);
        }

        private void CancelCall_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentVolunteer?.CurrentCallInProgress != null)
                s_bl.Call.UpdateCallCancellation(VolunteerId, CurrentVolunteer.CurrentCallInProgress.CallId);
        }

        private void OpenHistory_Click(object sender, RoutedEventArgs e) =>
            new CallHistoryWindow(VolunteerId).Show();

        private void OpenAvailableCalls_Click(object sender, RoutedEventArgs e) =>
            new AvailableCallsWindow(VolunteerId).Show();

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            new LoginWindow().Show();
            Close();
        }
    }
}
