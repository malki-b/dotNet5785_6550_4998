using System;
using System.Net.Http;
using System.Threading.Tasks;
using DalApi;
using System.Runtime.InteropServices;
using System.Text;
using System.Security.Cryptography;
using BlImplementation;

namespace Helpers;

public static class VolunteerManager
{
    private static IDal s_dal = Factory.Get;

    internal static ObserverManager Observers = new(); //stage 5

    static internal bool CheckValidation(BO.Volunteer v)
    {
        // If you ever add DAL access here, keep the lock.
        lock (AdminManager.BlMutex)
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
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }
    }

    internal static void SimulateVolunteerAssignmentsAndCallHandling()
    {
        Thread.CurrentThread.Name = $"Simulator{Thread.CurrentThread.ManagedThreadId}";

        List<int> updatedVolunteerIds = new();
        List<int> updatedCallIds = new();

        List<DO.Volunteer> activeVolunteers;
        lock (AdminManager.BlMutex)
            activeVolunteers = s_dal.Volunteer.ReadAll(v => v.IsActive == true).ToList();

        foreach (var volunteer in activeVolunteers)
        {
            DO.Assignment? currentAssignment;

            lock (AdminManager.BlMutex)
            {
                currentAssignment = s_dal.Assignment
                    .ReadAll(a => a.VolunteerId == volunteer.Id && a.EndOfTreatmentTime == null)
                    .FirstOrDefault();
            }

            if (currentAssignment == null)
            {
                List<BO.OpenCallInList> openCalls;
                lock (AdminManager.BlMutex)
                    openCalls = new CallImplementation().RequestOpenCallsForSelection(volunteer.Id).ToList();

                if (!openCalls.Any() || Random.Shared.NextDouble() > 0.2) continue;
                if (openCalls.Any())
                {
                    var selectedCall = openCalls[Random.Shared.Next(openCalls.Count)];
                    try
                    {
                        new CallImplementation().SelectCallForTreatment(volunteer.Id, selectedCall.Id);
                        updatedVolunteerIds.Add(volunteer.Id);
                        updatedCallIds.Add(selectedCall.Id);
                    }
                    catch { continue; }
                }
            }
            else
            {
                DO.Call? call;
                lock (AdminManager.BlMutex)
                    call = s_dal.Call.Read(currentAssignment.CallId);

                if (call is null) continue;

                double distance = Tools.DistanceCalculation(volunteer.Address!, call.Address);
                TimeSpan baseTime = TimeSpan.FromMinutes(distance * 2);
                TimeSpan extra = TimeSpan.FromMinutes(Random.Shared.Next(1, 5));
                TimeSpan totalNeeded = baseTime + extra;
                TimeSpan actual = AdminManager.Now - currentAssignment.EntryTimeForTreatment;

                if (actual >= totalNeeded)
                {
                    try
                    {
                        new CallImplementation().UpdateCallCompletion(volunteer.Id, currentAssignment.Id);
                        updatedVolunteerIds.Add(volunteer.Id);
                        updatedCallIds.Add(call.Id);
                    }
                    catch { continue; }
                }
                else if (Random.Shared.NextDouble() < 0.1)
                {
                    try
                    {
                        new CallImplementation().UpdateCallCancellation(volunteer.Id, currentAssignment.Id);
                        updatedVolunteerIds.Add(volunteer.Id);
                        updatedCallIds.Add(call.Id);
                    }
                    catch { continue; }
                }
            }
        }

        foreach (var id in updatedVolunteerIds.Distinct())
            VolunteerManager.Observers.NotifyItemUpdated(id);
        foreach (var id in updatedCallIds.Distinct())
            CallManager.Observers.NotifyItemUpdated(id);
    }
}
