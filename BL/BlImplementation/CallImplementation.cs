
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
            CallManager.Observers.NotifyListUpdated();
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
                  
                    DO.Assignment assignment = _dal.Assignment.Read(a => a.CallId == callId) ??
          throw new Exception("The requested assignment does not exist");


                    var updatedAssignment = assignment with // יצירת אובייקט חדש עם הערכים המעודכנים
                    {
                        TypeOfEnding = DO.TypeOfEnding.ManagerCancellation, // עדכון זמן הסיום
                        //TypeOfEnding = DO.TypeOfEnding.Teated // עדכון סוג הסיום
                    };
                    _dal.Assignment.Update(updatedAssignment); // עדכון האובייקט ב- DAL
                    CallManager.Observers.NotifyItemUpdated(updatedAssignment.Id);
                    CallManager.Observers.NotifyListUpdated();


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








    public IEnumerable<CallInList> ReadAll(BO.CallField? filterField = null, object? filterValue = null, BO.CallField? sortField = null)
    {
        try
        {
            var calls = _dal.Call.ReadAll()
                .Select(c =>
                {
                    var assignments = _dal.Assignment.ReadAll(a => a.CallId == c.Id);
                    var lastAssignment = assignments.OrderByDescending(a => a.EntryTimeForTreatment).FirstOrDefault();
                    return new CallInList
                    {
                        AssignmentId = lastAssignment?.Id,
                        CallId = c.Id,
                        TypeOfReading = (BO.TypeOfReading)c.TypeOfReading,
                        OpeningTime = c.OpeningTime,
                        RemainingTimeToEndCall = CallManager.getRemainingTimeToEndCall(c),
                        LastVolunteerName = lastAssignment != null ? _dal.Volunteer.Read(lastAssignment.VolunteerId)?.Name : null,
                        TotalHandlingTime = CallManager.getMaxTimeFinishRead(c),
                        Status = CallManager.GetCallStatus(c.Id),
                        TotalAssignments = assignments.Count()
                    };

                });
            if (filterField.HasValue && filterValue != null)
            {
                var prop = typeof(BO.CallInList).GetProperty(filterField.ToString());
                if (prop != null)
                {
                    if (prop.PropertyType.IsEnum)
                    {
                        var enumValue = Enum.Parse(prop.PropertyType, filterValue.ToString());
                        calls = calls.Where(c => prop.GetValue(c)?.Equals(enumValue) == true);
                    }
                    else
                    {
                        var convertedValue = Convert.ChangeType(filterValue, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                        calls = calls.Where(c => prop.GetValue(c)?.Equals(convertedValue) == true);
                    }
                }
            }

            return sortField.HasValue
                ? calls.OrderBy(c => typeof(BO.CallInList).GetProperty(sortField.ToString())?.GetValue(c))
                : calls.OrderBy(c => c.CallId);
        }
        catch (Exception ex)
        {
            throw new BO.BlCannotDeleteException("Failed to retrieve calls list", ex);
        }
    }






    //גירסא ישנה
    //public IEnumerable<CallInList> ReadAll(CallField? filterBy, object? filterValue, CallField? sortBy)
    //{
    //    IEnumerable<DO.Call> calls = _dal.Call.ReadAll().ToList();

    //    // פונקציה ליצירת CallInList מתוך DO.Call
    //    CallInList CreateCallInList(DO.Call call)
    //    {
    //        return new CallInList
    //        {
    //            AssignmentId = call.Id, // אם אתה מתכוון להשתמש ב-Id של ה-Call
    //            CallId = call.Id, // אם CallId הוא אותו Id
    //            TypeOfReading = (BO.TypeOfReading)call.TypeOfReading,
    //            OpeningTime = call.OpeningTime,
    //            RemainingTimeToEndCall = CallManager.getRemainingTimeToEndCall(call), // יש להוסיף לוגיקה אם נדרשת
    //            LastVolunteerName = null, // יש להוסיף לוגיקה אם נדרשת
    //            TotalHandlingTime = CallManager.getMaxTimeFinishRead(call), // יש להוסיף לוגיקה אם נדרשת
    //            Status = CallManager.GetCallStatus(call.Id), // יש להוסיף לוגיקה אם נדרשת
    //            TotalAssignments = 0 // יש להוסיף לוגיקה אם נדרשת
    //        };
    //    }

    //    var callInLists = calls.Select(CreateCallInList).DistinctBy(c => c.CallId);

    //    // אם filterBy הוא null, מחזירים את כל הקריאות
    //    //if (filterBy != null)
    //    //{
    //    //    var propertyFilter = typeof(BO.CallInList).GetProperty(filterBy.ToString());
    //    //    callInLists = callInLists.Where(call => propertyFilter.GetValue(call, null)?.Equals(filterValue) ?? false);

    //    //}


    //    if (filterBy != null)
    //    {
    //        var propertyFilter = typeof(BO.CallInList).GetProperty(filterBy.ToString());

    //        if (propertyFilter == null)
    //        {
    //            throw new Exception($"Property '{filterBy}' not found in BO.CallInList.");

    //           // Console.WriteLine($"Property '{filterBy}' not found in BO.CallInList.");
    //            //return; // ניתן לשנות את זה בהתאם לצורך שלך בטיפול בשגיאה
    //        }

    //        callInLists = callInLists.Where(call => propertyFilter.GetValue(call, null)?.Equals(filterValue) ?? false);
    //    }
    //    // סינון הקריאות לפי filterBy ו-filterValue
    //    //var propertyFilter = typeof(BO.CallInList).GetProperty(filterBy.ToString());
    //    //var filteredCalls = callInLists.Where(call => propertyFilter.GetValue(call, null)?.Equals(filterValue) ?? false);

    //    // אם sortBy הוא null, מחזירים את הקריאות המסוננות
    //    if (sortBy == null)
    //    {
    //        return callInLists.OrderBy(v => v.CallId).ToList();

    //    }

    //    // מיון הקריאות לפי sortBy
    //    var propertyInfo = typeof(BO.CallInList).GetProperty(sortBy.ToString());
    //    if (propertyInfo != null)
    //    {
    //        return callInLists.OrderBy(v => propertyInfo.GetValue(v, null)).ToList();
    //    }

    //    return callInLists.ToList();
    //}

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
    public IEnumerable<BO.ClosedCallInList> RequestClosedCallsByVolunteer(int volunteerId, BO.TypeOfReading? filterBy = null, ClosedCallField? sortByField = null)
    {

  
        try
        {
            var volunteer = _dal.Volunteer.Read(v => v.Id == volunteerId);
            if (volunteer == null)
                throw new BO.BlNullPropertyException($"Volunteer with ID {volunteerId} not found.");

            var assignments = _dal.Assignment.ReadAll()
                .Where(a => a.VolunteerId == volunteerId &&
                            (CallManager.GetCallStatus(a.CallId) == Status.Closed || a.TypeOfEnding == DO.TypeOfEnding.SelfCancellation));

       
            var calls = from assign in assignments
                        join call in _dal.Call.ReadAll()
                            on assign.CallId equals call.Id
                        select new { call, assign };

            if (filterBy != null)
                calls = calls.Where(x => x.call.TypeOfReading == (DO.TypeOfReading)filterBy);
            var result = calls.Select(c => new BO.ClosedCallInList
            {
                Id = c.call.Id,
                TypeOfReading = (BO.TypeOfReading)c.call.TypeOfReading,
                FullAddress = c.call.Address,
                OpeningTime = c.call.OpeningTime,
                EntryTimeForHandling = c.assign.EntryTimeForTreatment,
                ActualHandlingEndTime = c.assign.EndOfTreatmentTime,
                TypeOfEnding = (BO.TypeOfEnding)c.assign.TypeOfEnding
            });

            if (sortByField != null)
            {
                var property = typeof(BO.ClosedCallInList).GetProperty(sortByField.ToString());
                if (property != null)
                    result = result.OrderBy(call => property.GetValue(call));
            }
            else
            {
                result = result.OrderBy(call => call.OpeningTime);
            }

            return result;
        }
        catch (DO.DalDoesNotExistException)
        {
            throw new BO.BlDoesNotExistException("Error retrieving calls.");
        }


    }

    //return CallManager.GetCallsByFilterClose<BO.ClosedCallInList>(volunteerId, filterBy, sortByField, isOpen: false);


    //public IEnumerable<BO.OpenCallInList> RequestOpenCallsForSelection(int volunteerId, BO.TypeOfReading? filterBy,CallField? sortByField)
    //{
    //    return CallManager.GetCallsByFilter<BO.OpenCallInList>(volunteerId, filterBy, sortByField, isOpen: true);

    //}
    public IEnumerable<BO.OpenCallInList> RequestOpenCallsForSelection(int volunteerId, BO.TypeOfReading? filterBy = null, OpenCallField? sortByField = null)
    {
  
        var volunteer = _dal.Volunteer.Read(v => v.Id == volunteerId)
            ?? throw new BO.BlNullPropertyException($"Volunteer with ID {volunteerId} not found.");

        var calls = _dal.Call.ReadAll()
            .Where(c =>
            {
                var status = CallManager.GetCallStatus(c.Id);
                return status == Status.Open || status == Status.OpenAtRisk;
            });

        if (filterBy != null)
            calls = calls.Where(c => c.TypeOfReading == (DO.TypeOfReading)filterBy);

        var openCallList = calls.Select(c => new BO.OpenCallInList
        {
            Id = c.Id,
            Type = (BO.TypeOfReading)c.TypeOfReading,
            Description = c.VerbalDescription,
            FullAddress = c.Address,
            OpeningTime = c.OpeningTime,
            MaxCompletionTime = c.MaxTimeFinishRead,
            DistanceFromVolunteer = CallManager.CalculateDistance(volunteer.Latitude, volunteer.Longitude, c.Latitude, c.Longitude)
        });

        if (sortByField != null)
        {
            var prop = typeof(BO.OpenCallInList).GetProperty(sortByField.ToString());
            if (prop != null)
                openCallList = openCallList.OrderBy(c => prop.GetValue(c));
        }

        return openCallList;
   

        //try
        //{
        //    var volunteer = _dal.Volunteer.Read(v => v.Id == volunteerId);
        //    if (volunteer == null)
        //        throw new BO.BlNullPropertyException($"Volunteer with ID {volunteerId} not found.");

        //    var assignments = _dal.Assignment.ReadAll()
        //        .Where(a => a.VolunteerId == volunteerId &&
        //                    (CallManager.GetCallStatus(a.CallId) == Status.Open || CallManager.GetCallStatus(a.CallId) == Status.OpenAtRisk));

        //    var calls = from assign in assignments
        //                join call in _dal.Call.ReadAll()
        //                    on assign.CallId equals call.Id
        //                select new { call, assign };

        //    if (filterBy != null)
        //        calls = calls.Where(x => x.call.TypeOfReading == (DO.TypeOfReading)filterBy);
        //    var result = calls.Select(c => new BO.OpenCallInList
        //    {
        //        Id = c.call.Id,
        //        Type = (BO.TypeOfReading)c.call.TypeOfReading,
        //        Description = c.call.VerbalDescription,
        //        FullAddress = c.call.Address,
        //        OpeningTime = c.call.OpeningTime,
        //        MaxCompletionTime = c.call.MaxTimeFinishRead,
        //        DistanceFromVolunteer = CallManager.CalculateDistance(volunteer.Latitude, volunteer.Longitude, c.call.Latitude, c.call.Longitude)
        //    });

        //    // מיון בטוח לפי השדה שנבחר
        //    if (sortByField != null)
        //    {
        //        switch (sortByField)
        //        {
        //            case OpenCallField.FullAddress:
        //                result = result.OrderBy(call => call.FullAddress);
        //                break;
        //            case OpenCallField.OpeningTime:
        //                result = result.OrderBy(call => call.OpeningTime);
        //                break;
        //            case OpenCallField.MaxCompletionTime:
        //                result = result.OrderBy(call => call.MaxCompletionTime);
        //                break;
        //            case OpenCallField.DistanceFromVolunteer:
        //                result = result.OrderBy(call => call.DistanceFromVolunteer);
        //                break;
        //            default:
        //                result = result.OrderBy(call => call.OpeningTime);
        //                break;
        //        }
        //    }
        //    else
        //    {
        //        result = result.OrderBy(call => call.OpeningTime);
        //    }

        //    return result;
        //}
        //catch (DO.DalDoesNotExistException)
        //{
        //    throw new BO.BlDoesNotExistException("Error retrieving calls.");
        //}
    }

      //  var openedCalls = ReadAll();
      //  openedCalls = openedCalls.Where(a =>
      //Helpers.CallManager.GetCallStatus(a.CallId) == BO.Status.Open ||
      // Helpers.CallManager.GetCallStatus(a.CallId) == BO.Status.OpenAtRisk);
      //  var openCallsToReturn = openedCalls.Select(a =>
      //  {
      //      var call = _dal.Call.Read(c => c.Id == a.CallId)!;
      //      return new BO.OpenCallInList
      //      {
      //          Id = a.CallId,
      //          Type = (BO.TypeOfReading)call.TypeOfReading,
      //          FullAddress = call.Address,
      //          OpeningTime = call.OpeningTime,
      //          MaxCompletionTime = call.MaxTimeFinishRead,
      //          DistanceFromVolunteer = Helpers.CallManager.CalculateDistance(volunteerId, a.CallId),
      //          DistanceFromVolunteer = call.VerbalDescription
      //      };
      //  });

      //  if (filterBy != null)
      //      openCallsToReturn = openCallsToReturn.Where(call => call.TypeOfReading == filterBy);
      //  openCallsToReturn = sortByField switch
      //  {
      //      BO.CallField.Address => openCallsToReturn.OrderBy(call => call.Address),
      //      BO.CallField.CallType => openCallsToReturn.OrderBy(call => call.CallType),
      //      BO.OpenCallInListFields.OpenTime => openCallsToReturn.OrderBy(call => call.OpenTime),
      //      BO.OpenCallInListFields.MaxCloseTime => openCallsToReturn.OrderBy(call => call.MaxCloseTime),
      //      BO.OpenCallInListFields.Distance => openCallsToReturn.OrderBy(call => call.Distance),
      //      BO.OpenCallInListFields.Description => openCallsToReturn.OrderBy(call => call.Description),
      //      _ => openCallsToReturn.OrderBy(call => call.Id),
      //  };
      //  return openCallsToReturn;
     // return CallManager.GetCallsByFilterOpen<BO.OpenCallInList>(volunteerId, filterBy, sortByField, isOpen: true); // הפונקציה משתמשת ב-GetCallsByFilter

//}

    //public void SelectCallForTreatment(int volunteerId, int callId)
    //{

    //    try
    //    {

    //        var assignment = _dal.Assignment.Read(a => a.CallId == callId);
    //        if (assignment != null || assignment.TypeOfEnding == assignment.TypeOfEnding)
    //            throw new Exception($"Assignment with ID {callId}  already exists");

    //    }
    //    catch (Exception ex)
    //    {
    //        throw new Exception("Error closing call", ex);
    //    }
    //}




    public void SelectCallForTreatment(int volunteerId, int callId)
    {
        try
        {
            var call = _dal.Call.Read(callId) ?? throw new BO.BlInvalidException($"Call with ID {callId} not found.");
            var status = CallManager.GetCallStatus(callId);

            if (status == Status.Expired || status == Status.Closed || (status == Status.InProgress && _dal.Assignment.Read(callId) != null))
            {
                throw new BO.BlInvalidException($"Cannot select this call for treatment, since the call's status is: {status}");
            }

            var newAssignment = new DO.Assignment(
                CallId: callId,
                VolunteerId: volunteerId,
                EntryTimeForTreatment: AdminManager.Now,
                EndOfTreatmentTime: null,
                TypeOfEnding: null
            );
            _dal.Assignment.Create(newAssignment);
            CallManager.Observers.NotifyListUpdated();

        }
        catch (BO.BlInvalidException ex)
        {
            throw new BO.BlInvalidException($"Invalid operation: {ex.Message}", ex);
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
            //_dal.Assignment.Update(assignment);  maybe
            _dal.Assignment.Update(updatedAssignment); // עדכון האובייקט ב- DAL
            CallManager.Observers.NotifyItemUpdated(updatedAssignment.Id);
            CallManager.Observers.NotifyListUpdated();

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
            //if (assignment.TypeOfEnding == DO.TypeOfEnding.None)
            //    throw new Exception("Call has already been closed or expired");

            // Update the assignment


            // Create a new assignment with updated EntryTimeForTreatment
            var updatedAssignment = assignment with // יצירת אובייקט חדש עם הערכים המעודכנים
            {
                EndOfTreatmentTime = DateTime.Now, // עדכון זמן הסיום
                TypeOfEnding = DO.TypeOfEnding.Teated // עדכון סוג הסיום
            };
            //assignment.EndOfTreatmentTime = ClockManager.Now;
            //assignment.TypeOfEnding = DO.TypeOfEnding.Closed; // השתמש ב-CallStatus במקום ClosureType
            _dal.Assignment.Update(updatedAssignment);
            CallManager.Observers.NotifyItemUpdated(updatedAssignment.Id);
            CallManager.Observers.NotifyListUpdated();
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
            CallManager.Observers.NotifyItemUpdated(doCall.Id);
            CallManager.Observers.NotifyListUpdated();

        }
        catch (Exception ex)
        {
            throw new BO.BlInvalidException("Volunteer with ID already exists", ex);
        }
    }




    ///// <summary>
    ///// Retrieves a filtered and sorted list of calls based on optional filtering and sorting criteria.
    ///// </summary>
    ///// <param name="filterBy">Optional property to filter by.</param>
    ///// <param name="filterValue">Value to filter the specified property by.</param>
    ///// <param name="sortBy">Optional property to sort results by.</param>
    ///// <returns>An <see cref="IEnumerable{BO.CallInList}"/> containing the filtered and sorted calls.</returns>
    //public IEnumerable<BO.CallInList> GetFilteredAndSortedCalls(
    //    BO.CallInListFields? filterBy = null,
    //    object? filterValue = null,
    //    BO.CallInListFields? sortBy = null)
    //{
    //    IEnumerable<DO.Call> allCalls = _dal.Call.ReadAll().ToList();

    //    IEnumerable<BO.CallInList> callsList = allCalls.Select(call =>
    //    {
    //        var callAssignments = _dal.Assignment.ReadAll(a => a.CallId == call.Id);

    //        int TotalHandledRequests = callAssignments.Count(a => a.TypeOfEnding == DO.TypeOfEnding.Teated);
    //        int TotalCanceledRequests = callAssignments.Count(a => a.TypeOfEnding == DO.TypeOfEnding.SelfCancellation || a.TypeOfEnding == DO.TypeOfEnding.ManagerCancellation);
    //        int TotalExpiredRequests = callAssignments.Count(a => a.TypeOfEnding == DO.TypeOfEnding.CancellationHasExpired);

    //        var currentCallHandled = callAssignments.FirstOrDefault(a => a.TypeOfEnding == null);
    //        var currentCallId = currentCallHandled?.CallId;
    //        BO.TypeOfReading callType = currentCallId.HasValue ? (BO.TypeOfReading)_dal.Call.Read(currentCallId.Value)!.TypeOfReading : (BO.TypeOfReading)DO.TypeOfReading.None;

    //        return new BO.CallInList
    //        {
    //            CallId = call.Id,
    //            AssignmentId = call.Id,
    //            IsActive = call.IsActive ?? false,
    //            TotalHandledRequests = TotalHandledRequests,
    //            TotalCanceledRequests = TotalCanceledRequests,
    //            TotalExpiredRequests = TotalExpiredRequests,
    //            HandledRequestId = currentCallId,
    //            TypeOfReading = callType
    //        };
    //    });

    //    // Filtering
    //    if (filterBy != null && filterValue != null)
    //    {
    //        callsList = filterBy switch
    //        {
    //            BO.CallInListFields.CallId => callsList.Where(c => c.CallId.Equals(Convert.ToInt32(filterValue))),
    //            BO.CallInListFields.FullName => callsList.Where(c => c.FullName.Equals(filterValue.ToString())),
    //            BO.CallInListFields.IsActive => callsList.Where(c => c.IsActive.Equals(Convert.ToBoolean(filterValue))),
    //            BO.CallInListFields.TotalHandledRequests => callsList.Where(c => c.TotalHandledRequests.Equals(Convert.ToInt32(filterValue))),
    //            BO.CallInListFields.TotalCanceledRequests => callsList.Where(c => c.TotalCanceledRequests.Equals(Convert.ToInt32(filterValue))),
    //            BO.CallInListFields.TotalExpiredRequests => callsList.Where(c => c.TotalExpiredRequests.Equals(Convert.ToInt32(filterValue))),
    //            BO.CallInListFields.HandledRequestId => callsList.Where(c => c.HandledRequestId.Equals(Convert.ToInt32(filterValue))),
    //            BO.CallInListFields.TypeOfReading => callsList.Where(c => c.TypeOfReading.Equals((BO.TypeOfReading)filterValue)),
    //            _ => callsList
    //        };
    //    }

    //    // Sorting
    //    callsList = sortBy switch
    //    {
    //        BO.CallInListFields.CallId => callsList.OrderBy(c => c.CallId),
    //        BO.CallInListFields.FullName => callsList.OrderBy(c => c.FullName),
    //        BO.CallInListFields.IsActive => callsList.OrderBy(c => c.IsActive),
    //        BO.CallInListFields.TotalHandledRequests => callsList.OrderBy(c => c.TotalHandledRequests),
    //        BO.CallInListFields.TotalCanceledRequests => callsList.OrderBy(c => c.TotalCanceledRequests),
    //        BO.CallInListFields.TotalExpiredRequests => callsList.OrderBy(c => c.TotalExpiredRequests),
    //        BO.CallInListFields.HandledRequestId => callsList.OrderBy(c => c.HandledRequestId),
    //        BO.CallInListFields.TypeOfReading => callsList.OrderBy(c => c.TypeOfReading),
    //        _ => callsList.OrderBy(c => c.CallId)
    //    };

    //    return callsList;
    //}



    public IEnumerable<BO.CallInList> GetFilteredAndSortedCalls(
    BO.CallInListFields? filterBy = null, object? filterValue = null,
    BO.CallInListFields? sortBy = null)
    {
        var allCalls = _dal.Call.ReadAll().ToList();
        var allAssignments = _dal.Assignment.ReadAll().ToList();
        var allVolunteers = _dal.Volunteer.ReadAll().ToDictionary(v => v.Id, v => v.Name);

        var list = allCalls.Select(call =>
        {
            var latest = allAssignments.Where(a => a.CallId == call.Id)
                                       .OrderByDescending(a => a.EntryTimeForTreatment).FirstOrDefault();

            return new BO.CallInList
            {
                AssignmentId = latest?.Id,
                CallId = call.Id,
                TypeOfReading = (BO.TypeOfReading)call.TypeOfReading,
                OpeningTime = call.OpeningTime,
                RemainingTimeToEndCall = call.MaxTimeFinishRead.HasValue
                    ? (call.MaxTimeFinishRead > DateTime.Now ? call.MaxTimeFinishRead.Value - DateTime.Now : TimeSpan.Zero)
                    : null,
                LastVolunteerName = latest != null && allVolunteers.TryGetValue(latest.VolunteerId, out var name) ? name : null,
                TotalHandlingTime = latest?.EndOfTreatmentTime.HasValue == true
                    ? latest.EndOfTreatmentTime.Value - call.OpeningTime : null,
                Status = CallManager.GetCallStatus(call.Id),
                TotalAssignments = allAssignments.Count(a => a.CallId == call.Id)
            };
        });

        // Filtering
        if (filterBy != null && filterValue != null)
        {
            list = filterBy switch
            {

                BO.CallInListFields.AssignmentId => list.Where(c => c.AssignmentId.Equals(Convert.ToInt32(filterValue))),
                BO.CallInListFields.TypeOfReading => list.Where(c => c.TypeOfReading.Equals((BO.TypeOfReading)filterValue)),
                BO.CallInListFields.LastVolunteerName => list.Where(c => c.LastVolunteerName != null && c.LastVolunteerName.Contains(filterValue.ToString(), StringComparison.OrdinalIgnoreCase)),
                BO.CallInListFields.Status => list.Where(c => c.Status.Equals(filterValue.ToString())),
                BO.CallInListFields.OpeningTime => list.Where(c => c.OpeningTime.Equals(Convert.ToDateTime(filterValue))),
                BO.CallInListFields.TotalAssignments => list.Where(c => c.TotalAssignments.Equals(Convert.ToInt32(filterValue))),
                _ => list
            };
        }

        // Sorting
        list = sortBy switch
        {
            BO.CallInListFields.AssignmentId => list.OrderBy(c => c.AssignmentId),
            BO.CallInListFields.CallId => list.OrderBy(c => c.CallId),
            BO.CallInListFields.TypeOfReading => list.OrderBy(c => c.TypeOfReading),
            BO.CallInListFields.LastVolunteerName => list.OrderBy(c => c.LastVolunteerName),
            BO.CallInListFields.Status => list.OrderBy(c => c.Status),
            BO.CallInListFields.OpeningTime => list.OrderBy(c => c.OpeningTime),
            BO.CallInListFields.TotalAssignments => list.OrderBy(c => c.TotalAssignments),
            _ => list.OrderBy(c => c.CallId) 
        };

        return list;
    }

    #region Stage 5
    public void AddObserver(Action listObserver) =>
    CallManager.Observers.AddListObserver(listObserver); //stage 5
    public void AddObserver(int id, Action observer) =>
CallManager.Observers.AddObserver(id, observer); //stage 5
    public void RemoveObserver(Action listObserver) =>
CallManager.Observers.RemoveListObserver(listObserver); //stage 5
    public void RemoveObserver(int id, Action observer) =>
CallManager.Observers.RemoveObserver(id, observer); //stage 5
    #endregion Stage 5

}
