using DalApi;

internal class Program
{

    //static readonly IDal s_dal = new DalList(); //stage 2
    //static readonly IDal s_dal = new DalXml(); //stage 3
    static readonly IDal s_dal = Factory.Get; //stage 4

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

    public static BO.Volunteer CreateVolunteer()
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
        BO.Role Role = (BO.Role)Enum.Parse(typeof(BO.Role), Console.ReadLine()!, true);

        Console.WriteLine("Is User Active? (true/false - press Enter to skip):");
        bool IsActive = bool.Parse(Console.ReadLine()!);

        Console.WriteLine("Enter Max Distance :");
        double MaxDistance = double.Parse(Console.ReadLine()!); ;

        Console.WriteLine("Enter Type of Distance (Air,Walking, Road - default is Air):");
        BO.TypeDistance TypeDistance = (BO.TypeDistance)Enum.Parse(typeof(BO.TypeDistance), Console.ReadLine()!);
        return new BO.Volunteer(Id, Name, Phone, Email, Password, TypeDistance, Role, Address, Latitude, Longitude, IsActive, MaxDistance);
        //s_dal.Volunteer!.Create(new Volunteer(Id, Name, Phone, Email, Password, Address, Latitude, Longitude, Role, IsActive, MaxDistance, TypeDistance));

    }
}