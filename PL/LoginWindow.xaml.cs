using PL.Volunteer;
using System.Windows;



namespace PL
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public LoginWindow()
        {
            InitializeComponent();
        }

        public int? VolunteerId
        {
            get { return (int?)GetValue(VolunteerIdProperty); }
            set { SetValue(VolunteerIdProperty, value); }
        }
        public static readonly DependencyProperty VolunteerIdProperty =
            DependencyProperty.Register("VolunteerId", typeof(int?), typeof(LoginWindow));

        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(string), typeof(LoginWindow));

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int id = VolunteerId ?? 0;
                BO.Role role = s_bl.Volunteer.Login(id,Password);
                if (role == BO.Role.Manager)
                {
                    MessageBoxResult result = MessageBox.Show(
                        "To enter as a manager?",
                        "Choose Screen",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question,
                        MessageBoxResult.Yes
                    );

                    if (result == MessageBoxResult.Yes)
                        new AdminMainWindow().Show();
                    else
                        new VolunteerMainWindow(id).Show();
                }
                else
                {
                    new VolunteerMainWindow(id).Show();
                }
                // this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Login Failed: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }

}

