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

//        private void clockObserver(object sender, EventArgs e)
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
















using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;

namespace PL
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        // הגדרת DependencyProperty עבור CurrentTime מסוג DateTime
        public DateTime CurrentTime
        {
            get { return (DateTime)GetValue(CurrentTimeProperty); }
            set { SetValue(CurrentTimeProperty, value); }
        }

        public static readonly DependencyProperty CurrentTimeProperty =
            DependencyProperty.Register("CurrentTime", typeof(DateTime), typeof(MainWindow), new PropertyMetadata(DateTime.Now));

        public TimeSpan CurrentMaxRange
        {
            get { return (TimeSpan)GetValue(CurrentMaxRangeProperty); }
            set { SetValue(CurrentMaxRangeProperty, value); }
        }

        public static readonly DependencyProperty CurrentMaxRangeProperty =
            DependencyProperty.Register("CurrentMaxRange", typeof(TimeSpan), typeof(MainWindow));

        //private DispatcherTimer timer;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            //timer = new DispatcherTimer();
            //timer.Interval = TimeSpan.FromSeconds(1);
            //timer.Tick += clockObserver; // מתקנים ל-clockObserver
            //timer.Start();

            // עדכון ראשוני
            CurrentTime = s_bl.Admin.GetClock(); // אם GetClock מחזירה DateTime. אם לא, המר ל-DateTime בהתאם!


            //קופילוט
            //this.Loaded += MainWindow_Loaded; // רישום מתודה לאירוע Loaded

        }



        //זה מה שקופילוט הביא כדי שיעבוד מיד
        //כרגע זה עדיין לא עובד

        //// הוסף למימוש המחלקה:
        //private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        //{
        //    CurrentTime = s_bl.Admin.GetClock(); // השמת ערך השעון
        //    CurrentMaxRange = s_bl.Admin.GetMaxRange(); // השמת ערך משתנה תצורה (דוגמה לאחד)
        //    s_bl.Admin.AddClockObserver(clockObserver); // רישום משקיף שעון
        //    s_bl.Admin.AddConfigObserver(configObserver); // רישום משקיף משתני תצורה
        //}

        //// הוסף מתודת observer למשתני תצורה:
        //private void configObserver(object sender, EventArgs e)
        //{
        //    CurrentMaxRange = s_bl.Admin.GetMaxRange();
        //}




        // מתודת ההשקפה על השעון
        private void clockObserver(object sender, EventArgs e)
        {
            CurrentTime = s_bl.Admin.GetClock(); // אם GetClock מחזירה DateTime. אם לא, המר ל-DateTime בהתאם!
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

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            s_bl.Admin.SetMaxRange(CurrentMaxRange);
        }

        private void Resert_Click(object sender, RoutedEventArgs e)
        {
            s_bl.Admin.ResetDB();
        }

        private void Init_Click(object sender, RoutedEventArgs e)
        {
            s_bl.Admin.InitializeDB();

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        // מחקתי את כל Button_Click הריקים
        // מחקתי Add_One_Month_Click עם TextChangedEventArgs

        // אפשר להוסיף כאן מתודות השקפה למשתני תצורה נוספים
    }
}