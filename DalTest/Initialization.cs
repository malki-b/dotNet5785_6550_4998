namespace DalTest;
using DalApi;
using DO;
using System.Data;
using System.Data.Common;
using System.Net.NetworkInformation;
using System.Text.Json;

/// <summary>
/// Updating the Initialization class to work with one interface in the 
/// data layer rather than 4 separate interfaces.
/// </summary>
public static class Initialization
{
    private static IDal? s_dal; //stage 1

    private static readonly Random s_rand = new();

    private static void createVolunteers()
    {
        string[] namesVolunteer = ["Yaakov Levi", "Moshe Cohen", "Avraham Goldstein", "David Friedman", "Shlomo Rabinowitz", "Menachem Green", "Aryeh Schwartz", "Shimon Baruch", "Yisrael Weiner", "Aharon Fried", "Shlomo Fisher", "Daniel Grossman", "Avraham Super", "Shmuel Gross", "Eliyahu Braun"];
        string[] phoneVolunteer = ["0521234567", "0549876543", "0506543298", "0525432187", "0589876543", "0524321987", "0583219876", "0551987654", "0534567892", "0587654321", "0509876543", "0541234567", "0576543219", "0509876543", "0565432198"];
        string[] emailVolunteer = ["yaakovlevi@gmail.com", "moshecohen@gmail.com", "avrahamgoldstein@gmail.com", "davidfriedman@gmail.com", "shlomorabinowitz@gmail.com", "menachemgreen@gmail.com", "aryehschwartz@gmail.com", "shimonbaruch@gmail.com", "yisraelweiner@gmail.com", "aharonfried@gmail.com", "shlomofisher@gmail.com", "danielgrossman@gmail.com", "avrahamsuper@gmail.com", "shmuelgross@gmail.com", "eliyahubraun@gmail.com"];
        string[] passVolunteer = new string[]
        {
    "Passw0rd!",
    "SecurePass123!",
    "Gmail1234!",
    "Password123!",
    "SecretPass!",
    "Green2021!",
    "AryehS!23!",
    "Baruch2021!",
    "WeinerPass!",
    "Aharon12345!",
    "Fish1234!3456",
    "GrossPass!!",
    "SuperPass2021!",
    "ShmuelPass!",
    "Braun2021!"
        };
        string[] addressVolunteer = [
   "5 Hamerkaz Street, Haifa City Center, Haifa, Israel",
"12 Rothschild Boulevard, Tel Aviv, Israel",
"10 King David Street, Jerusalem City Center, Jerusalem, Israel",
"25 Herzl Street, Rehovot, Israel",
"18 Bialik Street, Ramat Gan, Israel",
"9 Ben Yehuda Street, Tel Aviv, Israel",
"30 Hanevi'im Street, Jerusalem, Israel",
"15 Hahistadrut Avenue, Haifa, Israel",
"7 Dizengoff Street, Tel Aviv, Israel",
"20 Menachem Begin Road, Tel Aviv, Israel",
"22 Jabotinsky Street, Ramat Gan, Israel",
"14 Weizmann Street, Herzliya, Israel",
"3 Shderot Ben Gurion, Haifa, Israel",
"8 Agron Street, Jerusalem, Israel",
"6 Zamenhoff Street, Tel Aviv, Israel"
];
        //        double[] Longitude = [ 32.093035,32.072976, 32.075237,
        //    32.074741,
        //    32.069773,
        //    32.820103,
        //    32.820918,
        //    32.820536,
        //    32.977347,
        //    32.175679,
        //    32.072830,
        //    32.175537,
        //    32.070820,
        //    32.070131,
        //    32.074038
        //];
        //        double[] Latitude = [
        //    34.770410,
        //    34.773367,
        //    34.775382,
        //    34.778712,
        //    34.774574,
        //    34.983579,
        //    34.987190,
        //    34.988004,
        //    34.742663,
        //    34.912945,
        //    34.794960,
        //    34.912181,
        //    34.799589,
        //    34.801429,
        //    34.790317
        //];
        //       int[] volunteerIds =
        //[
        //               328306550,
        //    329333488,
        //   214491060,
        //   219595907,
        //   215281288,
        //   218445062,
        //   332987908,
        //   214388381,
        //   214875379,
        //   330986704,
        //   214324998
        //];
        int[] volunteerIds =
       {
    328306550, 329333488, 214491060, 219595907, 215281288,
    218445062, 332987908, 214388381, 214875379, 330986704,
    214324998, 327786612, 327880845, 329236087, 327796678
};

        int MIN_ID = 200000000;
        int MAX_ID = 400000000;
        for (int i = 0; i < 15; i++)
        {
            //int id;
            //do
            //    id = s_rand.Next(MIN_ID, MAX_ID);
            //while (s_dal!.Volunteer.Read(id) != null);

            Role r;
            if (i == 0)
            {
                r = Role.Manager;
            }
            else
            {
                r = Role.Volunteer;
            }

            DateTime start = new DateTime(1995, 1, 1);

            var values = Enum.GetValues(typeof(TypeDistance));
            int randomIndex = s_rand.Next(0, values.Length);
            TypeDistance distanceType = (TypeDistance)values.GetValue(randomIndex)!;
            var (latitude, longitude) =GetCoordinates(addressVolunteer[i]);
            Volunteer v = new(volunteerIds[i], namesVolunteer[i], phoneVolunteer[i], emailVolunteer[i], passVolunteer[i], distanceType, r, addressVolunteer[i], latitude, longitude, true, s_rand.Next(0, 20));
            s_dal!.Volunteer.Create(v);
        }
    }

    private static void createCall()
    {
        DateTime start = new DateTime(s_dal!.Config.Clock.Year, s_dal.Config.Clock.Month, s_dal.Config.Clock.Day, s_dal!.Config.Clock.Hour - 2, s_dal!.Config.Clock.Minute, 1);
        // DateTime start = new DateTime(s_dal.Config.Clock.Year, s_dal.Config.Clock.Hour - 2, 1); //stage 1
        // DateTime start = new DateTime(2024, 12, 1); //stage 1

        int range = (s_dal.Config.Clock - start).Days; //stage 1
        start.AddDays(s_rand.Next(range));

        string[] callAddresses =
           [
            "Meah Shearim St 10, Jerusalem", "Chazon Ish St 6, Jerusalem", "Ramat Eshkol St 11, Jerusalem",
            "Har Safra St 1, Jerusalem", "Mount Scopus St 4, Jerusalem", "Keren Hayesod St 30, Jerusalem",
            "Neve Yaakov St 17, Jerusalem", "Shmuel HaNavi St 12, Jerusalem", "Yechiel St 3, Jerusalem",
            "Rav Kook St 4, Jerusalem", "Talmud Torah St 8, Jerusalem", "Sanhedria St 18, Jerusalem",
            "Kiryat Moshe St 6, Jerusalem", "Achad Ha'am St 2, Jerusalem", "Bar Ilan St 7, Jerusalem",
            "City Center St 14, Jerusalem", "Rechov Yechiel 3, Jerusalem", "Giv'at Shaul St 7, Jerusalem",
            "Nachlaot St 7, Jerusalem", "Rav Kook St 5, Jerusalem", "Har Nof St 18, Jerusalem",
            "Ramat Shlomo St 15, Jerusalem", "Sderot Yitzhak Rabin St 5, Jerusalem", "Har Hatzofim St 8, Jerusalem",
            "Giv'at HaMivtar St 6, Jerusalem", "Tefilat Yisrael St 14, Jerusalem", "Malkhei Yisrael St 10, Jerusalem",
            "Kiryat Tzahal St 6, Jerusalem", "Nachal Noach St 17, Jerusalem", "Maalot Dafna St 6, Jerusalem",
            "Har HaMor St 3, Jerusalem", "Ramat HaSharon St 2, Jerusalem", "Yakar St 3, Jerusalem",
            "Rav Haim Ozer St 9, Jerusalem", "Yehoshua Ben-Nun St 5, Jerusalem", "Meir Schauer St 12, Jerusalem",
            "Menachem Begin St 11, Jerusalem", "Yisrael Yaakov St 13, Jerusalem", "Ben Yehuda St 6, Jerusalem",
              "Kiryat Tzahal St 61, Jerusalem", "Nachal Noach St 11, Jerusalem", "Maalot Dafna St 16, Jerusalem",
            "Har HaMor St 31, Jerusalem", "Ramat HaSharon St 21, Jerusalem", "Yakar St 31, Jerusalem",
            "Rav Haim Ozer St 1, Jerusalem", "Yehoshua Ben-Nun St 2, Jerusalem", "Meir Schauer St 1, Jerusalem",
            "Menachem Begin St 12, Jerusalem", "Yisrael Yaakov St 12, Jerusalem"
          ];
        double[] callLongitudes =
       [
            35.225721, 35.217133, 35.229169, 35.230535, 35.225939,
            35.224211, 35.219538, 35.224968, 35.226063, 35.219375,
            35.213736, 35.217712, 35.229053, 35.217509, 35.220429,
            35.222809, 35.222797, 35.226436, 35.221255, 35.220655,
            35.229191, 35.222992, 35.227074, 35.221162, 35.227591,
            35.225712, 35.220829, 35.223016, 35.219865, 35.230012,
            35.220076, 35.221336, 35.228300, 35.221133, 35.224713,
            35.227271, 35.219754, 35.226358, 35.225099, 35.228086,
            35.228418, 35.222438, 35.221694, 35.223145, 35.221228,
            35.222590, 35.222579, 35.222869, 35.226072, 35.221711
       ];
        double[] callLatitudes =
       [
            31.776545, 31.771675, 31.767727, 31.771267, 31.768520,
            31.785228, 31.786335, 31.769799, 31.773315, 31.786812,
            31.776216, 31.773144, 31.764577, 31.767558, 31.774280,
            31.782129, 31.784256, 31.779211, 31.783858, 31.783022,
            31.774607, 31.773122, 31.782645, 31.783712, 31.773770,
            31.779614, 31.767658, 31.785070, 31.778488, 31.766734,
            31.780314, 31.783537, 31.775809, 31.773657, 31.781039,
            31.779433, 31.771505, 31.770824, 31.774722, 31.776229,
            31.773940, 31.777524, 31.774912, 31.770963, 31.777611,
            31.776597, 31.785040, 31.772628, 31.776763, 31.780179
        ];
        string[] cases = [
      "Serious road accident",
    "Suspected heart attack",
    "Breathing cessation",
    "High fever",
    "Humanitarian crisis",
    "Fear of primary injury",
    "Bone injury",
    "Railway incident",
    "Fear of falling",
    "Fear of crashing",
    "Tooth injury",
    "Fear of fall injury",
    "Eye injury",
    "Fear of bone injury",
    "Fear of collapse",
    "Fear of accident",
    "Fear of neck injury",
    "Fear of back injury",
    "Burn",
    "Head injury",
    "Fear of abduction",
    "Facial injury",
    "Joint injury",
    "Fear of muscle rupture",
    "Fear of stumble",
    "Ear injury",
    "Chin injury",
    "Fear of surgery",
    "Shoulder injury",
    "Hand injury",
    "Fear of shoulder dislocation",
    "Fear of disorder",
    "Abdominal injury",
    "Fear of collapse",
    "Fear of pain",
    "Leg injury",
    "Ankle injury",
    "Fear of tear",
    "Fear of slip",
    "Car accident",
    "Fear of outbreak",
    "Fear of fall",
    "Fear of injury",
    "Foot injury",
    "Fear of collapse",
    "Hip injury",
    "Abdominal injury",
    "Fear of groin injury",
    "Fear of neck injury",
     "Fear of groins injury",
    "Fear of necks injury"
  ];

        for (int i = 0; i < 50; i++)
        {
            TypeOfReading type;
            DateTime ending;
            if (i < 15)
            {
                type = TypeOfReading.FearOfHumanLife;
                ending = start.AddMinutes(15);
            }
            else if (i < 30)
            {
                type = TypeOfReading.ImmediateDanger;
                ending = start.AddMinutes(30);
            }
            else
            {
                type = TypeOfReading.LongTermDanger;
                ending = start.AddMinutes(60);
            }
            var (latitude, longitude) = GetCoordinates(callAddresses[i]);

            s_dal!.Call.Create(new Call(callAddresses[i], latitude, longitude, start, type, cases[i], ending));
        }
    }

    //private static void createAssignment()
    //{
    //    List<Volunteer>? volunteers = s_dal!.Volunteer.ReadAll().ToList();
    //    List<Call>? calls = s_dal!.Call.ReadAll().ToList();
    //    for (int i = 15; i < 50; i++)
    //    {
    //        int volunteerId = volunteers[s_rand.Next(volunteers.Count)].Id;
    //        int callId = calls[s_rand.Next(calls.Count)].Id;
    //        DateTime openingCase = s_dal!.Config.Clock;
    //        DateTime endingCase = (DateTime)calls[i].MaxTimeFinishRead!;
    //        TimeSpan rangeOfTime = endingCase - openingCase;
    //        int validDifference = (int)Math.Max(rangeOfTime.TotalMinutes, 0);
    //        int randomHour = s_rand.Next(0, validDifference);
    //        DateTime time = openingCase.AddMinutes(randomHour);
    //        s_dal!.Assignment.Create(new Assignment(callId, volunteerId, openingCase, time, (TypeOfEnding)s_rand.Next(Enum.GetValues(typeof(TypeOfEnding)).Length - 1)));
    //    }
    //}
    private static void createAssignment()
    {
        var volunteers = s_dal!.Volunteer.ReadAll().ToList();
        var calls = s_dal!.Call.ReadAll().ToList();

        if (volunteers.Count == 0 || calls.Count < 40)
        {
            Console.WriteLine("Insufficient volunteers or calls.");
            return;
        }

        for (int i = 15; i < 50; i++)
        {
            try
            {
                var call = calls[i];

                if (call.MaxTimeFinishRead <= call.OpeningTime)
                {
                    Console.WriteLine($"Call {call.Id} has invalid time range.");
                    continue;
                }

                var volunteer = volunteers[s_rand.Next(volunteers.Count)];

                DateTime opening = call.OpeningTime;
                DateTime ending = call.MaxTimeFinishRead!.Value;
                int minutesRange = (int)(ending - opening).TotalMinutes;

                if (minutesRange <= 0) continue;
                TypeOfEnding endType = (TypeOfEnding)s_rand.Next(Enum.GetValues(typeof(TypeOfEnding)).Length);
                DateTime? endTime = endType == TypeOfEnding.None ? null : opening.AddMinutes(s_rand.Next(minutesRange));

                var assignment = new Assignment(
                    call.Id,
                    volunteer.Id,
                    opening,
                    endTime,
                    endType
                );

                s_dal.Assignment.Create(assignment);
                Console.WriteLine($"✅ Assigned call {call.Id} to volunteer {volunteer.Id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error in call index {i}: {ex.Message}");
            }
        }
    }


    //public static void Do(IDal dal) //stage 2
    public static void Do() //stage 4
    {
        //s_dalVolunteer = dalVolunteer ?? throw new NullReferenceException("DAL object can not be null!"); //stage 1
        //s_dalCall = dalCall ?? throw new NullReferenceException("DAL object can not be null!"); //stage 1
        //s_dalAssignment = dalAssignment ?? throw new NullReferenceException("DAL object can not be null!"); //stage 1
        //s_dalConfig = dalConfig ?? throw new NullReferenceException("DAL object can not be null!"); //stage 1

        //s_dal = dal ?? throw new NullReferenceException("DAL object can not be null!"); //stage 2
        s_dal = DalApi.Factory.Get; //stage 4

        Console.WriteLine("Reset Configuration values and List values...");
        //s_dalConfig.Reset(); //stage 1
        //s_dalVolunteer.DeleteAll(); //stage 1
        //s_dalCall.DeleteAll(); //stage 1
        //s_dalAssignment.DeleteAll(); //stage 1
        s_dal.ResetDB();

        Console.WriteLine("Initializing Volunteer list ...");
        createVolunteers();

        Console.WriteLine("Initializing Call list ...");
        createCall();

        Console.WriteLine("Initializing IAssignment list ...");
        createAssignment();
    }
    private static string apiKey = "PK.83B935C225DF7E2F9B1ee90A6B46AD86";

    public static (double, double) GetCoordinates(string address)
    {
        using var client = new HttpClient();
        string url = $"https://us1.locationiq.com/v1/search.php?key={apiKey}&q={Uri.EscapeDataString(address)}&format=json";

        var response = client.GetAsync(url).GetAwaiter().GetResult();
        //if (!response.IsSuccessStatusCode)
        //    throw new Exception("Invalid address or API error.");

        var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        using var doc = JsonDocument.Parse(json);

        if (doc.RootElement.ValueKind != JsonValueKind.Array || doc.RootElement.GetArrayLength() == 0)
            throw new Exception("Address not found.");

        var root = doc.RootElement[0];

        if (!root.TryGetProperty("lat", out var latProperty) ||
            !root.TryGetProperty("lon", out var lonProperty))
        {
            throw new Exception("Missing latitude or longitude in response.");
        }

        if (!double.TryParse(latProperty.GetString(), out double latitude) ||
            !double.TryParse(lonProperty.GetString(), out double longitude))
        {
            throw new Exception("Invalid latitude or longitude format.");
        }

        return (latitude, longitude);
    }

}
