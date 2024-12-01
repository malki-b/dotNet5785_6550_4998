using Dal;
using DalApi;
using DO;
using System.ComponentModel.Design;
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
        //private static IVolunteer? s_dalVolunteer = new VolunteerImplementation();
        //private static ICall? s_dalCall = new CallImplementation();
        //private static IAssignment? s_dalAssignment = new AssignmentImplementation();
        //private static IConfig? s_dalConfig = new ConfigImplementation();
        static readonly IDal s_dal = new DalList(); //stage 2


        enum MainMenu
        {
            Exit, DisplayVolunteer, DisplayAssignments, DisplayCalls, DisplayConfig, InitializeData, ResetDatabase, DisplayAllData
        }
        enum Crud
        {
            Exit, Create, Read, ReadAll, Update, Delete, DeleteAll
        }

        enum Config
        {
            Exit, AddClockMinute, AddClockHour, AddClockByDay, AddClockByMonth, AddClockByYear, ShowCurrentClock, ChangeClock, ShowCurrentRiskRange, ResetConfig
        }

        public static void DisplayMainMenu()
        {
            foreach (MainMenu choice in Enum.GetValues(typeof(MainMenu)))
            {
                Console.WriteLine($"press {(int)choice} to {choice}");
            };

        }

        public static void DisplayCrud()
        {
            foreach (Crud choice in Enum.GetValues(typeof(Crud)))
            {
                Console.WriteLine($"press {(int)choice} to {choice}");
            };
        }

        public static void CreateVolunteer()
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
            string? Address = Console.ReadLine()!;

            Console.WriteLine("Enter Latitude:");
            double? Latitude = double.Parse(Console.ReadLine()!);

            Console.WriteLine("Enter Longitude:");
            double? Longitude = double.Parse(Console.ReadLine()!);

            Console.WriteLine("Enter Role (Volunteer, Manager:");
            Role Role = (Role)Enum.Parse(typeof(Role), Console.ReadLine()!, true);

            Console.WriteLine("Is User Active? (true/false - press Enter to skip):");
            bool IsActive = bool.Parse(Console.ReadLine()!);

            Console.WriteLine("Enter Max Distance :");
            double MaxDistance = double.Parse(Console.ReadLine()!); ;

            Console.WriteLine("Enter Type of Distance (Air,Walking, Road - default is Air):");
            TypeDistance TypeDistance = (TypeDistance)Enum.Parse(typeof(TypeDistance), Console.ReadLine()!);
            // return new Volunteer(Id, Name, Phone, Email, Password, Address, Latitude, Longitude, Role, IsActive, MaxDistance, TypeDistance);
            s_dalVolunteer!.Create(new Volunteer(Id, Name, Phone, Email, Password, Address, Latitude, Longitude, Role, IsActive, MaxDistance, TypeDistance));

        }
        public static void UpdateVolunteer()
        {
            Console.WriteLine("Enter Volunteer details:");
            Console.WriteLine("Enter Id:");
            int Id = int.Parse(Console.ReadLine()!);

            Console.WriteLine("Enter Name:");
            string? Name = Console.ReadLine()!;

            Console.WriteLine("Enter Phone:");
            string? Phone = Console.ReadLine()!;

            Console.WriteLine("Enter Email:");
            string? Email = Console.ReadLine()!;

            Console.WriteLine("Enter Password:");
            string? Password = Console.ReadLine()!;

            Console.WriteLine("Enter Address :");
            string? Address = Console.ReadLine()!;

            Console.WriteLine("Enter Latitude:");
            double? Latitude = Convert.ToDouble(Console.ReadLine());

            Console.WriteLine("Enter Longitude:");
            double? Longitude = double.Parse(Console.ReadLine()!); ;

            Console.WriteLine("Enter Role (Volunteer, Manager:");
            Role Role = (Role)Enum.Parse(typeof(Role), Console.ReadLine()!, true);

            Console.WriteLine("Is User Active? (true/false - press Enter to skip):");
            bool? IsActive = bool.Parse(Console.ReadLine()!);

            Console.WriteLine("Enter Max Distance :");
            double? MaxDistance = double.Parse(Console.ReadLine()!); ;

            Console.WriteLine("Enter Type of Distance (Air,Walking, Road - default is Air):");
            TypeDistance TypeDistance = (TypeDistance)Enum.Parse(typeof(TypeDistance), Console.ReadLine()!);
            //return new Volunteer(Id, Name, Phone, Email, Password, Address, Latitude, Longitude, Role, IsActive, MaxDistance, TypeDistance);
            s_dalVolunteer!.Create(new Volunteer(Id, Name, Phone, Email, Password, Address, Latitude, Longitude, Role, IsActive, MaxDistance, TypeDistance));

        }
        public static Assignment CreateAssignment()
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
            return (new Assignment(callId, volunteerId, openingCase, exitTime, finishCallType));
            // s_dalAssignment!.Create(new Assignment(callId, volunteerId, openingCase, exitTime, finishCallType));


        }

        public static Call CreateCall()
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
            string description = Console.ReadLine()!;

            Console.Write("Enter Time (yyyy-MM-dd HH:mm:ss): ");
            DateTime ending = DateTime.Parse(Console.ReadLine()!);

            Console.WriteLine("Enter description  Type (FearOfHumanLife, ImmediateDanger, LongTermDanger):");
            TypeOfReading typeOfReading = (TypeOfReading)Enum.Parse(typeof(TypeOfReading), Console.ReadLine()!);
            return (new Call(address, latitude, longitude, openingCase, description, ending, typeOfReading));
            //s_dalCall!.Create(new Call(address, latitude, longitude, openingCase, description, ending, typeOfReading));


        }

        public static void DisplayVolunteer()
        {
            while (true)
            {
                DisplayCrud();
                if (Enum.TryParse(Console.ReadLine(), out Crud choice))
                {
                    switch (choice)
                    {

                        case Crud.Exit:
                            return;
                        case Crud.Create:
                            CreateVolunteer();
                            break;
                        case Crud.Read:
                            Console.WriteLine("Enter Id:");
                            int Id = int.Parse(Console.ReadLine()!);
                            Console.WriteLine(s_dalVolunteer!.Read(Id));
                            break;
                        case Crud.ReadAll:
                            //Console.WriteLine(s_dalVolunteer!.ReadAll());
                            var volunteers = s_dalVolunteer!.ReadAll();
                            foreach (var volunteer in volunteers)
                            {
                                Console.WriteLine(volunteer);
                            };
                            break;
                        case Crud.Update:
                            UpdateVolunteer();
                            break;
                        case Crud.Delete:
                            Console.WriteLine("Enter Id to delete:");
                            int id = int.Parse(Console.ReadLine()!);
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
                else
                {
                    Console.WriteLine("The number entered is invalid, Enter a new number.");
                }

            }
        }

        public static void DisplayAssignments()
        {
            while (true)
            {
                DisplayCrud();
                if (Enum.TryParse(Console.ReadLine(), out Crud choice))
                {
                    switch (choice)
                    {
                        case Crud.Exit:
                            return;
                        case Crud.Create:
                            s_dalAssignment!.Create(CreateAssignment());
                            break;
                        case Crud.Read:
                            Console.WriteLine("Enter Id:");
                            int Id = int.Parse(Console.ReadLine()!);
                            Console.WriteLine(s_dalAssignment!.Read(Id));
                            break;
                        case Crud.ReadAll:
                            var assignments = s_dalAssignment!.ReadAll();
                            foreach (var assignment in assignments)
                                Console.WriteLine(assignment);
                            break;
                        case Crud.Update:
                            s_dalAssignment?.Update(CreateAssignment());
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

                else
                {
                    Console.WriteLine("The number entered is invalid, Enter a new number.");
                }

            }
        }

        public static void DisplayCalls()
        {
            while (true)
            {
                DisplayCrud();
                if (Enum.TryParse(Console.ReadLine(), out Crud choice))
                {
                    switch (choice)
                    {
                        case Crud.Exit:
                            return;
                        case Crud.Create:
                            s_dalCall!.Create(CreateCall());
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
                            s_dalCall!.Update(CreateCall());
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
                else
                {
                    Console.WriteLine("The number entered is invalid, Enter a new number.");
                }
            }

        }
        public static void DisplayConfig()
        {
            while (true)
            {
                foreach (Config c in Enum.GetValues(typeof(Config)))
                {
                    Console.WriteLine($"press{(int)c} to{c}");
                };

                if (Enum.TryParse(Console.ReadLine(), out Config choice))
                {
                    switch (choice)
                    {
                        case Config.Exit:
                            return;
                        case Config.AddClockMinute:
                            s_dalConfig!.Clock.AddMinutes(1);
                            break;
                        case Config.AddClockHour:
                            s_dalConfig!.Clock.AddHours(1);
                            break;
                        case Config.AddClockByDay:
                            s_dalConfig!.Clock.AddDays(1);
                            break;
                        case Config.AddClockByMonth:
                            s_dalConfig!.Clock.AddMonths(1);
                            break;
                        case Config.AddClockByYear:
                            s_dalConfig!.Clock.AddYears(1);
                            break;
                        case Config.ShowCurrentClock:
                            Console.WriteLine(s_dalConfig!.Clock);
                            break;
                        case Config.ChangeClock:
                            s_dalConfig!.Clock.AddHours(1);//מעבר לשעון קיץ
                            break;
                        case Config.ShowCurrentRiskRange:
                            Console.WriteLine(s_dalConfig!.RiskRange);
                            break;
                        case Config.ResetConfig:
                            s_dalConfig!.Reset();
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("The number entered is invalid, Enter a new number.");
                }
            }
        }
        public static void InitializeData()
        {
            Initialization.Do(s_dalVolunteer, s_dalCall, s_dalAssignment, s_dalConfig);
        }
        public static void ResetDatabase()
        {
            s_dalVolunteer!.DeleteAll();
            s_dalCall!.DeleteAll();
            s_dalAssignment!.DeleteAll();
            s_dalConfig!.Reset();
        }
        public static void DisplayAllData()
        {
            Console.WriteLine(s_dalVolunteer!.ReadAll());
            Console.WriteLine(s_dalCall!.ReadAll());
            Console.WriteLine(s_dalAssignment!.ReadAll());
        }

        static void Main(string[] Args)
        {

            try
            {
                while (true)
                {
                    DisplayMainMenu();
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

                        }
                    }
                    else
                    {
                        Console.WriteLine("The number entered is invalid, Enter a new number.");
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }
    }
}




