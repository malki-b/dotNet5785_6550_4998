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
    internal static Status GetCallStatus(int callId)
    {
        var call = s_dal.Call.Read(callId) ??
            throw new BO.BlDoesNotExistException($"Call with ID {callId} not found.");

        DateTime currentTime = AdminManager.Now;
        var assignments = s_dal.Assignment.ReadAll(a => a.CallId == callId).ToList();
        Assignment? activeAssignment = assignments.Find(a => a.EndOfTreatmentTime == null);
        Assignment? handledAssignments = assignments.Find(a => a.EndOfTreatmentTime != null && a.TypeOfEnding == DO.TypeOfEnding.Teated);

        if (activeAssignment != null)
        {
            if (call.MaxTimeFinishRead.HasValue && currentTime > call.MaxTimeFinishRead.Value - s_dal.Config.RiskRange)
                return Status.Open;

            return Status.InProgress;
        }

        if (handledAssignments != null)
            return Status.Closed;

        if (call.MaxTimeFinishRead.HasValue && currentTime > call.MaxTimeFinishRead.Value)
            return Status.Expired;

        if (call.MaxTimeFinishRead.HasValue && currentTime > call.MaxTimeFinishRead.Value - s_dal.Config.RiskRange)
            return Status.OpenAtRisk;

        return Status.Open;
    }


    public static TimeSpan getRemainingTimeToEndCall(DO.Call c)
    {
        if (c.MaxTimeFinishRead.HasValue)
        {
            return c.MaxTimeFinishRead.Value - DateTime.Now;
        }
        else
        {
            return TimeSpan.Zero; // או להחזיר ערך אחר במידה ואין MaxTimeFinishRead
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
            return TimeSpan.Zero; // או להחזיר ערך אחר במידה ואין MaxTimeFinishRead
        }
    }
    public static DO.Call ConvertToDO(BO.Call boCall)
    {
        (double latitude, double longitude) = Tools.GetCoordinates(boCall.Address);
        return new DO.Call
        {
            Id = boCall.CallId,
            VerbalDescription = boCall.Description,
            TypeOfReading = (DO.TypeOfReading)boCall.TypeOfReading,  // המרה בין הטיפוסים
            Address = boCall.Address,
            //Latitude = boCall.Latitude,
            //Longitude = boCall.Longitude,
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

    public static IEnumerable<T> GetCallsByFilter<T>(int volunteerId, BO.TypeOfReading? filterBy=null, CallField? sortByField = null, bool isOpen=true) where T : class
    {
      
        try
        {
            var volunteer = s_dal.Volunteer.Read(v => v.Id == volunteerId);
            if (volunteer == null)
                throw new BO.BlNullPropertyException($"Volunteer with ID {volunteerId} not found.");

            // "קריאה סגורה ברשימה"
            //קריאה סגורה
            //var assignments = s_dal.Assignment.ReadAll()
            //    .Where(a => a.VolunteerId == volunteerId &&
            //                (isOpen ? a. == BO.Status.Open || a.TypeOfEnding == DO.TypeOfEnding.OpenRisk
            //                        : a.TypeOfEnding == DO.TypeOfEnding.Closed));

            var assignments = s_dal.Assignment.ReadAll()
                .Where(a => a.VolunteerId == volunteerId &&
                            (isOpen ? CallManager.GetCallStatus(a.CallId) == Status.Open || CallManager.GetCallStatus(a.CallId) == Status.OpenAtRisk
                                    : CallManager.GetCallStatus(a.CallId) == Status.Closed));

            //(isOpen ? a.TypeOfEnding == DO.TypeOfEnding.Open || a.TypeOfEnding == DO.TypeOfEnding.OpenRisk
            //                        : a.TypeOfEnding == DO.TypeOfEnding.Closed));

            var calls = from assign in assignments
                        join call in s_dal.Call.ReadAll()
                            on assign.CallId equals call.Id
                        select new { call, assign };
            ///
            if (filterBy != null)
                calls = calls.Where(x => x.call.TypeOfReading.Equals(filterBy));

            ////
                        var result = calls.Select(c => isOpen
                        ? new BO.OpenCallInList
                        {
                            Id = c.call.Id,
                            Type = (BO.TypeOfReading)c.call.TypeOfReading,
                            Description = c.call.VerbalDescription,
                            FullAddress = c.call.Address,
                            OpeningTime = c.call.OpeningTime,
                            MaxCompletionTime = c.call.MaxTimeFinishRead,
                            DistanceFromVolunteer =CalculateDistance(volunteer.Latitude, volunteer.Longitude, c.call.Latitude, c.call.Longitude)
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
                            //TypeOfEnding = Enum.TryParse<BO.TypeOfReading>(c.assign.TypeOfEnding.ToString(), out BO.TypeOfEnding completionStatus)
                            //                   ? (BO.TypeOfReading?)completionStatus
                            //                   : null
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

        const double R = 6371; // רדיוס כדור הארץ בקילומטרים
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
}

