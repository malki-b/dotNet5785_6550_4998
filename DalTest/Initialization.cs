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
        string [] phoneVolunteer = ["0521234567", "0549876543", "0506543298", "0525432187", "0589876543", "0524321987", "0583219876", "0551987654", "0534567892", "0587654321", "0509876543", "0541234567", "0576543219", "0509876543", "0565432198"]
        string[] emailVolunteer = ["yaakovlevi@gmail.com", "moshecohen@gmail.com", "avrahamgoldstein@gmail.com", "davidfriedman@gmail.com", "shlomorabinowitz@gmail.com", "menachemgreen@gmail.com", "aryehschwartz@gmail.com", "shimonbaruch@gmail.com", "yisraelweiner@gmail.com", "aharonfried@gmail.com", "shlomofisher@gmail.com", "danielgrossman@gmail.com", "avrahamsuper@gmail.com", "shmuelgross@gmail.com", "eliyahubraun@gmail.com"]
        string[] passVolunteer = ["Passw0rd!", "SecurePass123", "Gmail1234", "Password123", "SecretPass", "Green2021", "AryehS!23", "Baruch2021", "WeinerPass", "Aharon12345", "Fish1234", "GrossPass!", "SuperPass2021", "ShmuelPass", "Braun2021"]

        int MIN_ID = 200000000;
         int MAX_ID = 400000000;
        for (int i = 0; i<15; i++)
        {
            int id;
            do
                id = s_rand.Next(MIN_ID, MAX_ID);
            while (s_dalVolunteer!.Read(id) != null);

            //bool? even = (id % 2) == 0 ? true : false;
            //string? alias = even ? name + "ALIAS" : null;
            DateTime start = new DateTime(1995, 1, 1);
            DateTime bdt = start.AddDays(s_rand.Next((s_dalConfig.Clock - start).Days));
            Volunteer v = new(id, namesVolunteer[i], phoneVolunteer[i], emailVolunteer[i], passVolunteer[i], address, latitude, longitude);
            s_dalVolunteer!.Create(v);
        }
    }
}