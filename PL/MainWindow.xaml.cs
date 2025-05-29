//using System.ComponentModel;
//using System.Data;
//using System.Text;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Navigation;
//using System.Windows.Shapes;
//using System.Windows.Threading;

//namespace PL
//{
//    /// <summary>
//    /// Interaction logic for MainWindow.xaml
//    /// </summary>
//    /// 





//    public partial class MainWindow : Window , INotifyPropertyChanged
//    {



//        public event PropertyChangedEventHandler PropertyChanged;

//       // private DataTime currentTime;
//        //public DataTime CurrentTime
//        //{
//        //    get { return currentTime; }
//        //    set
//        //    {
//        //        if (currentTime != value)
//        //        {
//        //            currentTime = value;
//        //            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentTime)));
//        //        }
//        //    }
//        //}


//        private DispatcherTimer timer;

//        CurrentTime = s_bl.Admin.GetClock();


//        public DateTime CurrentTime
//        {
//            get { return (DateTime)GetValue(CurrentTimeProperty); }
//            set { SetValue(CurrentTimeProperty, value); }
//        }

//        public static readonly DependencyProperty CurrentTimeProperty =
//            DependencyProperty.Register("CurrentTime", typeof(DateTime), typeof(MainWindow), new PropertyMetadata(DateTime.Now));

//        private void ClockObserver(object sender, EventArgs e)
//        {
//            //CurrentTime = DateTime.Now.ToString("HH:mm:ss");
//            CurrentTime = s_bl.Admin.GetClock();

//        }



//        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
//        //public DateTime CurrentTime
//        //{
//        //    get { return (DateTime)GetValue(CurrentTimeProperty); }
//        //    set { SetValue(CurrentTimeProperty, value); }
//        //}

//        //public static readonly DependencyProperty CurrentTimeProperty =
//        //    DependencyProperty.Register("CurrentTime", typeof(DateTime), typeof(MainWindow));


//        public TimeSpan CurrentMaxRange
//        {
//            get { return (TimeSpan)GetValue(CurrentMaxRangeProperty); }
//            set { SetValue(CurrentMaxRangeProperty, value); }
//        }

//        public static readonly DependencyProperty CurrentMaxRangeProperty =
//            DependencyProperty.Register("CurrentMaxRange", typeof(TimeSpan), typeof(MainWindow));


//        public MainWindow()
//        {
//            InitializeComponent();
//            DataContext=this;

//            timer = new DispatcherTimer();
//            timer.Interval = TimeSpan.FromSeconds(1);
//            timer.Tick += Timer_Tick;
//            timer.Start();
//                        // עדכון ראשוני
//            CurrentTime = DateTime.Now.ToString("HH:mm:ss");

//        }

//        private void Button_Click(object sender, RoutedEventArgs e)
//        {

//        }

//        private void Button_Click_1(object sender, RoutedEventArgs e)
//        {

//        }

//        private void Button_Click_2(object sender, RoutedEventArgs e)
//        {

//        }

//        private void Add_One_Minute_Click(object sender, RoutedEventArgs e)
//        {
//            s_bl.Admin.ForwardClock(BO.TimeUnit.Minute);

//        }

//        private void Add_One_Hour_Click(object sender, RoutedEventArgs e)
//        {
//            s_bl.Admin.ForwardClock(BO.TimeUnit.Hour);

//        }

//        private void Add_One_Day_Click(object sender, RoutedEventArgs e)
//        {
//            s_bl.Admin.ForwardClock(BO.TimeUnit.Day);

//        }

//        private void Add_One_Month_Click(object sender, TextChangedEventArgs e)
//        {
//            s_bl.Admin.ForwardClock(BO.TimeUnit.Month);

//        }

//        private void Add_One_Year_Click(object sender, RoutedEventArgs e)
//        {
//            s_bl.Admin.ForwardClock(BO.TimeUnit.Year);

//        }

//        private void Update_Click(object sender, RoutedEventArgs e)
//        {

//            s_bl.Admin.SetMaxRange(CurrentMaxRange);

//        }

//        private void Add_One_Month_Click(object sender, RoutedEventArgs e)
//        {
//            s_bl.Admin.ForwardClock(BO.TimeUnit.Month);

//        }
//    }
//}
















//using System;
//using System.ComponentModel;
//using System.Windows;
//using System.Windows.Threading;

//namespace PL
//{
//    public partial class MainWindow : Window, INotifyPropertyChanged
//    {
//        public event PropertyChangedEventHandler PropertyChanged;

//        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

//        // הגדרת DependencyProperty עבור CurrentTime מסוג DateTime
//        public DateTime CurrentTime
//        {
//            get { return (DateTime)GetValue(CurrentTimeProperty); }
//            set { SetValue(CurrentTimeProperty, value); }
//        }

//        public static readonly DependencyProperty CurrentTimeProperty =
//            DependencyProperty.Register("CurrentTime", typeof(DateTime), typeof(MainWindow), new PropertyMetadata(DateTime.Now));

//        public TimeSpan CurrentMaxRange
//        {
//            get { return (TimeSpan)GetValue(CurrentMaxRangeProperty); }
//            set { SetValue(CurrentMaxRangeProperty, value); }
//        }

//        public static readonly DependencyProperty CurrentMaxRangeProperty =
//            DependencyProperty.Register("CurrentMaxRange", typeof(TimeSpan), typeof(MainWindow));

//        //private DispatcherTimer timer;

//        public MainWindow()
//        {
//            InitializeComponent();
//            DataContext = this;

//            //timer = new DispatcherTimer();
//            //timer.Interval = TimeSpan.FromSeconds(1);
//            //timer.Tick += ClockObserver; // מתקנים ל-ClockObserver
//            //timer.Start();

//            // עדכון ראשוני
//            CurrentTime = s_bl.Admin.GetClock(); // אם GetClock מחזירה DateTime. אם לא, המר ל-DateTime בהתאם!


//            //קופילוט
//            //this.Loaded += MainWindow_Loaded; // רישום מתודה לאירוע Loaded

//        }



//        //זה מה שקופילוט הביא כדי שיעבוד מיד
//        //כרגע זה עדיין לא עובד

//        //// הוסף למימוש המחלקה:
//        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
//        {
//            CurrentTime = s_bl.Admin.GetClock(); // השמת ערך השעון
//            CurrentMaxRange = s_bl.Admin.GetMaxRange(); // השמת ערך משתנה תצורה (דוגמה לאחד)
//            s_bl.Admin.AddClockObserver(ClockObserver); // רישום משקיף שעון
//            s_bl.Admin.AddConfigObserver(ConfigObserver); // רישום משקיף משתני תצורה
//        }

//        //// הוסף מתודת observer למשתני תצורה:
//        ///        private void ConfigObserver(object sender, EventArgs e)

//        private void ConfigObserver()
//        {
//            CurrentMaxRange = s_bl.Admin.GetMaxRange();
//        }




//        // מתודת ההשקפה על השעון
//        //private void ClockObserver(object sender, EventArgs e)

//        private void ClockObserver()
//        {
//            CurrentTime = s_bl.Admin.GetClock(); // אם GetClock מחזירה DateTime. אם לא, המר ל-DateTime בהתאם!
//        }

//        private void Add_One_Minute_Click(object sender, RoutedEventArgs e)
//        {
//            s_bl.Admin.ForwardClock(BO.TimeUnit.Minute);
//        }

//        private void Add_One_Hour_Click(object sender, RoutedEventArgs e)
//        {
//            s_bl.Admin.ForwardClock(BO.TimeUnit.Hour);
//        }

//        private void Add_One_Day_Click(object sender, RoutedEventArgs e)
//        {
//            s_bl.Admin.ForwardClock(BO.TimeUnit.Day);
//        }

//        private void Add_One_Month_Click(object sender, RoutedEventArgs e)
//        {
//            s_bl.Admin.ForwardClock(BO.TimeUnit.Month);
//        }

//        private void Add_One_Year_Click(object sender, RoutedEventArgs e)
//        {
//            s_bl.Admin.ForwardClock(BO.TimeUnit.Year);
//        }

//        private void Update_Click(object sender, RoutedEventArgs e)
//        {
//            s_bl.Admin.SetMaxRange(CurrentMaxRange);
//        }

//        private void Resert_Click(object sender, RoutedEventArgs e)
//        {
//            s_bl.Admin.ResetDB();
//        }

//        private void Init_Click(object sender, RoutedEventArgs e)
//        {
//            s_bl.Admin.InitializeDB();

//        }

//        private void Button_Click_2(object sender, RoutedEventArgs e)
//        {

//        }

//        private void Button_Click(object sender, RoutedEventArgs e)
//        {

//        }

//        private void Button_Click_1(object sender, RoutedEventArgs e)
//        {

//        }

//        // מחקתי את כל Button_Click הריקים
//        // מחקתי Add_One_Month_Click עם TextChangedEventArgs

//        // אפשר להוסיף כאן מתודות השקפה למשתני תצורה נוספים
//    }
//}



//עובדדדדדדדדדדדדדדדדדדדדדדדדדדדדדדדדדדד
//using System;
//using System.Windows;
//using System.Windows.Threading;

//namespace PL
//{
//    public partial class MainWindow : Window
//    {
//        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

//        // DependencyProperty עבור CurrentTime
//        public DateTime CurrentTime
//        {
//            get { return (DateTime)GetValue(CurrentTimeProperty); }
//            set { SetValue(CurrentTimeProperty, value); }
//        }

//        public static readonly DependencyProperty CurrentTimeProperty =
//            DependencyProperty.Register(
//                "CurrentTime",
//                typeof(DateTime),
//                typeof(MainWindow),
//                new PropertyMetadata(DateTime.Now)
//            );

//        // DependencyProperty עבור CurrentMaxRange
//        public TimeSpan CurrentMaxRange
//        {
//            get { return (TimeSpan)GetValue(CurrentMaxRangeProperty); }
//            set { SetValue(CurrentMaxRangeProperty, value); }
//        }

//        public static readonly DependencyProperty CurrentMaxRangeProperty =
//            DependencyProperty.Register(
//                "CurrentMaxRange",
//                typeof(TimeSpan),
//                typeof(MainWindow),
//                new PropertyMetadata(default(TimeSpan))
//            );

//        public MainWindow()
//        {
//            InitializeComponent();
//            DataContext = this;

//            // עדכון ראשוני של הערכים
//            CurrentTime = s_bl.Admin.GetClock();
//            CurrentMaxRange = s_bl.Admin.GetMaxRange();

//            // רישום לאירוע Loaded (קריטי!)
//            Loaded += MainWindow_Loaded;
//        }

//        // רישום observerים ל־AdminManager דרך IAdmin
//        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
//        {
//            // עדכון ראשוני (לא חובה, אבל מוודא סנכרון)
//            CurrentTime = s_bl.Admin.GetClock();
//            CurrentMaxRange = s_bl.Admin.GetMaxRange();

//            s_bl.Admin.AddClockObserver(ClockObserver);
//            s_bl.Admin.AddConfigObserver(ConfigObserver);
//        }

//        // observer לעדכון השעון
//        private void ClockObserver()
//        {
//            // לוודא שהעדכון מתבצע ב־UI Thread
//            Dispatcher.Invoke(() =>
//            {
//                CurrentTime = s_bl.Admin.GetClock();
//            });
//        }

//        // observer לעדכון משתני תצורה
//        private void ConfigObserver()
//        {
//            Dispatcher.Invoke(() =>
//            {
//                CurrentMaxRange = s_bl.Admin.GetMaxRange();
//            });
//        }

//        // פעולות כפתורים - אין שינוי, הכל תקין
//        private void Add_One_Minute_Click(object sender, RoutedEventArgs e)
//        {
//            s_bl.Admin.ForwardClock(BO.TimeUnit.Minute);
//        }

//        private void Add_One_Hour_Click(object sender, RoutedEventArgs e)
//        {
//            s_bl.Admin.ForwardClock(BO.TimeUnit.Hour);
//        }

//        private void Add_One_Day_Click(object sender, RoutedEventArgs e)
//        {
//            s_bl.Admin.ForwardClock(BO.TimeUnit.Day);
//        }

//        private void Add_One_Month_Click(object sender, RoutedEventArgs e)
//        {
//            s_bl.Admin.ForwardClock(BO.TimeUnit.Month);
//        }

//        private void Add_One_Year_Click(object sender, RoutedEventArgs e)
//        {
//            s_bl.Admin.ForwardClock(BO.TimeUnit.Year);
//        }

//        private void Update_Click(object sender, RoutedEventArgs e)
//        {
//            s_bl.Admin.SetMaxRange(CurrentMaxRange);
//        }

//        private void Resert_Click(object sender, RoutedEventArgs e)
//        {
//            s_bl.Admin.ResetDB();
//        }

//        private void Init_Click(object sender, RoutedEventArgs e)
//        {
//            s_bl.Admin.InitializeDB();
//        }

//        private void Button_Click_2(object sender, RoutedEventArgs e)
//        {
//            // פעולה עתידית
//        }

//        private void Button_Click(object sender, RoutedEventArgs e)
//        {
//            // פעולה עתידית
//        }

//        private void Button_Click_1(object sender, RoutedEventArgs e)
//        {
//            // פעולה עתידית
//        }
//    }
//}



using PL.Volunteer;
using PL.Call;
using System;
using System.Windows;

namespace PL
{
    public partial class MainWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        // === שינוי: שדה לעבודה פנימית עם ה-TimeSpan ===
       // private TimeSpan _currentMaxRange;

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
        // === שינוי: מחרוזת שמוצגת ב-TextBox ומומרת ל-TimeSpan ===
        //private string _currentMaxRangeString;
        //public string CurrentMaxRangeString
        //{
        //    get => _currentMaxRangeString;
        //    set
        //    {
        //        _currentMaxRangeString = value;
        //        ErrorMessage = ""; // ננקה כל שגיאה קודמת
        //        try
        //        {
        //            _currentMaxRange = TimeSpan.Parse(value);
        //        }
        //        catch
        //        {
        //            ErrorMessage = "יש להכניס ערך זמן חוקי (למשל 01:00:00)";
        //        }
        //    }
        //}
        //public string CurrentMaxRangeString
        //{
        //    get => _currentMaxRangeString;
        //    set
        //    {
        //        _currentMaxRangeString = value;
        //        if (string.IsNullOrWhiteSpace(value))
        //        {
        //            ErrorMessage = "";
        //            return;
        //        }
        //        try
        //        {
        //            _currentMaxRange = TimeSpan.Parse(value);
        //            ErrorMessage = "";
        //        }
        //        catch
        //        {
        //            ErrorMessage = "יש להכניס ערך מספרי חוקי";
        //        }
        //    }
        //}

        // === שינוי: הודעת שגיאה לתצוגה מתחת ל-TextBox ===
        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                // גרום לרענון UI
                Dispatcher.Invoke(() =>
                {
                    SetValue(ErrorMessageProperty, value);
                });
            }
        }
        public static readonly DependencyProperty ErrorMessageProperty =
            DependencyProperty.Register(
                "ErrorMessage",
                typeof(string),
                typeof(MainWindow),
                new PropertyMetadata("")
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

            // === שינוי: עדכון מחרוזת תצוגה מה-TimeSpan האמיתי ===
            //CurrentMaxRangeString = _currentMaxRange.ToString();

            s_bl.Admin.AddClockObserver(ClockObserver);
            s_bl.Admin.AddConfigObserver(ConfigObserver);
        }

        private void MainWindow_Closed(object sender, EventArgs e) // *** שינוי: מימוש אירוע הסגירה ***
        {
            s_bl.Admin.RemoveClockObserver(ClockObserver);  // *** שינוי: הסרת משקיף השעון ***
            s_bl.Admin.RemoveConfigObserver(ConfigObserver); // *** שינוי: הסרת משקיף התצורה ***
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
               // CurrentMaxRangeString = _currentMaxRange.ToString();
            });
        }

        private void Add_One_Minute_Click(object sender, RoutedEventArgs e)
        {
            s_bl.Admin.ForwardClock(BO.TimeUnit.Minute);
        }

        private void Add_One_Hour_Click(object sender, RoutedEventArgs e)
        {
            s_bl.Admin.ForwardClock(BO.TimeUnit.Hour);
        }

        private void Add_One_Day_Click(object sender, RoutedEventArgs e)
        {
            s_bl.Admin.ForwardClock(BO.TimeUnit.Day);
        }

        private void Add_One_Month_Click(object sender, RoutedEventArgs e)
        {
            s_bl.Admin.ForwardClock(BO.TimeUnit.Month);
        }

        private void Add_One_Year_Click(object sender, RoutedEventArgs e)
        {
            s_bl.Admin.ForwardClock(BO.TimeUnit.Year);
        }

        //private void Update_Click(object sender, RoutedEventArgs e)
        //{
        //    string input = CurrentMaxRangeString;
        //    CurrentMaxRangeString = "";
        //    if (string.IsNullOrEmpty(ErrorMessage))
        //    {
        //        if (!string.IsNullOrWhiteSpace(input))
        //            s_bl.Admin.SetMaxRange(_currentMaxRange);
        //    }
        //    else
        //    {
        //        MessageBox.Show(ErrorMessage, "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            s_bl.Admin.SetMaxRange(CurrentMaxRange);
            //if (string.IsNullOrWhiteSpace(CurrentMaxRangeString) || !string.IsNullOrEmpty(ErrorMessage))
            //{
            //    MessageBox.Show("יש להכניס ערך מספרי חוקי", "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
            //    CurrentMaxRangeString = "";

            //}
            //else
            //{
            //    s_bl.Admin.SetMaxRange(_currentMaxRange);
            //    CurrentMaxRangeString = "";

            //}
            //CurrentMaxRangeString = "";
        }

        private void Resert_Click(object sender, RoutedEventArgs e)
        {
            s_bl.Admin.ResetDB();
        }

        private void Init_Click(object sender, RoutedEventArgs e)
        {
            s_bl.Admin.InitializeDB();
        }
        private void HandleVolunteer_Click(object sender, RoutedEventArgs e)
        {
            new VolunteerListWindow().Show();
        }

        private void HandleCall_Click(object sender, RoutedEventArgs e)
        {
            //new CallListWindow().Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }
    }
}