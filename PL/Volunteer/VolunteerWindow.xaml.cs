//u

using System;
using System.Windows;
using System.Windows.Threading;

namespace PL.Volunteer;

/// <summary>
/// Interaction logic for VolunteerWindow.xaml
/// </summary>
public partial class VolunteerWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public bool IsUpdateMode { get; private set; }

    private volatile bool _observerWorking = false; // ✅ דגל לתמיכה ב-Dispatcher שלב 7

    public VolunteerWindow(int id = 0)
    {
        try
        {
            IsUpdateMode = id != 0; 
            ButtonText = IsUpdateMode ? "Update" : "Add";
            InitializeComponent();

            CurrentVolunteer = IsUpdateMode
                ? s_bl.Volunteer.Read(id)!
                : new BO.Volunteer(
                    0,
                    "",
                    "",
                    "",
                    "",
                    BO.TypeDistance.Air,
                    BO.Role.Volunteer,
                    "12 Rothschild Boulevard, Tel Aviv, Israel",
                    10,
                    10,
                    false,
                    0.0,
                    0,
                    0,
                    0,
                    null
                );
        }
        catch (Exception ex)
        {
            MessageBox.Show($"אירעה שגיאה בטעינת פרטי המתנדב: {ex.Message}", "שגיאת טעינה", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    public string ButtonText
    {
        get { return (string)GetValue(ButtonTextProperty); }
        set { SetValue(ButtonTextProperty, value); }
    }

    public static readonly DependencyProperty ButtonTextProperty =
        DependencyProperty.Register("ButtonText", typeof(string), typeof(VolunteerWindow), new PropertyMetadata(null));

    public BO.Volunteer CurrentVolunteer
    {
        get { return (BO.Volunteer)GetValue(CurrentVolunteerProperty); }
        set { SetValue(CurrentVolunteerProperty, value); }
    }

    public static readonly DependencyProperty CurrentVolunteerProperty =
        DependencyProperty.Register("CurrentVolunteer", typeof(BO.Volunteer), typeof(VolunteerWindow), new PropertyMetadata(null));

    private void AddAndUpdate_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (ButtonText == "Add")
                s_bl.Volunteer.Create(CurrentVolunteer);
            else
                s_bl.Volunteer.Update(CurrentVolunteer.Id, CurrentVolunteer);

            MessageBox.Show($"ה{ButtonText} בוצע בהצלחה.");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"ה{ButtonText} נכשל: {ex.Message}", "שגיאת עדכון", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void VolunteerObserver()
    {
        if (_observerWorking) return;

        _observerWorking = true;
        _ = Dispatcher.BeginInvoke(() =>
        {
            try
            {
                int id = CurrentVolunteer!.Id;
                CurrentVolunteer = s_bl.Volunteer.Read(id);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"אירעה שגיאה בעת עדכון פרטי המתנדב: {ex.Message}", "שגיאת תצוגה", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                _observerWorking = false;
            }
        });
    }

    private void VolunteerWindow_Loaded(object sender, RoutedEventArgs e)
    {
        try
        {
            if (CurrentVolunteer != null && CurrentVolunteer.Id != 0)
                s_bl.Volunteer.AddObserver(CurrentVolunteer.Id, VolunteerObserver);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"אירעה שגיאה במהלך הוספת המעקב למתנדב: {ex.Message}", "שגיאת מעקב", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void VolunteerWindow_Closed(object sender, EventArgs e)
    {
        try
        {
            if (CurrentVolunteer != null)
                s_bl.Volunteer.RemoveObserver(CurrentVolunteer.Id, VolunteerObserver);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"אירעה שגיאה במהלך הסרת המעקב למתנדב: {ex.Message}", "שגיאת הסרה", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
