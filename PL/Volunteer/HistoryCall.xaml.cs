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

namespace PL.Volunteer
{
    /// <summary>
    /// Interaction logic for CallHistoryWindow.xaml
    /// </summary>
    public partial class CallHistoryWindow : Window
    { 
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public CallHistoryWindow(int volunteerId)
        {
            InitializeComponent();
            VolunteerId = volunteerId;
            CallHistoryList = s_bl.Call.GetClosedCallsByVolunteer(volunteerId);
        }

        public int VolunteerId { get; }
        public IEnumerable<BO.ClosedCallInList> CallHistoryList
        {
            get { return (IEnumerable<BO.ClosedCallInList>)GetValue(CallHistoryListListProperty); }
            set { SetValue(CallHistoryListListProperty, value); }
        }
        public static readonly DependencyProperty CallHistoryListListProperty =
        DependencyProperty.Register("CallHistoryList", typeof(IEnumerable<BO.ClosedCallInList>), typeof(CallHistoryWindow), new PropertyMetadata(null));

        public BO.ClosedCallField CallSortProp { get; set; } = BO.ClosedCallField.None;
        public BO.TypeOfCall TypeOfCallFilterProp { get; set; } = BO.TypeOfCall.None;

        //public string? FilterValue
        //{
        //    get => (string?)GetValue(FilterValueProperty);
        //    set => SetValue(FilterValueProperty, value);
        //}

        //public static readonly DependencyProperty FilterValueProperty =
        //    DependencyProperty.Register(nameof(FilterValue),typeof(string),typeof(CallHistoryWindow),new PropertyMetadata(null));

        private void SelectionChangedInCallHistoryListProp(object sender, RoutedEventArgs e)
        {
            try { queryCallHistoryList(); }
            catch (Exception ex) { MessageBox.Show($"Failed to load the CallHistoryList: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
        }
        private void queryCallHistoryList()
        {
            if (TypeOfCallFilterProp == BO.TypeOfCall.None)
            {
                CallHistoryList = s_bl?.Call.GetClosedCallsByVolunteer(VolunteerId, null,
                    CallSortProp == BO.ClosedCallField.None ? null : CallSortProp
                )!;
            }
            else
            {
                CallHistoryList = s_bl?.Call.GetClosedCallsByVolunteer(VolunteerId, TypeOfCallFilterProp,
                       CallSortProp == BO.ClosedCallField.None ? null : CallSortProp
                   )!;
            }
        }
        private void CallHistoryListObserver() => queryCallHistoryList();
        private void Window_Loaded(object sender, RoutedEventArgs e) => s_bl.Volunteer.AddObserver(CallHistoryListObserver);
        private void Window_Closed(object sender, EventArgs e) => s_bl.Volunteer.RemoveObserver(CallHistoryListObserver);

        //private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{

        //}

        //private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{

        //}

        //private void OpenVolunteerWindow(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        new VolunteerWindow().Show();
        //    }
        //    catch (Exception ex) { MessageBox.Show($"Failed to load the VolunteerList: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
        //}
        //public BO.VolunteerInList? SelectedVolunteer { get; set; }
//        //private void lsvVolunteerList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
//        //{
//        //    try
//        //    {
//        //        if (SelectedVolunteer != null)
//        //        {
//        //            int id = SelectedVolunteer.VolunteerId ?? 0;
//        //            new VolunteerWindow(id).Show();
//        //        }
//        //    }
//        //    catch (Exception ex) { MessageBox.Show($"Failed to load the VolunteerWindow: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
//        //}
//        //private void DeleteVolunteer_Click(object sender, RoutedEventArgs e)
//        //{
//        //    try
//        //    {
//        //        var button = sender as Button;
//        //        if (button == null) return;

//        //        int volunteerId = (int)button.Tag;

//        //        var result = MessageBox.Show(
//        //            $"Are you sure you want to delete volunteer #{volunteerId}?",
//        //            "Confirm Deletion",
//        //            MessageBoxButton.YesNo,
//        //            MessageBoxImage.Warning);

//        //        if (result == MessageBoxResult.Yes)
//        //        {
//        //            s_bl.Volunteer.DeleteVolunteer(volunteerId);
//        //        }
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        MessageBox.Show($"Failed to delete the volunteer: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
//        //    }
//        //}

//    }
//}





//< Window x: Class = "PL.Volunteer.AvailableCallsWindow"
//        xmlns = "http://schemas.microsoft.com/winfx/2006/xaml/presentation"
//        xmlns: x = "http://schemas.microsoft.com/winfx/2006/xaml"
//        Title = "Available Calls"
//        Height = "700"
//        Width = "1000"
//        DataContext = "{Binding RelativeSource={RelativeSource Mode=Self}}"
//        WindowStartupLocation = "CenterScreen"
//        FontFamily = "Segoe UI"
//        Background = "#f7faff" >



//    < Border >
//        < DockPanel LastChildFill = "True" Margin = "0" >

//            < !--Header Section-- >
//            < StackPanel DockPanel.Dock = "Top" Orientation = "Vertical" Margin = "0,0,0,20" >
//                < TextBlock Text = "Availanle Calls List"
//                           FontSize = "26"
//                           FontWeight = "Bold"
//                           Foreground = "#2770f1"
//                           Margin = "20,10,0,10" Width = "228" />

//                < !--Filters Row-- >
//                < UniformGrid Columns = "2" Margin = "20,0,20,0" HorizontalAlignment = "Stretch" Width = "822" >
//                    < !--Sorting-- >
//                    < StackPanel Orientation = "Vertical" Margin = "0,0,12,0" >
//                        < TextBlock Text = "Sort by"
//                                   FontSize = "14"
//                                   FontWeight = "SemiBold"
//                                   Margin = "0,0,0,4" />
//                        < ComboBox ItemsSource = "{Binding Source={StaticResource OpenCallFieldKey}}"
//                                  SelectedValue = "{Binding Path=CallSortProp, Mode=TwoWay}"
//                                  SelectionChanged = "SelectionChangedInOpenCallListProp"
//                                  Height = "32"
//                                  FontSize = "14" />
//                    </ StackPanel >

//                    < !--Filter by Type -->
//                    <StackPanel Orientation="Vertical" Margin="0,0,12,0">
//                        <TextBlock Text="Filter by Type"
//                                   FontSize="14"
//                                   FontWeight="SemiBold"
//                                   Margin="0,0,0,4"/>
//                        <ComboBox ItemsSource="{Binding Source={StaticResource TypeOfCallPropertyKey}}"
//                                  SelectionChanged="SelectionChangedInOpenCallListProp"
//                                  SelectedValue="{Binding Path=TypeOfCallFilterProp, Mode=TwoWay}"
//                                  Height="32"
//                                  FontSize="14"/>
//                    </StackPanel>

//                    <!-- Future filter placeholder -->
//                    <!--<StackPanel Orientation="Vertical">
//                        <TextBlock Text="(Other Filter)"
//                                   FontSize="14"
//                                   FontWeight="SemiBold"
//                                   Margin="0,0,0,4"/>
//                        <ComboBox Height="32"
//                                  FontSize="14"
//                                  IsEnabled="False" SelectionChanged="ComboBox_SelectionChanged"
//                               />
//                    </StackPanel>-->
//                </UniformGrid>
//            </StackPanel>

//            <!-- ListView Section -->
//            <ListView ItemsSource="{Binding Path=OpenCallList}"
//                      Margin="20,0,20,20"
//                      BorderThickness="1"
//                      BorderBrush="#e1e6f2"
//                      Background="White" Width="827" SelectionChanged="ListView_SelectionChanged">
//                <ListView.View>
//                    <GridView AllowsColumnReorder="True">
//                        <GridViewColumn Header="ID" Width="60">
//                            <GridViewColumn.CellTemplate>
//                                <DataTemplate>
//                                    <TextBlock Text="{Binding Id}" />
//                                </DataTemplate>
//                            </GridViewColumn.CellTemplate>
//                        </GridViewColumn>

//                        <GridViewColumn Header="Type of Call" Width="120">
//                            <GridViewColumn.CellTemplate>
//                                <DataTemplate>
//                                    <TextBlock Text="{Binding TypeOfCall}" />
//                                </DataTemplate>
//                            </GridViewColumn.CellTemplate>
//                        </GridViewColumn>

//                        <GridViewColumn Header="Description Of Call" Width="120">
//                            <GridViewColumn.CellTemplate>
//                                <DataTemplate>
//                                    <TextBlock Text="{Binding DescriptionOfCall}" />
//                                </DataTemplate>
//                            </GridViewColumn.CellTemplate>
//                        </GridViewColumn>

//                        <GridViewColumn Header="Address" Width="150">
//                            <GridViewColumn.CellTemplate>
//                                <DataTemplate>
//                                    <TextBlock Text="{Binding Address}" />
//                                </DataTemplate>
//                            </GridViewColumn.CellTemplate>
//                        </GridViewColumn>

//                        <GridViewColumn Header="Opened" Width="130">
//                            <GridViewColumn.CellTemplate>
//                                <DataTemplate>
//                                    <TextBlock Text="{Binding TimeOfOpeningCall}" />
//                                </DataTemplate>
//                            </GridViewColumn.CellTemplate>
//                        </GridViewColumn>

//                        <GridViewColumn Header="Ended" Width="130">
//                            <GridViewColumn.CellTemplate>
//                                <DataTemplate>
//                                    <TextBlock Text="{Binding MaxTimeOfFinishCall}" />
//                                </DataTemplate>
//                            </GridViewColumn.CellTemplate>
//                        </GridViewColumn>

//                        <GridViewColumn Header="DistanceFromTheVolunteer" Width="130">
//                            <GridViewColumn.CellTemplate>
//                                <DataTemplate>
//                                    <TextBlock Text="{Binding DistanceFromTheVolunteer}" />
//                                </DataTemplate>
//                            </GridViewColumn.CellTemplate>
//                        </GridViewColumn>
//                    </GridView>
//                </ListView.View>
//            </ListView>
//            <!--<ListView ItemsSource="{Binding OpenCallList}"
//          SelectedItem="{Binding SelectedCall, Mode=TwoWay}"
//          SelectionChanged="ListView_SelectionChanged"
//          MouseDoubleClick="lsvVolunteerList_MouseDoubleClick"
//          Margin="20,0,20,20"
//          BorderThickness="1"
//          BorderBrush="#e1e6f2"
//          Background="White"
//          Width="827">
//                <ListView.View>
//                    <GridView AllowsColumnReorder="True">
//                        <GridViewColumn Header="ID" Width="60">
//                            <GridViewColumn.CellTemplate>
//                                <DataTemplate>
//                                    <TextBlock Text="{Binding Id}" />
//                                </DataTemplate>
//                            </GridViewColumn.CellTemplate>
//                        </GridViewColumn>

//                        <GridViewColumn Header="Type of Call" Width="120">
//                            <GridViewColumn.CellTemplate>
//                                <DataTemplate>
//                                    <TextBlock Text="{Binding TypeOfCall}" />
//                                </DataTemplate>
//                            </GridViewColumn.CellTemplate>
//                        </GridViewColumn>

//                        <GridViewColumn Header="Description Of Call" Width="120">
//                            <GridViewColumn.CellTemplate>
//                                <DataTemplate>
//                                    <TextBlock Text="{Binding DescriptionOfCall}" />
//                                </DataTemplate>
//                            </GridViewColumn.CellTemplate>
//                        </GridViewColumn>

//                        <GridViewColumn Header="Address" Width="150">
//                            <GridViewColumn.CellTemplate>
//                                <DataTemplate>
//                                    <TextBlock Text="{Binding Address}" />
//                                </DataTemplate>
//                            </GridViewColumn.CellTemplate>
//                        </GridViewColumn>

//                        <GridViewColumn Header="Opened" Width="130">
//                            <GridViewColumn.CellTemplate>
//                                <DataTemplate>
//                                    <TextBlock Text="{Binding TimeOfOpeningCall}" />
//                                </DataTemplate>
//                            </GridViewColumn.CellTemplate>
//                        </GridViewColumn>

//                        <GridViewColumn Header="Ended" Width="130">
//                            <GridViewColumn.CellTemplate>
//                                <DataTemplate>
//                                    <TextBlock Text="{Binding MaxTimeOfFinishCall}" />
//                                </DataTemplate>
//                            </GridViewColumn.CellTemplate>
//                        </GridViewColumn>

//                        <GridViewColumn Header="Distance" Width="130">
//                            <GridViewColumn.CellTemplate>
//                                <DataTemplate>
//                                    <TextBlock Text="{Binding DistanceFromTheVolunteer}" />
//                                </DataTemplate>
//                            </GridViewColumn.CellTemplate>
//                        </GridViewColumn>
//                    </GridView>
//                </ListView.View>
//            </ListView>-->

//        </DockPanel>
//    </Border>
//</Window>
