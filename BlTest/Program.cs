using BlApi;
using BlImplementation;
using DalApi;

internal class Program
{
    //public void Main()
    //{
    //    s_bl.Call.RequestCallCounts();
    //}
    //static readonly IDal s_dal = new DalList(); //stage 2
    //static readonly IDal s_dal = new DalXml(); //stage 3
    static readonly IDal s_dal = DalApi.Factory.Get; //stage 4
    static readonly IBl s_bl = BlApi.Factory.Get(); //stage 4
    enum MainMenu
    {
        Exit, DisplayVolunteer, DisplayAssignments, DisplayCalls, DisplayConfig, InitializeData, ResetDatabase, DisplayAllData
    }
    //enum Crud
    //{
    //    Exit, Create, Read, ReadAll, Update, Delete, DeleteAll
    //}
    //enum Config
    //{
    //    Exit, AddClockMinute, AddClockHour, AddClockByDay, AddClockByMonth, AddClockByYear, ShowCurrentClock, ChangeClock, ShowCurrentRiskRange, ResetConfig
    //}

    //public static void DisplayMainMenu()
    //{
    //    foreach (MainMenu choice in Enum.GetValues(typeof(MainMenu)))
    //    {
    //        Console.WriteLine($"press {(int)choice} to {choice}");
    //    };

    //}

    //public static void DisplayCrud()
    //{
    //    foreach (Crud choice in Enum.GetValues(typeof(Crud)))
    //    {
    //        Console.WriteLine($"press {(int)choice} to {choice}");
    //    };
    //}
    static void Main(string[] args)
    {
        Console.WriteLine("Welcome ");
        CheckRole();
       
    }
    private static void CheckRole()
    {
        while (true)
        {
            try
            {
                Console.WriteLine("\nLogin Menu:");
                Console.Write("Enter username: ");
                string username = Console.ReadLine() ?? "";
                Console.Write("Enter password: ");
                string password = Console.ReadLine() ?? "";

                // Assuming Login method returns role as BO.Role (Enum)
                BO.Role role = s_bl.Volunteer.Login(username, password);

                switch (role)
                {
                    case BO.Role.Manager:
                        ManagerMainMenu();
                        break;
                    case BO.Role.Volunteer:
                        VolunteerMainMenu(username);
                        break;
                    default:
                        Console.WriteLine("Invalid login credentials. Please try again.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login error: {ex.Message}");
            }
        }
    }

    private static void VolunteerMainMenu(string username)
    {
        throw new NotImplementedException();
    }

    private static void ManagerMainMenu()
    {
        while (true)
        {
            Console.WriteLine("\nManager Main Menu:");
            //Console.WriteLine("1. Call Management Screen");
            //Console.WriteLine("2. Single Call Management Screen");
            Console.WriteLine("3. Add Call Screen");
            Console.WriteLine("4. Volunteer Management Screen");
            Console.WriteLine("5. Single Volunteer Management Screen");
            Console.WriteLine("6. Add Volunteer Screen");
            Console.WriteLine("0. Logout");
            Console.Write("\nEnter your choice: ");

            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Invalid input. Please enter a number.");
                continue;
            }

            //try
            //{
            //    switch (choice)
            //    {
            //        case 1:
            //            CallManagementScreen();
            //            break;
            //        case 2:
            //            SingleCallManagementScreen();
            //            break;
            //        case 3:
            //            AddCallScreen();
            //            break;
            //        case 4:
            //            VolunteerManagementScreen();
            //            break;
            //        case 5:
            //            SingleVolunteerManagementScreen();
            //            break;
            //        case 6:
            //            AddVolunteerScreen();
            //            break;
            //        case 7:
            //            return; // Return to login menu
            //        default:
            //            Console.WriteLine("Invalid choice. Please try again.");
            //            break;
            //    }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}