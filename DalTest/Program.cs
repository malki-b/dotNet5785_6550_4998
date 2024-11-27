using Dal;
using DalApi;
using DO;
using System.Data;
using System.Net;
using System.Net.Mail;
using System.Numerics;
using System.Text;
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
            Console.WriteLine("Enter Volunteer details:");
            Console.WriteLine("Enter Id:");
            int Id = int.Parse(Console.ReadLine()!);

            Console.WriteLine("Enter Name:");
            string Name = Console.ReadLine()!;

            Console.WriteLine("Enter Phone:");
            string Phone = Console.ReadLine()!;

            Console.WriteLine("Enter Email:");
            string Email = Console.ReadLine()!;

            Console.WriteLine("Enter Password:");
            string Password = Console.ReadLine()!;

            Console.WriteLine("Enter Address :");
            string Address = Console.ReadLine()!;

            Console.WriteLine("Enter Latitude:");
            double Latitude = double.Parse(Console.ReadLine()!);

            Console.WriteLine("Enter Longitude:");
            double Longitude = double.Parse(Console.ReadLine()!); ;

            Console.WriteLine("Enter Role (Volunteer, Manager:");
            Role Role = (Role)Enum.Parse(typeof(Role), Console.ReadLine()!, true);

            Console.WriteLine("Is User Active? (true/false - press Enter to skip):");
            bool IsActive = bool.Parse(Console.ReadLine()!);

            Console.WriteLine("Enter Max Distance :");
            double MaxDistance = double.Parse(Console.ReadLine()!); ;

            Console.WriteLine("Enter Type of Distance (Air,Walking, Road - default is Air):");
            TypeDistance TypeDistance = (TypeDistance)Enum.Parse(typeof(TypeDistance), Console.ReadLine()!);

            s_dalVolunteer!.Create(new Volunteer(Id, Name, Phone, Email, Password, Address, Latitude, Longitude, Role, IsActive, MaxDistance, TypeDistance));

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
                        Console.WriteLine("Enter Id:");
                        int Id = int.Parse(Console.ReadLine()!);
                        Console.WriteLine(s_dalVolunteer!.Read(Id));
                        break;
                    case Crud.ReadAll:
                        var volunteers = s_dalCall!.ReadAll();
                        foreach (var volunteer in volunteers)
                        {
                            Console.WriteLine(volunteer);
                        }
                        break;
                    case Crud.Update:

                        break;
                    case Crud.Delete:
                        Console.WriteLine("Enter Id to delete:");
                        int id= int.Parse(Console.ReadLine()!);
                        s_dalVolunteer!.Delete(id);
                        Console.WriteLine("Volunteer deleted.");
                        break;
                    case Crud.DeleteAll:
                        s_dalVolunteer!.DeleteAll();
                        break;
                    default:
                        break;
                }
            }
        }
        public static void createAssignment()
        {
            Console.WriteLine("Enter Assignment details:");
          
            Random s_rand = new();
            List<Volunteer>? volunteers = s_dalVolunteer!.ReadAll();
            List<Call>? calls = s_dalCall!.ReadAll();
           
            int callId = calls[s_rand.Next(calls.Count)].Id;
            int volunteerId = volunteers[s_rand.Next(volunteers.Count)].Id;

            Console.Write("Enter Time (yyyy-MM-dd HH:mm:ss): ");
            DateTime openingCase = DateTime.Parse(Console.ReadLine()!);

            Console.Write("Exit Time (yyyy-MM-dd HH:mm:ss): ");
            DateTime exitTime = DateTime.Parse(Console.ReadLine()!);

            Console.Write("Finish Call Type (Teated, SelfCancellation, CancellationHasExpired): ");
            TypeOfEnding finishCallType = (TypeOfEnding)Enum.Parse(typeof(TypeOfEnding), Console.ReadLine()!, true);

            s_dalAssignment!.Create(new Assignment(callId, volunteerId, openingCase, exitTime, finishCallType));

          
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
                        createAssignment();
                        break;
                    case Crud.Read:
                        Console.WriteLine("Enter Id:");
                        int Id = int.Parse(Console.ReadLine()!);
                        Console.WriteLine(s_dalAssignment!.Read(Id));
                        break;
                    case Crud.ReadAll:
                        var assignments = s_dalAssignment!.ReadAll();
                        foreach (var assignment in assignments)
                        {
                            Console.WriteLine(assignment);
                        }
                        break;
                       
                    case Crud.Update:
                        break;
                    case Crud.Delete:
                        Console.WriteLine("Enter Id to delete:");
                        int id = int.Parse(Console.ReadLine()!);
                        s_dalAssignment!.Delete(id);
                        Console.WriteLine("Assignment deleted.");
                        break;
                    case Crud.DeleteAll:
                        s_dalAssignment!.DeleteAll();
                        break;
                    default:
                        break;
                }
            }
        }
        public static void createCall()
        {
            Console.WriteLine("Enter Call details:");
            Console.WriteLine("Enter Address:");
            string address = Console.ReadLine()!;

            Console.WriteLine("Enter Latitude :");
            double latitude = double.Parse(Console.ReadLine()!);

            Console.WriteLine("Enter Longitude :");
            double longitude = double.Parse(Console.ReadLine()!);

            Console.Write("Enter Time (yyyy-MM-dd HH:mm:ss): ");
            DateTime openingCase = DateTime.Parse(Console.ReadLine()!);


            Console.WriteLine("Enter description :");
            string description =Console.ReadLine()!;

            Console.Write("Enter Time (yyyy-MM-dd HH:mm:ss): ");
            DateTime ending = DateTime.Parse(Console.ReadLine()!);

            Console.WriteLine("Enter description  Type (FearOfHumanLife, ImmediateDanger, LongTermDanger):");
            TypeOfReading typeOfReading = (TypeOfReading)Enum.Parse(typeof(TypeOfReading), Console.ReadLine()!);

            s_dalCall!.Create(new Call(address, latitude, longitude, openingCase, description, ending, typeOfReading));
         

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
                        createCall();
                        break;
                    case Crud.Read:
                        Console.WriteLine("Enter Id:");
                        int Id = int.Parse(Console.ReadLine()!);
                        Console.WriteLine(s_dalCall!.Read(Id));
                        break;
                    case Crud.ReadAll:
                        var calls = s_dalCall!.ReadAll();
                        foreach (var call in calls)
                        {
                            Console.WriteLine(call);
                        }
                        break;
                     
                    case Crud.Update:
                        break;
                    case Crud.Delete:
                        Console.WriteLine("Enter Id to delete:");
                        int id = int.Parse(Console.ReadLine()!);
                        s_dalCall!.Delete(id);
                        Console.WriteLine("Call deleted.");
                        break;
                    case Crud.DeleteAll:
                        s_dalCall!.DeleteAll();
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
            }

            catch { }
        }
    }

}




