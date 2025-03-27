
namespace BlImplementation;
using BlApi;
using BO;
using DalApi;
using DO;
using Helpers;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using ICall = BlApi.ICall;

internal class CallImplementation : ICall
{
    private readonly DalApi.IDal _dal = DalApi.Factory.Get;
    public void Create(BO.Call boCall)
    {
        //לזכור לעשות בדיקת תקינות 2 חשוב מאדדדדדדדדדדדדדדדדדדדדדד
        try
        {
            CallManager.ValidateCall(boCall);

            DO.Call newCallDO = CallManager.ConvertToDO(boCall);

            _dal.Call.Create(newCallDO);
            // Assuming the constructor for DO.Call takes parameters in a specific order.
            //DO.Call doCall = new DO.Call(
            //    boCall.Address,
            //    boCall.Latitude,
            //    boCall.Longitude,
            //    boCall.OpeningTime,
            //    (DO.TypeOfReading)boCall.TypeOfReading, // Assuming TypeOfReading needs to be cast
            //    boCall.Description,
            //    boCall.MaxEndTime
            //);

            //// Assuming this method is for creating a Call, not a Volunteer
            //_dal.Call.Create(doCall);
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
            var doCall = _dal.Call.Read(callId) ?? throw new BO.BlDoesNotExistException($"Call with ID={callId} does not exist.");
          
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

        //var volunteer = _dal.Volunteer.Read(v => v.Id == id) ?? throw new DO.DalDoesNotExistException($"Volunteer with ID={id} does not exist.");

        //var currentAssignment = _dal.Assignment.ReadAll(a => a.VolunteerId == id && a.FinishCompletionTime == null).FirstOrDefault();
        //if (currentAssignment != null)
        //{
        //    throw new InvalidOperationException("Cannot delete volunteer while they are handling a call.");
        //}
        //_dal.Volunteer.Delete(id);
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

    public BO.Call RequestCallDetails(int callId)
    {
        try
        {
            DO.Call? callData = _dal.Call.Read(c => c.Id == callId);
            if (callData == null)
                throw new BO.BlDoesNotExistException("Call not found");

            var assignmentData = _dal.Assignment.ReadAll()
                .Where(a => a.CallId == callId)
                .ToList();

            List<BO.CallAssignInList> assignmentList = assignmentData.Select(assignment => new BO.CallAssignInList
            {
                VolunteerId = assignment.VolunteerId,
                //check
                VolunteerName = _dal.Volunteer.Read(assignment.VolunteerId).Name,
                EntryTimeForTreatment = assignment.EntryTimeForTreatment,
                EndOfTreatmentTime = assignment.EndOfTreatmentTime,
                TypeOfEnding = (BO.TypeOfEnding)assignment.TypeOfEnding,
            }).ToList();


            BO.Call callBO = CallManager.ConvertToBO(callData, assignmentList);
            return callBO;
        }
        catch (Exception ex)
        {
            throw new BO.BlDoesNotExistException($"in RequestCallDetails", ex);
        
        }
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
                TypeOfReading = (BO.TypeOfReading)call.TypeOfReading,
                OpeningTime = call.OpeningTime,
                RemainingTimeToEndCall = CallManager.getRemainingTimeToEndCall(call), // יש להוסיף לוגיקה אם נדרשת
                LastVolunteerName = null, // יש להוסיף לוגיקה אם נדרשת
                TotalHandlingTime = CallManager.getMaxTimeFinishRead(call), // יש להוסיף לוגיקה אם נדרשת
                Status = CallManager.GetCallStatus(call.Id), // יש להוסיף לוגיקה אם נדרשת
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

    #region a
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
    #endregion
    public IEnumerable<BO.ClosedCallInList> RequestClosedCallsByVolunteer(int volunteerId, BO.TypeOfReading? filterBy, CallField? sortByField)
    {
        return CallManager.GetCallsByFilter<BO.ClosedCallInList>(volunteerId, filterBy, sortByField, isOpen: false);
    }

    //public IEnumerable<BO.OpenCallInList> RequestOpenCallsForSelection(int volunteerId, BO.TypeOfReading? filterBy,CallField? sortByField)
    //{
    //    return CallManager.GetCallsByFilter<BO.OpenCallInList>(volunteerId, filterBy, sortByField, isOpen: true);

    //}
    public IEnumerable<BO.OpenCallInList> RequestOpenCallsForSelection(int volunteerId, BO.TypeOfReading? filterBy, CallField? sortByField)
    {
        return CallManager.GetCallsByFilter<BO.OpenCallInList>(volunteerId, filterBy, sortByField, isOpen: true); // הפונקציה משתמשת ב-GetCallsByFilter
    }


    public void SelectCallForTreatment(int volunteerId, int callId)
    {

        try
        {

            var assignment = _dal.Assignment.Read(a => a.CallId == callId);
             if (assignment != null|| assignment.TypeOfEnding== assignment.TypeOfEnding)
               throw new Exception($"Assignment with ID {callId}  already exists");

        }
        catch (Exception ex)
        {
            throw new Exception("Error closing call", ex);
        }
    }



    public void UpdateCallCancellation(int requesterId, int assignmentId)
    {
        try
        {
            // Retrieve the assignment from the data layer using Read with filter
            DO.Assignment assignment = _dal.Assignment.Read(a => a.Id == assignmentId) ??
                throw new Exception("The requested assignment does not exist");

            // Check authorization - the requester must be either the volunteer or an admin
            DO.Volunteer volunteer = _dal.Volunteer.Read(v => v.Id == assignment.VolunteerId) ??
                throw new Exception("The volunteer was not found in the system");

            // Check if the requestor is an admin or the volunteer themselves
            bool isAdmin = volunteer.Role == DO.Role.Manager;
            bool isVolunteer = assignment.VolunteerId == requesterId;

            if (!isAdmin && !isVolunteer)
                throw new Exception("You do not have permission to cancel this call");

            // Ensure the assignment is still open
            if (assignment.EndOfTreatmentTime != null) // שורה זו נבדקת אם זמן הסיום כבר קיים
                throw new Exception("Cannot cancel a call that has already been closed");

            // Create a new assignment with updated EntryTimeForTreatment
            var updatedAssignment = assignment with // יצירת אובייקט חדש עם הערכים המעודכנים
            {
                EndOfTreatmentTime = DateTime.Now, // עדכון זמן הסיום
                TypeOfEnding = isVolunteer ? DO.TypeOfEnding.SelfCancellation : DO.TypeOfEnding.ManagerCancellation // עדכון סוג הסיום
            };

            _dal.Assignment.Update(updatedAssignment); // עדכון האובייקט ב- DAL
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while canceling the call", ex);
        }
    }





    //עדכון "ביטול טיפול" 
    //public void UpdateCallCancellation(int requesterId, int assignmentId)
    //{
    //    try
    //    {
    //        // Retrieve the assignment from the data layer using Read with filter
    //        DO.Assignment assignment = _dal.Assignment.Read(a => a.Id == assignmentId) ??
    //            throw new Exception("The requested assignment does not exist");

    //        // Check authorization - the requester must be either the volunteer or an admin
    //        DO.Volunteer volunteer = _dal.Volunteer.Read(v => v.Id == assignment.VolunteerId) ??
    //            throw new Exception("The volunteer was not found in the system");

    //        // Check if the requestor is an admin or the volunteer themselves
    //        bool isAdmin = volunteer.Role == DO.Role.Manager;

    //        bool isVolunteer = assignment.VolunteerId == requesterId;

    //        if (!isAdmin && !isVolunteer)
    //            throw new Exception("You do not have permission to cancel this call");

    //        //check

    //        // Ensure the assignment is still open
    //        if (assignment.EndOfTreatmentTime != null || assignment.EntryTimeForTreatment < DateTime.Now)
    //            throw new Exception("Cannot cancel a call that has already been closed");

    //        // Update the assignment data
    //        //if()
    //        assignment.EntryTimeForTreatment = DateTime.Now;
    //      //  assignment.FinishCompletionTime = isVolunteer ? ClosureType.SelfCancellation : ClosureType.AdminCancellation;

    //        _dal.Assignment.Update(assignment);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw new Exception("An error occurred while canceling the call", ex);
    //    }
    //}
    //עדכון "סיום טיפול"
    public void UpdateCallCompletion(int volunteerId, int assignmentId)
    {
        try
        {
            // Retrieve the assi gnment from the data layer
            var assignment = _dal.Assignment.Read(a => a.Id == assignmentId)
                ?? throw new Exception($"Assignment with ID {assignmentId} not found");

            // Check permissions - verify the volunteer assigned to this call
            if (assignment.VolunteerId != volunteerId)
                throw new Exception("Volunteer is not authorized to close this call");

            // Ensure the call is still open
            if (assignment.TypeOfEnding == DO.TypeOfEnding.None)
                throw new Exception("Call has already been closed or expired");

            // Update the assignment


            // Create a new assignment with updated EntryTimeForTreatment
            var updatedAssignment = assignment with // יצירת אובייקט חדש עם הערכים המעודכנים
            {
                EndOfTreatmentTime = DateTime.Now, // עדכון זמן הסיום
                TypeOfEnding =DO.TypeOfEnding.Teated // עדכון סוג הסיום
            };
            //assignment.EndOfTreatmentTime = ClockManager.Now;
            //assignment.TypeOfEnding = DO.TypeOfEnding.Closed; // השתמש ב-CallStatus במקום ClosureType
            _dal.Assignment.Update(assignment);
        }
        catch (Exception ex)
        {
            throw new Exception("Error closing call", ex);
        }
    }
    public void UpdateCallDetails(BO.Call call)
    {
        if (call == null)
            throw new BO.BlInvalidException("Call object cannot be null.");

        try
        {
            CallManager.ValidateCall(call);
            var (latitude, longitude) = Tools.GetCoordinates(call.Address);///
            if (latitude == 0 || longitude == 0)
                throw new BO.BlInvalidException("Invalid address: Unable to retrieve coordinates.");

            call.Latitude = latitude;
            call.Longitude = longitude;

            DO.Call doCall = CallManager.ConvertToDO(call);
            _dal.Call.Update(doCall);
    }
        catch (Exception ex)
        {
            throw new BO.BlInvalidException("Volunteer with ID already exists", ex);
        }
    }
}
