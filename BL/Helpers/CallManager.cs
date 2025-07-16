using BO;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Helpers;

internal static class CallManager
{
    private static IDal s_dal = Factory.Get; //stage 4

    internal static ObserverManager Observers = new(); //stage 5

    //internal static Status GetCallStatus(int callId)
    //{
    //    DO.Call call;
    //    List<Assignment> assignments;
    //    TimeSpan riskRange;
    //    lock (AdminManager.BlMutex)
    //    {
    //        call = s_dal.Call.Read(callId) ??
    //            throw new BO.BlDoesNotExistException($"Call with ID {callId} not found.");
    //        assignments = s_dal.Assignment.ReadAll(a => a.CallId == callId).ToList();
    //        riskRange = s_dal.Config.RiskRange;
    //    }

    //    DateTime currentTime = AdminManager.Now;
    //    Assignment? activeAssignment = assignments.Find(a => a.EndOfTreatmentTime == null);
    //    Assignment? handledAssignments = assignments.Find(a => a.EndOfTreatmentTime != null && a.TypeOfEnding == DO.TypeOfEnding.Teated);

    //    if (activeAssignment != null)
    //    {
    //        if (call.MaxTimeFinishRead.HasValue && currentTime > call.MaxTimeFinishRead.Value - riskRange)
    //            return Status.Open;

    //        return Status.InProgress;
    //    }

    //    if (handledAssignments != null)
    //        return Status.Closed;

    //    if (call.MaxTimeFinishRead.HasValue && currentTime > call.MaxTimeFinishRead.Value)
    //        return Status.Expired;

    //    if (call.MaxTimeFinishRead.HasValue && currentTime > call.MaxTimeFinishRead.Value - riskRange)
    //        return Status.OpenAtRisk;

    //    return Status.Open;
    //}
    internal static BO.Status GetCallStatus(int callId)
    {
        try
        {
            DO.Call? call;
            List<DO.Assignment> assignments;

            lock (AdminManager.BlMutex)
            {
                call = s_dal.Call.Read(callId);
                if (call == null)
                    throw new BO.BlDoesNotExistException($"קריאה עם מזהה {callId} לא קיימת.");

                assignments = s_dal.Assignment.ReadAll(a => a.CallId == callId).ToList();
            }

            // אם אין הקצאות בכלל
            if (!assignments.Any() ||
                assignments.All(a =>
                    a.EndOfTreatmentTime.HasValue &&
                    (a.TypeOfEnding == DO.TypeOfEnding.ManagerCancellation ||
                     a.TypeOfEnding == DO.TypeOfEnding.SelfCancellation)))
            {
                var status = Tools.CalculateStatus(call);
                return status switch
                {
                    BO.Status.InProgress => BO.Status.Open,
                    BO.Status.Expired => BO.Status.Expired,
                    _ => BO.Status.OpenAtRisk
                };
            }

            // אם יש הקצאה שטופלה בהצלחה
            if (assignments.Any(a =>
                a.EndOfTreatmentTime.HasValue &&
                a.TypeOfEnding == DO.TypeOfEnding.Teated))
            {
                return BO.Status.Closed;
            }

            // אם עבר הזמן המותר
            if (call.MaxTimeFinishRead.HasValue &&
                call.MaxTimeFinishRead.Value < AdminManager.Now)
            {
                return BO.Status.Expired;
            }

            // אחרת - מחשבים לפי הזמן
            return Tools.CalculateStatus(call);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"קריאה עם מזהה {callId} לא קיימת.", ex);
        }
    }


    public static TimeSpan getRemainingTimeToEndCall(DO.Call c)
    {
        if (c.MaxTimeFinishRead.HasValue)
        {
            return c.MaxTimeFinishRead.Value - DateTime.Now;
        }
        else
        {
            return TimeSpan.Zero;
        }
    }

    public static TimeSpan getMaxTimeFinishRead(DO.Call c)
    {
        if (c.MaxTimeFinishRead.HasValue)
        {
            return c.MaxTimeFinishRead.Value - c.OpeningTime;
        }
        else
        {
            return TimeSpan.Zero;
        }
    }

    public static DO.Call ConvertToDO(BO.Call boCall)
    {
        (double latitude, double longitude) = Tools.GetCoordinates(boCall.Address);
        return new DO.Call
        {
            Id = boCall.CallId,
            VerbalDescription = boCall.Description,
            TypeOfReading = (DO.TypeOfReading)boCall.TypeOfReading,
            Address = boCall.Address,
            Latitude = latitude,
            Longitude = longitude,
            OpeningTime = boCall.OpeningTime,
            MaxTimeFinishRead = boCall.MaxEndTime
        };
    }

    public static BO.Call ConvertToBO(DO.Call call, List<CallAssignInList> assignments)
    {
        return new BO.Call
        {
            CallId = call.Id,
            Description = call.VerbalDescription,
            TypeOfReading = (BO.TypeOfReading)call.TypeOfReading,
            Address = call.Address,
            Latitude = call.Latitude,
            Longitude = call.Longitude,
            OpeningTime = call.OpeningTime,
            MaxEndTime = call.MaxTimeFinishRead,
            CallAssignments = assignments
        };
    }

    public static void ValidateCall(BO.Call newCall)
    {
        if (newCall == null)
            throw new Exception("Call object cannot be null");

        if (string.IsNullOrWhiteSpace(newCall.Description))
            throw new Exception("Call description cannot be empty");
        if (string.IsNullOrWhiteSpace(newCall.Address))
            throw new Exception("Call address cannot be empty");
        if (newCall.MaxEndTime <= newCall.OpeningTime)
            throw new Exception("Expiration time must be later than start time");
    }

    public static IEnumerable<T> GetCallsByFilter<T>(int volunteerId, BO.TypeOfReading? filterBy = null, CallField? sortByField = null, bool isOpen = true) where T : class
    {
        try
        {
            DO.Volunteer volunteer;
            List<DO.Assignment> assignments;
            List<DO.Call> callsList;
            lock (AdminManager.BlMutex)
            {
                volunteer = s_dal.Volunteer.Read(v => v.Id == volunteerId);
                if (volunteer == null)
                    throw new BO.BlNullPropertyException($"Volunteer with ID {volunteerId} not found.");

                assignments = s_dal.Assignment.ReadAll().Where(a =>
                    a.VolunteerId == volunteerId &&
                    (isOpen
                        ? CallManager.GetCallStatus(a.CallId) == Status.Open || CallManager.GetCallStatus(a.CallId) == Status.OpenAtRisk
                        : CallManager.GetCallStatus(a.CallId) == Status.Closed || a.TypeOfEnding == DO.TypeOfEnding.SelfCancellation)
                ).ToList();

                callsList = s_dal.Call.ReadAll().ToList();
            }

            var calls = from assign in assignments
                        join call in callsList on assign.CallId equals call.Id
                        select new { call, assign };

            if (filterBy != null)
                calls = calls.Where(x => x.call.TypeOfReading.Equals(filterBy));

            var result = calls.Select(c => isOpen
                ? new BO.OpenCallInList
                {
                    Id = c.call.Id,
                    Type = (BO.TypeOfReading)c.call.TypeOfReading,
                    Description = c.call.VerbalDescription,
                    FullAddress = c.call.Address,
                    OpeningTime = c.call.OpeningTime,
                    MaxCompletionTime = c.call.MaxTimeFinishRead,
                    DistanceFromVolunteer = CalculateDistance(volunteer.Latitude, volunteer.Longitude, c.call.Latitude, c.call.Longitude)
                } as T
                : new BO.ClosedCallInList
                {
                    Id = c.call.Id,
                    TypeOfReading = (BO.TypeOfReading)c.call.TypeOfReading,
                    FullAddress = c.call.Address,
                    OpeningTime = c.call.OpeningTime,
                    EntryTimeForHandling = c.assign.EntryTimeForTreatment,
                    ActualHandlingEndTime = c.assign.EndOfTreatmentTime,
                    TypeOfEnding = (BO.TypeOfEnding)c.assign.TypeOfEnding
                } as T);

            if (sortByField != null)
            {
                var property = typeof(T).GetProperty(sortByField.ToString());
                if (property != null)
                    result = result.OrderBy(call => property.GetValue(call));
            }
            else
            {
                result = isOpen
                    ? result.Cast<BO.OpenCallInList>().OrderBy(call => call.Id).Cast<T>()
                    : result.Cast<BO.ClosedCallInList>().OrderBy(call => call.OpeningTime).Cast<T>();
            }

            return result;
        }
        catch (DO.DalDoesNotExistException)
        {
            throw new BO.BlDoesNotExistException("Error retrieving calls.");
        }
    }

    public static double CalculateDistance(double? lat1, double? lon1, double lat2, double lon2)
    {
        if (lat1 == null || lon1 == null)
            throw new ArgumentException("Latitude or Longitude values are null.");

        const double R = 6371;
        double lat1Value = lat1.Value;
        double lon1Value = lon1.Value;
        double dLat = (lat2 - lat1Value) * Math.PI / 180;
        double dLon = (lon2 - lon1Value) * Math.PI / 180;
        double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                   Math.Cos(lat1Value * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180) *
                   Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return R * c;
    }

    internal static void PeriodicCallsUpdates(DateTime oldClock, DateTime newClock)
    {
        List<DO.Call> expiredCalls;
        lock (AdminManager.BlMutex)
            expiredCalls = s_dal.Call.ReadAll(c => c.MaxTimeFinishRead < newClock).ToList();

        expiredCalls.ForEach(call =>
        {
            List<DO.Assignment> assignments;
            lock (AdminManager.BlMutex)
            {
                assignments = s_dal.Assignment.ReadAll(a => a.CallId == call.Id).ToList();
                if (!assignments.Any())
                {
                    s_dal.Assignment.Create(new DO.Assignment(
                        CallId: call.Id,
                        VolunteerId: 0,
                        TypeOfEnding: (DO.TypeOfEnding)BO.TypeOfEnding.CancellationHasExpired,
                        EntryTimeForTreatment: AdminManager.Now,
                        EndOfTreatmentTime: AdminManager.Now
                    ));
                }
            }
            Observers.NotifyItemUpdated(call.Id);

            List<DO.Assignment> assignmentsWithNull;
            lock (AdminManager.BlMutex)
                assignmentsWithNull = s_dal.Assignment.ReadAll(a => a.CallId == call.Id && a.TypeOfEnding is null).ToList();
            if (assignmentsWithNull.Any())
            {
                lock (AdminManager.BlMutex)
                    foreach (var assignment in assignmentsWithNull)
                    {
                        s_dal.Assignment.Update(assignment with
                        {
                            EndOfTreatmentTime = AdminManager.Now,
                            TypeOfEnding = (DO.TypeOfEnding)BO.TypeOfEnding.CancellationHasExpired
                        });
                    }

                Observers.NotifyItemUpdated(call.Id);
            }
        });
    }
}
