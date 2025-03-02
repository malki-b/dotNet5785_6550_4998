
namespace BlImplementation;
using BlApi;
using BO;
using Helpers;
using System.Collections.Generic;

internal class CallImplementation : ICall
{
    private readonly DalApi.IDal _dal = DalApi.Factory.Get;
    public void Create(BO.Call boCall)
    {
        //לזכור לעשות בדיקת תקינות 2 חשוב מאדדדדדדדדדדדדדדדדדדדדדד
        try
        {
            // Assuming the constructor for DO.Call takes parameters in a specific order.
            DO.Call doCall = new DO.Call(
                boCall.Address,
                boCall.Latitude,
                boCall.Longitude,
                boCall.OpeningTime,
                (DO.TypeOfReading)boCall.TypeOfReading, // Assuming TypeOfReading needs to be cast
                boCall.Description,
                boCall.MaxEndTime
            );

            // Assuming this method is for creating a Call, not a Volunteer
            _dal.Call.Create(doCall);
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlDoesNotExistException($"Volunteer with ID={boCall.CallId} already exists", ex);
        }
    }
    public void Delete(int callId)
    {
        try
        {
            var doCall = _dal.Call.Read(callId);
            if (doCall == null)
                throw new BO.BlDoesNotExistException($"Call with ID={callId} does not exist.");

            //var volunteer = _dal.Volunteer.Read(id)
            //    ?? throw new BO.BlDoesNotExistException($"Volunteer with ID={id} does not exist.");

            // Check if the volunteer is handling any cases
            if ((CallManager.GetCallStatus(callId) == BO.Status.Open)/*&&(doCall.)*/)
            {
                //_dal.Volunteer.Delete(id);
                //throw new BO.BlDoesNotExistException($"Failed to delete volunteer with ID={id}.", ex);
                try
                {
                    //Attempt to delete the volunteer from the data access layer
                    _dal.Call.Delete(callId);
                }
                catch (DO.DalNotFoundException ex)
                {
                    // Handle the case when the volunteer is not found in the data layer
                    throw new BO.BlDoesNotExistException($"Failed to delete volunteer with ID={callId}.", ex);
                }
            }
            else
            {
                throw new BO.BlCannotDeleteException("Volunteer cannot be deleted because they are currently handling cases.");
            }
        }
        catch (DO.DalDoNotSuccseedDelete ex)
        {
            throw new BO.BlCannotDeleteException("Volunteer with ID already exists", ex);
        }
    }
    public int[] RequestCallCounts()
     {

        var statusId = from dalCall in _dal.Call.ReadAll()
                       select new { Id = dalCall.Id, Status = CallManager.GetCallStatus(dalCall.Id) };

        var counter = from callLine in statusId
                      group callLine by callLine.Status into statusGroup
                      select statusGroup.Count();

        Console.WriteLine(counter.ToArray());
        return counter.ToArray();

     }
    //public int[] RequestCallCounts()
    //{
    //    //var calls = _dal.Call.GetAllCalls(); // Assuming you have a method to get all calls

    //    //// Group the calls by their status and count them
    //    //var callQuantities = calls.GroupBy(call => call.CallStatus)
    //    //                           .Select(group => new
    //    //                           {
    //    //                               Status = group.Key,
    //    //                               Count = group.Count()
    //    //                           })
    //    //                           .OrderBy(statusCount => statusCount.Status) // Optional: Order by status if needed
    //    //                           .ToArray();

    //    //// Initialize an array for counts, assuming Status is 0 to n
    //    //int[] quantities = new int[Enum.GetValues(typeof(Status)).Length];

    //    //foreach (var statusCount in callQuantities)
    //    //{
    //    //    quantities[(int)statusCount.Status] = statusCount.Count;
    //    //}

    //    //return quantities;
    //    throw new NotImplementedException();

    //}

    public Call RequestCallDetails(int callId)
    {
        throw new NotImplementedException();
    }


    public IEnumerable<CallInList> ReadAll(CallField? filterBy, object? filterValue, CallField? sortBy)
    {
        IEnumerable<DO.Call> calls = _dal.Call.ReadAll().ToList();

        // פונקציה ליצירת CallInList מתוך DO.Call
        CallInList CreateCallInList(DO.Call call)
        {
            return new CallInList
            {
                AssignmentId = call.Id, // אם אתה מתכוון להשתמש ב-Id של ה-Call
                CallId = call.Id, // אם CallId הוא אותו Id
                TypeOfReading =(BO.TypeOfReading)call.TypeOfReading,
                OpeningTime = call.OpeningTime,
                RemainingTimeToEndCall = CallManager.getRemainingTimeToEndCall(call), // יש להוסיף לוגיקה אם נדרשת
                LastVolunteerName = null, // יש להוסיף לוגיקה אם נדרשת
                TotalHandlingTime = CallManager.getMaxTimeFinishRead(call), // יש להוסיף לוגיקה אם נדרשת
                Status =CallManager.GetCallStatus(call.Id), // יש להוסיף לוגיקה אם נדרשת
                TotalAssignments = 0 // יש להוסיף לוגיקה אם נדרשת
            };
        }
  
        var callInLists = calls.Select(CreateCallInList).DistinctBy(c => c.CallId);

        // אם filterBy הוא null, מחזירים את כל הקריאות
        if (filterBy != null)
        {
            var propertyFilter = typeof(BO.CallInList).GetProperty(filterBy.ToString());
             callInLists = callInLists.Where(call => propertyFilter.GetValue(call, null)?.Equals(filterValue) ?? false);

        }

        // סינון הקריאות לפי filterBy ו-filterValue
        //var propertyFilter = typeof(BO.CallInList).GetProperty(filterBy.ToString());
        //var filteredCalls = callInLists.Where(call => propertyFilter.GetValue(call, null)?.Equals(filterValue) ?? false);

        // אם sortBy הוא null, מחזירים את הקריאות המסוננות
        if (sortBy == null)
        {
            return callInLists.OrderBy(v => v.CallId).ToList();

        }

        // מיון הקריאות לפי sortBy
        var propertyInfo = typeof(BO.CallInList).GetProperty(sortBy.ToString());
        if (propertyInfo != null)
        {
            return callInLists.OrderBy(v => propertyInfo.GetValue(v, null)).ToList();
        }

        return callInLists.ToList();
    }


    //public IEnumerable<CallInList> ReadAll(CallField? filterBy, object? filterValue, CallField? sortBy)
    //{
    //    IEnumerable<DO.Call> calls = _dal.Call.ReadAll().ToList();

    //    // אם filterBy הוא null, מחזירים את כל הקריאות
    //    if (filterBy == null)
    //    {
    //        return calls.Select(c => new CallInList { /* אתחול שדות כאן */ })
    //                    .DistinctBy(c => c.CallId) // להבטיח שכל קריאה תופיע פעם אחת
    //                    .OrderBy(v => v.CallId) // מיון לפי CallId
    //                    .ToList();
    //    }

    //    // סינון הקריאות לפי filterBy ו-filterValue
    //    var propertyFilter = typeof(BO.CallInList).GetProperty(filterBy.ToString());
    //    var filteredCalls = calls.Where(call => propertyFilter.GetValue(new CallInList { /* אתחול שדות כאן */ }, null)?.Equals(filterValue) ?? false);

    //    // אם sortBy הוא null, מחזירים את הקריאות המסוננות
    //    if (sortBy == null)
    //    {
    //        return filteredCalls.Select(c => new CallInList { /* אתחול שדות כאן */ })
    //                            .DistinctBy(c => c.CallId)
    //                            .ToList();
    //    }

    //    // מיון הקריאות לפי sortBy
    //    var propertyInfo = typeof(BO.CallInList).GetProperty(sortBy.ToString());
    //    if (propertyInfo != null)
    //    {
    //        return filteredCalls.Select(c => new CallInList { /* אתחול שדות כאן */ })
    //                           .DistinctBy(c => c.CallId)
    //                           .OrderBy(v => propertyInfo.GetValue(v, null))
    //                           .ToList();
    //    }

    //    return filteredCalls.Select(c => new CallInList { /* אתחול שדות כאן */ }).DistinctBy(c => c.CallId).ToList();
    //}



    //public IEnumerable<CallInList> ReadAll(CallField? filterBy, object? filterValue, CallField? sortBy)
    //{


    //    IEnumerable<DO.Call> calls = _dal.Call.ReadAll().ToList();
    //    var propertyFilter = typeof(BO.CallInList).GetProperty(filterBy.ToString());

    //    var counter = from callLine in calls
    //                  where (callLine.propertyFilter.GetValue == filterValue)
    //                  select o => o.all;


    //    //IEnumerable<BO.CallInList> callList = calls.Select(call =>
    //    //{
    //    //    var volunteerCalls = _dal.Call.ReadAll(a => a.Id == call.Id);

    //    //    int TotalHandledRequests = volunteercalls.Count(a => a.TypeOfEnding == DO.TypeOfEnding.Teated);
    //    //    int TotalCanceledRequests = volunteerAssignments.Count(a => a.TypeOfEnding == DO.TypeOfEnding.SelfCancellation || a.TypeOfEnding == DO.TypeOfEnding.ManagerCancellation);
    //    //    int TotalExpiredRequests = volunteerAssignments.Count(a => a.TypeOfEnding == DO.TypeOfEnding.CancellationHasExpired);

    //    //    var currentCallId = volunteerAssignments.FirstOrDefault(a => a.TypeOfEnding == null)?.CallId;

    //    //    BO.TypeOfReading callType = currentCallId.HasValue ? (BO.TypeOfReading)_dal.Call.Read(currentCallId.Value)!.TypeOfReading : (BO.TypeOfReading)DO.TypeOfReading.None;

    //    //    return new BO.CallInList
    //    //    {
    //    //        AssignmentId = call.volunteers,
    //    //        CallId = call.CallId,
    //    //        TypeOfReading = call.TypeOfReading,
    //    //        OpeningTime = call.OpeningTime,
    //    //        RemainingTimeToEndCall = call.RemainingTimeToEndCall,
    //    //        LastVolunteerName = call.LastVolunteerName,
    //    //        TotalHandlingTime = call.TotalHandlingTime,
    //    //        Status = call.Status,
    //    //        TotalAssignments = call.TotalAssignments
    //    //    };
    //    //});


    //    if (sortBy == null)
    //    {
    //        return callList.OrderBy(v => v.CallId).ToList(); // Sort by Id if sortBy is null
    //    }

    //    // Sorting based on the specified enum value
    //    var propertyInfo = typeof(BO.CallInList).GetProperty(sortBy.ToString());

    //    if (propertyInfo != null)
    //    {
    //        return callList.OrderBy(v => propertyInfo.GetValue(v, null)).ToList();
    //    }

    //    if (filterBy == null)
    //    {
    //        return callList;
    //    }

    //    return callList;

    //}

    public IEnumerable<ClosedCallInList> RequestClosedCallsByVolunteer(int volunteerId, TypeOfReading? filterBy, CallField? sortBy)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<OpenCallInList> RequestOpenCallsForSelection(int volunteerId, TypeOfReading? filterBy, CallField? sortBy)
    {
        throw new NotImplementedException();
    }

    public void SelectCallForTreatment(int volunteerId, int callId)
    {
        throw new NotImplementedException();
    }

    public void UpdateCallCancellation(int requesterId, int assignmentId)
    {
        throw new NotImplementedException();
    }

    public void UpdateCallCompletion(int volunteerId, int assignmentId)
    {
        throw new NotImplementedException();
    }

    public void UpdateCallDetails(Call call)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<CallInList> ReadAll(CallField? filterBy, object? filterValue, CallField? sortBy)
    {
        throw new NotImplementedException();
    }

    Call ICall.RequestCallDetails(int callId)
    {
        throw new NotImplementedException();
    }

    public void UpdateCallDetails(Call call)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<ClosedCallInList> RequestClosedCallsByVolunteer(int volunteerId, TypeOfReading? filterBy, CallField? sortBy)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<OpenCallInList> RequestOpenCallsForSelection(int volunteerId, TypeOfReading? filterBy, CallField? sortBy)
    {
        throw new NotImplementedException();
    }
}
