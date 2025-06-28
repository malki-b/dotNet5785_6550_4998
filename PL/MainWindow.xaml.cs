
using PL.Volunteer;
using PL.Call;
using System;
using System.Windows;

namespace PL
{
    public partial class MainWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public DateTime CurrentTime
        {
            get { return (DateTime)GetValue(CurrentTimeProperty); }
            set { SetValue(CurrentTimeProperty, value); }
        }

        public static readonly DependencyProperty CurrentTimeProperty =
            DependencyProperty.Register(
                "CurrentTime",
                typeof(DateTime),
                typeof(MainWindow),
                new PropertyMetadata(DateTime.Now)
            );
        public TimeSpan CurrentMaxRange
        {
            get { return (TimeSpan)GetValue(currentMaxRangeProperty); }
            set { SetValue(currentMaxRangeProperty, value); }
        }

        public static readonly DependencyProperty currentMaxRangeProperty =
           DependencyProperty.Register(
               "CurrentMaxRange",
               typeof(TimeSpan),
               typeof(MainWindow),
               new PropertyMetadata(null)
           );

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            CurrentTime = s_bl.Admin.GetClock();
            CurrentMaxRange = s_bl.Admin.GetMaxRange();


            s_bl.Admin.AddClockObserver(ClockObserver);
            s_bl.Admin.AddConfigObserver(ConfigObserver);
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            s_bl.Admin.RemoveClockObserver(ClockObserver);
            s_bl.Admin.RemoveConfigObserver(ConfigObserver);
        }
        private void ClockObserver()
        {
            Dispatcher.Invoke(() =>
            {
                CurrentTime = s_bl.Admin.GetClock();
            });
        }

        private void ConfigObserver()
        {
            Dispatcher.Invoke(() =>
            {
                CurrentMaxRange = s_bl.Admin.GetMaxRange();
            });
        }

        private void Add_One_Minute_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                s_bl.Admin.ForwardClock(BO.TimeUnit.Minute);
            }
            catch (Exception ex)
            {
                MessageBox.Show("אירעה שגיאה בעת ניסיון להוסיף דקה לשעון. אנא נסה שוב מאוחר יותר.", "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Add_One_Hour_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                s_bl.Admin.ForwardClock(BO.TimeUnit.Hour);
            }
            catch (Exception ex)
            {
                MessageBox.Show("אירעה שגיאה בעת ניסיון להוסיף שעה לשעון. אנא נסה שוב מאוחר יותר.", "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Add_One_Day_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                s_bl.Admin.ForwardClock(BO.TimeUnit.Day);
            }
            catch (Exception ex)
            {
                MessageBox.Show("אירעה שגיאה בעת ניסיון להוסיף יום לשעון. אנא נסה שוב מאוחר יותר.", "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Add_One_Month_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                s_bl.Admin.ForwardClock(BO.TimeUnit.Month);
            }
            catch (Exception ex)
            {
                MessageBox.Show("אירעה שגיאה בעת ניסיון להוסיף חודש לשעון. אנא נסה שוב מאוחר יותר.", "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Add_One_Year_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                s_bl.Admin.ForwardClock(BO.TimeUnit.Year);
            }
            catch (Exception ex)
            {
                MessageBox.Show("אירעה שגיאה בעת ניסיון להוסיף שנה לשעון. אנא נסה שוב מאוחר יותר.", "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void Update_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                s_bl.Admin.SetMaxRange(CurrentMaxRange);
            }
            catch (Exception ex)
            {
                MessageBox.Show("אירעה שגיאה בעת עדכון טווח המקסימום. אנא נסה שוב מאוחר יותר.", "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Resert_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                s_bl.Admin.ResetDB();
            }
            catch (Exception ex)
            {
                MessageBox.Show("אירעה שגיאה בעת ניסיון לאפס את בסיס הנתונים. אנא נסה שוב מאוחר יותר.", "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Init_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                s_bl.Admin.InitializeDB();
            }
            catch (Exception ex)
            {
                MessageBox.Show("אירעה שגיאה בעת ניסיון לאתחל את בסיס הנתונים. אנא נסה שוב מאוחר יותר.", "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void VolunteerList_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                new VolunteerListWindow().Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("אירעה שגיאה בעת פתיחת רשימת המתנדבים. אנא נסה שוב מאוחר יותר.", "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CallList_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // new CallListWindow().Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("אירעה שגיאה בעת פתיחת רשימת השיחות. אנא נסה שוב מאוחר יותר.", "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }

    }
}
