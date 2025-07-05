
using PL.Volunteer;
using PL.Call;
using System;
using System.Windows;

namespace PL
{
    public partial class MainWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        private static CallListWindow? callWindow;
        public int id { get; set; } // Connected volunteer ID

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

        public int Interval
        {
            get { return (int)GetValue(InternalProperty); }
            set { SetValue(InternalProperty, value); }
        }

        public static readonly DependencyProperty InternalProperty =
           DependencyProperty.Register(
               "Internal",
               typeof(int),
               typeof(MainWindow),
               new PropertyMetadata(1000)
           );

        /// <summary>
        /// Dependency property indicating whether the simulator is currently running.
        /// </summary>
        public static readonly DependencyProperty IsSimulatorRunningProperty =
            DependencyProperty.Register("IsSimulatorRunning", typeof(bool), typeof(MainWindow), new PropertyMetadata(false));

        /// <summary>
        /// Gets or sets a value indicating whether the simulator is currently running.
        /// </summary>
        public bool IsSimulatorRunning
        {
            get { return (bool)GetValue(IsSimulatorRunningProperty); }
            set { SetValue(IsSimulatorRunningProperty, value); }
        }
        public TimeSpan RiskRange
        {
            get { return (TimeSpan)GetValue(RiskRangeProperty); }
            set { SetValue(RiskRangeProperty, value); }
        }
        public static readonly DependencyProperty RiskRangeProperty =
            DependencyProperty.Register("RiskRange", typeof(TimeSpan), typeof(MainWindow));

        // Call Statistics Properties
        public int OpenCallsCount
        {
            get { return (int)GetValue(OpenCallsCountProperty); }
            set { SetValue(OpenCallsCountProperty, value); }
        }
        public static readonly DependencyProperty OpenCallsCountProperty =
            DependencyProperty.Register("OpenCallsCount", typeof(int), typeof(MainWindow));

        public int InProgressCallsCount
        {
            get { return (int)GetValue(InProgressCallsCountProperty); }
            set { SetValue(InProgressCallsCountProperty, value); }
        }
        public static readonly DependencyProperty InProgressCallsCountProperty =
            DependencyProperty.Register("InProgressCallsCount", typeof(int), typeof(MainWindow));

        public int ClosedCallsCount
        {
            get { return (int)GetValue(ClosedCallsCountProperty); }
            set { SetValue(ClosedCallsCountProperty, value); }
        }
        public static readonly DependencyProperty ClosedCallsCountProperty =
            DependencyProperty.Register("ClosedCallsCount", typeof(int), typeof(MainWindow));

        public int ExpiredCallsCount
        {
            get { return (int)GetValue(ExpiredCallsCountProperty); }
            set { SetValue(ExpiredCallsCountProperty, value); }
        }
        public static readonly DependencyProperty ExpiredCallsCountProperty =
            DependencyProperty.Register("ExpiredCallsCount", typeof(int), typeof(MainWindow));

        public int OpenAtRiskCallsCount
        {
            get { return (int)GetValue(OpenAtRiskCallsCountProperty); }
            set { SetValue(OpenAtRiskCallsCountProperty, value); }
        }
        public static readonly DependencyProperty OpenAtRiskCallsCountProperty =
            DependencyProperty.Register("OpenAtRiskCallsCount", typeof(int), typeof(MainWindow));

        public int InProgressAtRiskCallsCount
        {
            get { return (int)GetValue(InProgressAtRiskCallsCountProperty); }
            set { SetValue(InProgressAtRiskCallsCountProperty, value); }
        }
        public static readonly DependencyProperty InProgressAtRiskCallsCountProperty =
            DependencyProperty.Register("InProgressAtRiskCallsCount", typeof(int), typeof(MainWindow));

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            Loaded += MainWindow_Loaded;
        }
        public MainWindow(int volunteerId)
        {
            id = volunteerId;
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
            s_bl.Call.AddObserver(callStatisticsObserver); 
            callStatisticsObserver();
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
                new CallListWindow(id).Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("אירעה שגיאה בעת פתיחת רשימת השיחות. אנא נסה שוב מאוחר יותר.", "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Simulator_Click(object sender, RoutedEventArgs e)
        {
            if (IsSimulatorRunning)
            {
                s_bl.Admin.StopSimulator();
                IsSimulatorRunning = false;
            }
            else
            {
                s_bl.Admin.StartSimulator(Interval);
                IsSimulatorRunning = true;
            }
        }

        private void LoadCallStatistics()
        {
            try
            {
                var counts = s_bl.Call.RequestCallCounts();

                OpenCallsCount = counts.Length > (int)BO.Status.Open ? counts[(int)BO.Status.Open] : 0;
                InProgressCallsCount = counts.Length > (int)BO.Status.InProgress ? counts[(int)BO.Status.InProgress] : 0;
                ClosedCallsCount = counts.Length > (int)BO.Status.Closed ? counts[(int)BO.Status.Closed] : 0;
                ExpiredCallsCount = counts.Length > (int)BO.Status.Expired ? counts[(int)BO.Status.Expired] : 0;
                OpenAtRiskCallsCount = counts.Length > (int)BO.Status.OpenAtRisk ? counts[(int)BO.Status.OpenAtRisk] : 0;
                // InProgressAtRiskCallsCount = counts.Length > (int)BO.Status.InProgressAtRisk ? counts[(int)BO.Status.InProgressAtRisk] : 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading call statistics: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void callStatisticsObserver() => LoadCallStatistics();

        // Call Statistics Button Handlers
        private void OpenCalls_Click(object sender, RoutedEventArgs e) => OpenCallListWithFilter(BO.Status.Open);
        private void InProgressCalls_Click(object sender, RoutedEventArgs e) => OpenCallListWithFilter(BO.Status.InProgress);
        private void ClosedCalls_Click(object sender, RoutedEventArgs e) => OpenCallListWithFilter(BO.Status.Closed);
        private void ExpiredCalls_Click(object sender, RoutedEventArgs e) => OpenCallListWithFilter(BO.Status.Expired);
        private void OpenAtRiskCalls_Click(object sender, RoutedEventArgs e) => OpenCallListWithFilter(BO.Status.OpenAtRisk);
        private void InProgressAtRiskCalls_Click(object sender, RoutedEventArgs e) => OpenCallListWithFilter(BO.Status.OpenAtRisk);

        private void OpenCallListWithFilter(BO.Status status)
        {
            try
            {
                if (callWindow == null || !callWindow.IsVisible)
                {
                    callWindow = new CallListWindow(id);
                    callWindow.SelectedStatus = status; // Set the filter
                    callWindow.Closed += (s, args) => callWindow = null;
                    callWindow.Show();
                }
                else
                {
                    if (callWindow.WindowState == WindowState.Minimized)
                        callWindow.WindowState = WindowState.Normal;
                    callWindow.SelectedStatus = status; // Update the filter
                    callWindow.Activate();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening call list: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
