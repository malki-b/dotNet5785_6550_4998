//using BO;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Collections.ObjectModel;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Shapes;
//using System.ComponentModel;
//namespace PL.Volunteer;

///// <summary>
///// Interaction logic for VolunteerListWindow.xaml
///// </summary>
/////

//public partial class VolunteerListWindow : Window, INotifyPropertyChanged // ✅ הוספתי INotifyPropertyChanged
//{
//    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
//    public BO.VolunteerInListFields VolunteerFilter { get; set; } = BO.VolunteerInListFields.None;

//    public BO.VolunteerInList? SelectedVolunteer { get; set; }

//    public VolunteerListWindow()
//    {
//        InitializeComponent();
//        //DataContext = this;
//        Loaded += VolunteerWindow_Loaded;
//        queryVolunteerList();
//    }

//    public event PropertyChangedEventHandler? PropertyChanged;
//    protected void OnPropertyChanged(string propertyName) 
//    {
//        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
//    }

//    public IEnumerable<BO.VolunteerInList> VolunteerList
//    {
//        get { return (IEnumerable<BO.VolunteerInList>)GetValue(VolunteerListProperty); }
//        set { SetValue(VolunteerListProperty, value); }
//    }


//    // ✅ הוספה: שמירת כל הרשימת מתנדבים (לצורך סינון)
//    private List<BO.VolunteerInList> AllVolunteers = new();

//    // ✅ הוספה חדשה: שדה טקסט שאליו נקשר ה־TextBox מה־XAML
//    private string _filterText = "";
//    public string FilterText
//    {
//        get => _filterText;
//        set
//        {
//            _filterText = value;
//            OnPropertyChanged(nameof(FilterText)); // ✅ חובה לעדכן את ה־UI

//            ApplyFilter(); // ← בכל שינוי טקסט, נבצע סינון
//        }
//    }

//    // ✅ הוספה חדשה: פונקציה שמסננת את הרשימה לפי הטקסט של המשתמש
//    private void ApplyFilter()
//    {
//        if (string.IsNullOrWhiteSpace(FilterText))
//        {
//            // אם אין טקסט - נציג את כל הרשימה
//            VolunteerList = AllVolunteers.ToList();
//        }
//        else
//        {
//            // אחרת נסנן לפי שם (בצורה לא תלויה רישיות)

//            //VolunteerList = AllVolunteers
//            //    .Where(v => v.FullName.Contains(FilterText, StringComparison.OrdinalIgnoreCase))
//            //    .ToList();

//        }
//    }


//    public static readonly DependencyProperty VolunteerListProperty =
//        DependencyProperty.Register("VolunteerList", typeof(IEnumerable<BO.VolunteerInList>), typeof(VolunteerListWindow), new PropertyMetadata(null));




//    private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
//    {
//      //  VolunteerList = (VolunteerFilter == BO.VolunteerField.None) ?
//      //s_bl?.Volunteer.ReadAll()! : s_bl?.Volunteer.GetFilteredAndSortedVolunteers()!;//////here


//        VolunteerList = string.IsNullOrWhiteSpace(FilterText)
//    ? s_bl?.Volunteer.ReadAll()!
//    : s_bl?.Volunteer.GetFilteredAndSortedVolunteers(filterBy: VolunteerFilter, filterValue: FilterText)!;
//    }


//    private void Volunteer_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
//    {
//        //if (sender is ListView listView)
//        //{
//        //    if (SelectedVolunteer != null)
//        //    {
//        //        int? id = (listView.SelectedItem as BO.VolunteerInList)?.VolunteerId;
//        //        new VolunteerWindow(id ?? 0).Show();
//        //    }

//        //}
//        if (SelectedVolunteer != null) ;
//        new VolunteerWindow(SelectedVolunteer.VolunteerId).Show();

//    }




//    //private void queryVolunteerList()
//    //=> VolunteerList = (VolunteerFilter == BO.VolunteerField.None) ?
//    //    s_bl?.Volunteer.ReadAll()! : s_bl?.Volunteer.ReadAll(null, null)!;

//    //gpt
//    private void queryVolunteerList()
//    {
//        var volunteers = (VolunteerFilter == BO.VolunteerInListFields.None) ?
//            s_bl?.Volunteer.ReadAll()! :
//            s_bl?.Volunteer.ReadAll(null, null)!;

//        AllVolunteers = volunteers.ToList(); // שומרת את הרשימה המלאה
//        ApplyFilter(); // מסננת לפי טקסט מהמשתמש
//    }


//    private void volunteerListObserver()
//        => queryVolunteerList();

//    private void VolunteerWindow_Loaded(object sender, RoutedEventArgs e)
//    {
//        s_bl.Volunteer.AddObserver(volunteerListObserver);
//        queryVolunteerList();

//    }

//    private void VolunteerWindow_Closed(object sender, EventArgs e)
//        => s_bl.Volunteer.RemoveObserver(volunteerListObserver);

//    private void Add_Click(object sender, RoutedEventArgs e)
//    {
//        new VolunteerWindow().Show();

//    }

//    private void DeleteVolunteer_click(object sender, RoutedEventArgs e)
//    {


//        if (sender is Button btn && btn.CommandParameter is BO.VolunteerInList volunteer)
//        {
//            MessageBox.Show("האם אתה בטוח שברצונך למחוק את המתנדב?", "אישור מחיקה", MessageBoxButton.YesNo);

//            try
//            {
//                s_bl.Volunteer.Delete(volunteer.VolunteerId);
//                // אין צורך לקרוא ל-queryVolunteerList אם ה-observer עובד
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("המחיקה נכשלה: " + ex.Message);
//            }
//        }
//        //if (SelectedVolunteer != null)
//        //{
//        //    try
//        //    {
//        //        s_bl.Volunteer.Delete(SelectedVolunteer.VolunteerId);
//        //        // אין צורך לקרוא ל-queryVolunteerList אם ה-observer עובד
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        MessageBox.Show("המחיקה נכשלה: " + ex.Message);
//        //    }
//        //}


//    }
//}


////{
////    /// <summary>
////    /// Interaction logic for VolunteerListWindow.xaml
////    /// </summary>
////    public partial class VolunteerListWindow : Window
////    {
////        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

////        public IEnumerable<BO.VolunteerInList> VolunteerList
////        {
////            get { return (IEnumerable<BO.VolunteerInList>)GetValue(VolunteerListProperty); }
////            set { SetValue(VolunteerListProperty, value); }
////        }

////        public static readonly DependencyProperty VolunteerListProperty =
////            DependencyProperty.Register("VolunteerList", typeof(IEnumerable<BO.VolunteerInList>), typeof(VolunteerInList));

////        public BO.VolunteerInListFields SelectedSortField { get; set; } = BO.VolunteerInListFields.None;
////        public BO.CallInList? SelectedVolunteer
////        {
////            get;
////            set;
////        }

////        public VolunteerListWindow()
////        {
////            InitializeComponent();
////        }
////        /// <summary>
////        /// Handles the selection change event to update the volunteer list based on the selected sort field.
////        /// </summary>
////        /// <param name="sender">The source of the event</param>
////        /// <param name="e">The event data that contains the new selection state.</param>
////        private void ChangeVolunteersListSort(object sender, SelectionChangedEventArgs e)
////        {
////            UpdateVolunteersList();
////        }
////        /// <summary>
////        /// Update the volunteer list from bl
////        /// </summary>
////        private void UpdateVolunteersList()
////        {
////            VolunteerList = (SelectedSortField == BO.VolunteerInListFields.None) ?
////                s_bl?.Volunteer.ReadAll()! :
////                s_bl?.Volunteer.ReadAll(null, SelectedSortField)!;
////        }

////        /// <summary>
////        /// update the volunteer list
////        /// </summary>
////        private void VolunteerListObserver()
////        {
////            UpdateVolunteersList();
////        }

////        /// <summary>
////        /// add the volunteerListObserver to the observers list
////        /// </summary>
////        /// <param name="sender">The source of the event</param>
////        /// <param name="e">The event data that contains the new selection state.</param>
////        private void Window_Loaded(object sender, RoutedEventArgs e)
////        {
////            s_bl.Volunteer.AddObserver(VolunteerListObserver);
////        }

////        /// <summary>
////        /// remove the volunteerListObserver from the observers list
////        /// </summary>
////        /// <param name="sender">The source of the event</param>
////        /// <param name="e">The event data that contains the new selection state.</param>

////        private void Window_Closed(object sender, EventArgs e)
////        {
////            s_bl.Volunteer.RemoveObserver(VolunteerListObserver);
////        }

////        private void AddVolunteer_Click(object sender, RoutedEventArgs e)
////        {
////            new VolunteerWindow().Show();
////        }

////        private void Volunteer_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
////        {
////            if (sender is ListView listView)
////            {
////                if (SelectedVolunteer != null)
////                {
////                    int? id = (listView.SelectedItem as BO.VolunteerInList)?.Id;
////                    new VolunteerWindow(id ?? 0).Show();
////                }

////            }
////        }
////        public ICommand DeleteCommand => new RelayCommand<BO.CallInList>(DeleteVolunteer);

////        private void DeleteVolunteer(BO.VolunteerInList item)
////        {
////            var result = MessageBox.Show("האם אתה בטוח שברצונך למחוק את המתנדב?", "אישור מחיקה", MessageBoxButton.YesNo);

////            if (result == MessageBoxResult.Yes)
////            {
////                try
////                {
////                    s_bl.Volunteer.Delete((int)item.Id);
////                }
////                catch
////                {
////                    var failedErase = MessageBox.Show("המחיקה נכשלה");
////                }
////            }
////        }
////    }
////}





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

        // --------------------------
        // 📌 INotifyPropertyChanged
        // --------------------------
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        // --------------------------
        // 📌 VolunteerList Dependency Property
        // --------------------------
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

        // --------------------------
        // 📌 FilterText Dependency Property
        // --------------------------
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
            window?.ApplyFilter();
        }

        // --------------------------
        // 📌 רשימת כל המתנדבים הלא מסוננים
        // --------------------------
        private List<VolunteerInList> AllVolunteers = new();

        // --------------------------
        // 📌 סינון לפי טקסט
        // --------------------------
        private void ApplyFilter()
        {
            if (string.IsNullOrWhiteSpace(FilterText))
            {
                VolunteerList = AllVolunteers.ToList();
            }
            else
            {
                VolunteerList = AllVolunteers
                    .Where(v => v.FullName.Contains(FilterText, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
        }

        // --------------------------
        // 📌 טעינת הנתונים
        // --------------------------
        private void queryVolunteerList()
        {
            var volunteers = (VolunteerFilter == VolunteerInListFields.None) ?
                s_bl?.Volunteer.ReadAll()! :
                s_bl?.Volunteer.ReadAll(null, null)!;

            AllVolunteers = volunteers.ToList(); // שומרת את כל הרשימה
            ApplyFilter(); // מסננת לפי הטקסט מהמשתמש
        }

        // --------------------------
        // 📌 עדכון ברגע שיש שינוי בדאטה
        // --------------------------
        private void volunteerListObserver() => queryVolunteerList();

        private void VolunteerWindow_Loaded(object sender, RoutedEventArgs e)
        {
            s_bl.Volunteer.AddObserver(volunteerListObserver);
            queryVolunteerList();
        }

        private void VolunteerWindow_Closed(object? sender, EventArgs e)
        {
            s_bl.Volunteer.RemoveObserver(volunteerListObserver);
        }

        // --------------------------
        // 📌 לחיצה כפולה פותחת חלון מתנדב
        // --------------------------
        private void Volunteer_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SelectedVolunteer != null)
            {
                new VolunteerWindow(SelectedVolunteer.VolunteerId).Show();
            }
        }

        // --------------------------
        // 📌 שינוי בקומבו – לפי פילטר
        // --------------------------
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            VolunteerList = string.IsNullOrWhiteSpace(FilterText)
                ? s_bl?.Volunteer.ReadAll()!
                : s_bl?.Volunteer.GetFilteredAndSortedVolunteers(filterBy: VolunteerFilter, filterValue: FilterText)!;
        }

        // --------------------------
        // 📌 הוספת מתנדב
        // --------------------------
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            new VolunteerWindow().Show();
        }

        // --------------------------
        // 📌 מחיקת מתנדב
        // --------------------------
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
    }
}
