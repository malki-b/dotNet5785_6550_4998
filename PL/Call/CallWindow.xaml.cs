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

namespace PL.Call;

/// <summary>
/// Interaction logic for CallWindow.xaml
/// </summary>
public partial class CallWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public CallWindow(int id = 0)
    {
        try
        {
            ButtonText = id == 0 ? "Add" : "Update";
            InitializeComponent();
            CurrentCall = (id != 0)
                ? s_bl.Call.RequestCallDetails(id)!
                : new BO.Call();
        }
        catch (Exception ex)
        {
            MessageBox.Show("אירעה שגיאה בטעינת פרטי השיחה. אנא נסה שוב מאוחר יותר.", "שגיאת טעינה", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    public string ButtonText
    {
        get { return (string)GetValue(ButtonTextProperty); }
        set { SetValue(ButtonTextProperty, value); }
    }

    public static readonly DependencyProperty ButtonTextProperty =
    DependencyProperty.Register("ButtonText", typeof(string), typeof(CallWindow), new PropertyMetadata(null));

    public BO.Call CurrentCall
    {
        get { return (BO.Call)GetValue(CurrentCallProperty); }
        set { SetValue(CurrentCallProperty, value); }
    }

    public static readonly DependencyProperty CurrentCallProperty =
        DependencyProperty.Register("CurrentCall", typeof(BO.Call), typeof(CallWindow), new PropertyMetadata(null));

    private void AddAndUpdate_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (ButtonText == "Add")
                s_bl.Call.Create(CurrentCall);
            else
                s_bl.Call.UpdateCallDetails( CurrentCall);

            MessageBox.Show($"ה{ButtonText} בוצע בהצלחה.");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"ה{ButtonText} נכשל: אירעה שגיאה. אנא נסה שוב.", "שגיאת עדכון", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    private void CallObserver()
    {
        int id = CurrentCall!.CallId;
        CurrentCall = null;
        CurrentCall = s_bl.Call.RequestCallDetails(id);
    }
    private void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        try
        {
            if (CurrentCall!.CallId != 0)
                s_bl.Call.AddObserver(CurrentCall!.CallId, CallObserver);
        }
        catch (Exception ex)
        {
            MessageBox.Show("אירעה שגיאה במהלך הוספת המעקב לשיחה. אנא נסה שוב.", "שגיאת מעקב", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void MainWindow_Closed(object sender, EventArgs e)
    {
        try
        {
            s_bl.Call.RemoveObserver(CurrentCall!.CallId, CallObserver);
        }
        catch (Exception ex)
        {
            MessageBox.Show("אירעה שגיאה במהלך הסרת המעקב לשיחה. אנא נסה שוב.", "שגיאת הסרה", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
