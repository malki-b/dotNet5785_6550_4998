//using DalApi;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using static System.Runtime.InteropServices.JavaScript.JSType;

//namespace Helpers;

//internal static class VolunteerManager
//{
//    private static IDal s_dal = Factory.Get; //stage 4
//    internal static BO.Year GetStudentCurrentYear(DateTime? registrationDate)
//    {
//        BO.Year currYear = (BO.Year)(ClockManager.Now.Year - registrationDate?.Year!);
//        return currYear > BO.Year.None ? BO.Year.None : currYear;
//    }
//    internal static BO.StudentInCourse GetDetailedCourseForStudent(int volunteerId, int callId)
//    {
//        DO.Assignment? doLink = s_dal.Assignment.Read(l => l.VolunteerId == volunteerId && l.CallId == callId)
//            ?? throw new BO.BlDoesNotExistException($"Student with ID={volunteerId} does Not take Course with ID={callId}");
//        DO.Call? doCall = s_dal.Call.Read(callId)
//     ?? throw new BO.BlDoesNotExistException($"Course with ID={callId} does Not exist");

//        return new()
//        {
//            StudentId = volunteerId,
//            Course = new Tuple<int, string, string>(doCall.Id, doCall.CourseNumber, doCall.CourseName),
//            InYear = (BO.Year?)doCall.InYear,
//            InSemester = (BO.SemesterNames?)doCall.InSemester,
//            Grade = doLink.Grade,
//            Credits = doCall.Credits
//        };
//    }

//    internal static void PeriodicStudentsUpdates(DateTime oldClock, DateTime newClock) //stage 4
//    {
//        var list = s_dal.Volunteer.ReadAll().ToList();
//        foreach (var doStudent in list)
//        {
//            //if student study for more than MaxRange years
//            //then student should be automatically updated to 'not active'
//            if (ClockManager.Now.Year - doStudent.RegistrationDate?.Year >= s_dal.Config.MaxRange)
//            {
//                s_dal.Volunteer.Update(doStudent with { IsActive = false });
//            }
//        }
//    }

//}
using System;
using System.Net.Http;
using System.Threading.Tasks;
//using Newtonsoft.Json.Linq;
using DalApi;
using System.Runtime.InteropServices;
using System.Text;
using System.Security.Cryptography;

namespace Helpers;

public static class VolunteerManager
{
    private static IDal s_dal = Factory.Get;

    internal static ObserverManager Observers = new(); //stage 5 

    static internal bool CheckValidation(BO.Volunteer v)
    {
        var trimmedEmail = v.Email.Trim();

        if (trimmedEmail.EndsWith("."))
        {
            return false;
        }
        try
        {
            var addr = new System.Net.Mail.MailAddress(v.Email);
            if (addr.Address != trimmedEmail)
                return false;
        }
        catch
        {
            return false;
        }

        if (!decimal.TryParse(v.Phone, out decimal phoneInt) || v.Phone[0] != '0' || v.Phone.Length != 10)
            return false;
        if (!IsValidIdNumber(v.Id))
            return false;
        if (v.Password is not null && !IsStrongPassword(v.Password))
            return false;
        return true;
    }
    private static bool IsValidIdNumber(int id)
    {
        string idStr = id.ToString();

        if (idStr.Length != 9)
            return false;

        int sum = 0;
        for (int i = 0; i < idStr.Length - 1; i++)
        {
            int digit = int.Parse(idStr[i].ToString());
            if (i % 2 == 1)
            {
                digit *= 2;
                if (digit > 9) digit -= 9;
            }
            sum += digit;
        }

        int lastDigit = int.Parse(idStr[idStr.Length - 1].ToString());
        return (sum + lastDigit) % 10 == 0;
    }

    public static bool IsStrongPassword(string password)
    {
        if (password.Length < 8)
            return false;

        bool hasUpperCase = false;
        bool hasLowerCase = false;
        bool hasDigit = false;
        bool hasSpecialChar = false;

        foreach (char c in password)
        {
            if (char.IsUpper(c)) hasUpperCase = true;
            if (char.IsLower(c)) hasLowerCase = true;
            if (char.IsDigit(c)) hasDigit = true;
            if (!char.IsLetterOrDigit(c)) hasSpecialChar = true;
        }
        return hasUpperCase && hasLowerCase && hasDigit && hasSpecialChar;
    }

    public static string? HashPassword(string? password)
    {
        if (password == null)
            return null;
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(bytes).Replace("-", "").ToLower(); // Convert byte array to hex string
        }
    }

    //internal static async Task<(double? latitude, double? longitude)> GetCoordinatesAsync(string address)
    //{
    //    string url = $"https://nominatim.openstreetmap.org/search?q={Uri.EscapeDataString(address)}&format=json&limit=1";

    //    using (HttpClient client = new HttpClient())
    //    {
    //        var response = await client.GetStringAsync(url);
    //        var jsonArray = JArray.Parse(response);

    //        if (jsonArray.Count > 0)
    //        {
    //            var location = jsonArray[0];
    //            double latitude = (double)location["lat"];
    //            double longitude = (double)location["lon"];
    //            return (latitude, longitude);
    //        }
    //    }

    //    return (null, null);
    //}



    //internal static double CalculateDistance(double? lat1, double? lon1, double? lat2, double? lon2)
    //{
    //    const double EarthRadius = 6371.0;

    //    if (lat1 is null || lon1 is null || lat2 is null || lon2 is null)
    //        return 0;

    //    double lat1Rad = lat1.Value * Math.PI / 180.0;
    //    double lon1Rad = lon1.Value * Math.PI / 180.0;
    //    double lat2Rad = lat2.Value * Math.PI / 180.0;
    //    double lon2Rad = lon2.Value * Math.PI / 180.0;

    //    double dlat = lat2Rad - lat1Rad;
    //    double dlon = lon2Rad - lon1Rad;

    //    double a = Math.Sin(dlat / 2) * Math.Sin(dlat / 2) +
    //               Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
    //               Math.Sin(dlon / 2) * Math.Sin(dlon / 2);
    //    double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

    //    return EarthRadius * c;
    //}


}
