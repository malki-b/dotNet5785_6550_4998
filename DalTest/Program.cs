using Dal;
using DalApi;
using DO;
using System.Data;
using System.Net;
using System.Numerics;
using System.Xml.Linq;

namespace DalTest
{
    internal class Program
    {

        private static IVolunteer? s_dalVolunteer = new VolunteerImplementation(); //stage 1
        private static ICall? s_dalCall = new CallImplementation(); //stage 1
        private static IAssignment? s_dalAssignment = new AssignmentImplementation(); //stage 1
        private static IConfig? s_dalConfig = new ConfigImplementation(); //stage 1
        enum MainMenu
        {
            Exit, DisplayVolunteer, DisplayAssignments, DisplayCalls, DisplayConfig, InitializeData, ResetDatabase, DisplayAllData
        }
        enum Crud
        {
            Exit, Create, Read, ReadAll, Update, Delete, DeleteAll
        }

        public static void displayMainMenu()
        {
            foreach (MainMenu choice in Enum.GetValues(typeof(MainMenu)))
            {
                Console.WriteLine($"`press{(int)choice} to {choice}`");
            };

        }

        public static void DisplayCrud()
        {
            foreach (Crud choice in Enum.GetValues(typeof(Crud)))
            {
                Console.WriteLine($"`press{(int)choice} to {choice}`");
            };

        }
        public static void createVolunteer()
        {
            Console.WriteLine("Enter Id:");
            int Id = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter Name:");
            string Name = Console.ReadLine()!;

            Console.WriteLine("Enter Phone:");
            string Phone = Console.ReadLine()!;

            Console.WriteLine("Enter Email:");
            string Email = Console.ReadLine()!;

            Console.WriteLine("Enter Password:");
            string Password = Console.ReadLine()!;
            
            Console.WriteLine("Enter Address (press Enter to skip):");
            string inputAddress = Console.ReadLine()!;
            if (!string.IsNullOrEmpty(inputAddress))
            {
                string Address = inputAddress;
            }

            Console.WriteLine("Enter Latitude (press Enter to skip):");
            string inputLatitude = Console.ReadLine()!;
            if (!string.IsNullOrEmpty(inputLatitude))
            {
                double Latitude = Convert.ToDouble(inputLatitude);
            }

            Console.WriteLine("Enter Longitude (press Enter to skip):");
            string inputLongitude = Console.ReadLine()!;
            if (!string.IsNullOrEmpty(inputLongitude))
            {
                double Longitude = Convert.ToDouble(inputLongitude);
            }

            Console.WriteLine("Enter Role (Volunteer, Coordinator, Admin - default is Volunteer):");
            string inputRole = Console.ReadLine()!;
            if (!string.IsNullOrEmpty(inputRole))
            {
                Enum.TryParse(inputRole, out Role role);
                //Role = role;
            }

            Console.WriteLine("Is User Active? (true/false - press Enter to skip):");
            string inputIsActive = Console.ReadLine()!;
            if (!string.IsNullOrEmpty(inputIsActive))
            {
                bool IsActive = Convert.ToBoolean(inputIsActive);
            }

            Console.WriteLine("Enter Max Distance (press Enter to skip):");
            string inputMaxDistance = Console.ReadLine()!;
            if (!string.IsNullOrEmpty(inputMaxDistance))
            {
                double Max_Distance = Convert.ToDouble(inputMaxDistance);
            }

            Console.WriteLine("Enter Type of Distance (Air, Road - default is Air):");
            string inputTypeDistance = Console.ReadLine()!;
            if (!string.IsNullOrEmpty(inputTypeDistance))
            {
                Enum.TryParse(inputTypeDistance, out TypeDistance typeDistance);
                TypeDistance Type_Distance = typeDistance;
            }
            s_dalVolunteer!.Create(new Volunteer(Address, Latitude, Longitude, role, IsActive, Max_Distance, Type_Distance));

        }
        public static void DisplayVolunteer()
        {
            DisplayCrud();
            Console.ReadLine();

            if (Enum.TryParse(Console.ReadLine(), out Crud choice))
            {
                switch (choice)
                {

                    case Crud.Exit:
                        return;
                    case Crud.Create:
                        createVolunteer();
                        
                        break;
                    case Crud.Read:
                        s_dalVolunteer!.Read(Console.ReadLine());
                        break;
                    case Crud.ReadAll:
                        s_dalVolunteer!.ReadAll();
                        break;
                    case Crud.Update:

                        break;
                    case Crud.Delete:

                        break;
                    case Crud.DeleteAll:
                        s_dalVolunteer!.DeleteAll();

                        break;
                    default:
                        break;
                }
            }
        }
        public static void DisplayAssignments()
        {
            DisplayCrud();
            Console.ReadLine();
            if (Enum.TryParse(Console.ReadLine(), out Crud choice))
            {
                switch (choice)
                {
                    case Crud.Exit:
                        break;
                    case Crud.Create:
                        break;
                    case Crud.Read:
                        break;
                    case Crud.ReadAll:
                        break;
                    case Crud.Update:
                        break;
                    case Crud.Delete:
                        break;
                    case Crud.DeleteAll:
                        break;
                    default:
                        break;
                }
            }
        }
            public static void DisplayCalls()
        {
            DisplayCrud();
            Console.ReadLine();
            if (Enum.TryParse(Console.ReadLine(), out Crud choice))
            {
                switch (choice)
                {
                    case Crud.Exit:

                        break;
                    case Crud.Create:
                        break;
                    case Crud.Read:
                        break;
                    case Crud.ReadAll:
                        break;
                    case Crud.Update:
                        break;
                    case Crud.Delete:
                        break;
                    case Crud.DeleteAll:
                        break;
                    default:
                        break;
                }
            }
        }
        public static void DisplayConfig()
        {

        }
        public static void InitializeData()
        {

        }
        public static void ResetDatabase()
        {

        }
        public static void DisplayAllData()
        {

        }
        static void Main(string[] args)
        {
            try
            {
                displayMainMenu();
                Console.ReadLine();
                if (Enum.TryParse(Console.ReadLine(), out MainMenu choice))
                {
                    switch (choice)
                    {
                        case MainMenu.Exit:
                            return;
                        case MainMenu.DisplayVolunteer:
                            DisplayVolunteer();
                            break;
                        case MainMenu.DisplayAssignments:
                            DisplayAssignments();
                            break;
                        case MainMenu.DisplayCalls:
                            DisplayCalls();
                            break;
                        case MainMenu.DisplayConfig:
                            DisplayConfig();
                            break;
                        case MainMenu.InitializeData:
                            InitializeData();
                            break;
                        case MainMenu.ResetDatabase:
                            ResetDatabase();
                            break;
                        case MainMenu.DisplayAllData:
                            DisplayAllData();
                            break;

                    };
                };

            catch { }
        }
    }

}




