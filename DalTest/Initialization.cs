namespace DalTest;
using DalApi;
using DO;

public static class Initialization
{
    private static IVolunteer? s_dalVolunteer; //stage 1
    private static ICall? s_dalCall; //stage 1
    private static IAssignment? s_dalAssignment; //stage 1
    private static IConfig? s_dalConfig; //stage 1
    private static readonly Random s_rand = new();





    private static void createVolunteers()
    {

        string[] namesVolunteer = ["Yaakov Levi", "Moshe Cohen", "Avraham Goldstein", "David Friedman", "Shlomo Rabinowitz", "Menachem Green", "Aryeh Schwartz", "Shimon Baruch", "Yisrael Weiner", "Aharon Fried", "Shlomo Fisher", "Daniel Grossman", "Avraham Super", "Shmuel Gross", "Eliyahu Braun"]
         int MIN_ID = 1000000000;
         int MAX_ID = 999999999;
        foreach (var name in namesVolunteer)
        {
            int id;
            do
                id = s_rand.Next(MIN_ID, MAX_ID);
            while (s_dalVolunteer!.Read(id) != null);

            bool? even = (id % 2) == 0 ? true : false;
            string? alias = even ? name + "ALIAS" : null;
            DateTime start = new DateTime(1995, 1, 1);
            DateTime bdt = start.AddDays(s_rand.Next((s_dalConfig.Clock - start).Days));

            s_dalVolunteer!.Create(new(id, name, alias, even, bdt));
        }
    }

}


