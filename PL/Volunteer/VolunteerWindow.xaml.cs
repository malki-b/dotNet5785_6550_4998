using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL.Volunteer;

/// <summary>
/// Interaction logic for VolunteerWindow.xaml
/// </summary>
public partial class VolunteerWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public VolunteerWindow(int id = 0)
    {
        try
        {
            ButtonText = id == 0 ? "Add" : "Update";
            InitializeComponent();
            CurrentVolunteer = (id != 0)
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
            MessageBox.Show("אירעה שגיאה בטעינת פרטי המתנדב. אנא נסה שוב מאוחר יותר.", "שגיאת טעינה", MessageBoxButton.OK, MessageBoxImage.Error);
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
            MessageBox.Show($"ה{ButtonText} נכשל: אירעה שגיאה. אנא נסה שוב.", "שגיאת עדכון", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    private void VolunteerObserver()
    {
        int id = CurrentVolunteer!.Id;
        CurrentVolunteer = null;
        CurrentVolunteer = s_bl.Volunteer.Read(id);
    }
    private void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        try
        {
            if (CurrentVolunteer!.Id != 0)
                s_bl.Volunteer.AddObserver(CurrentVolunteer!.Id, VolunteerObserver);
        }
        catch (Exception ex)
        {
            MessageBox.Show("אירעה שגיאה במהלך הוספת המעקב למתנדב. אנא נסה שוב.", "שגיאת מעקב", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void MainWindow_Closed(object sender, EventArgs e)
    {
        try
        {
            s_bl.Volunteer.RemoveObserver(CurrentVolunteer!.Id, VolunteerObserver);
        }
        catch (Exception ex)
        {
            MessageBox.Show("אירעה שגיאה במהלך הסרת המעקב למתנדב. אנא נסה שוב.", "שגיאת הסרה", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}

