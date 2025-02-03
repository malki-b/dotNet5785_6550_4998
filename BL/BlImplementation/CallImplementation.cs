
namespace BlImplementation;
using BlApi;
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
            if ((CallManager.GetCallStatus(callId) == BO.Status.Open)&&(doCall.))
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
        //var calls = _dal.Call.GetAllCalls(); // Assuming you have a method to get all calls

        //// Group the calls by their status and count them
        //var callQuantities = calls.GroupBy(call => call.CallStatus)
        //                           .Select(group => new
        //                           {
        //                               Status = group.Key,
        //                               Count = group.Count()
        //                           })
        //                           .OrderBy(statusCount => statusCount.Status) // Optional: Order by status if needed
        //                           .ToArray();

        //// Initialize an array for counts, assuming Status is 0 to n
        //int[] quantities = new int[Enum.GetValues(typeof(Status)).Length];

        //foreach (var statusCount in callQuantities)
        //{
        //    quantities[(int)statusCount.Status] = statusCount.Count;
        //}

        //return quantities;
        throw new NotImplementedException();

    }

    public Call RequestCallDetails(int callId)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<CallInList> ReadAll(CallField? filterBy, object? filterValue, CallField? sortBy)
    {
     

        IEnumerable<DO.Call> calls = _dal.Call.ReadAll().ToList();

        IEnumerable<BO.CallInList> volunteersList = calls.Select(call =>
        {
            var volunteerCalls = _dal.Call.ReadAll(a => a.CallId == call.Id);

            int TotalHandledRequests = volunteercalls.Count(a => a.TypeOfEnding == DO.TypeOfEnding.Teated);
            int TotalCanceledRequests = volunteerAssignments.Count(a => a.TypeOfEnding == DO.TypeOfEnding.SelfCancellation || a.TypeOfEnding == DO.TypeOfEnding.ManagerCancellation);
            int TotalExpiredRequests = volunteerAssignments.Count(a => a.TypeOfEnding == DO.TypeOfEnding.CancellationHasExpired);

            var currentCallId = volunteerAssignments.FirstOrDefault(a => a.TypeOfEnding == null)?.CallId;

            BO.TypeOfReading callType = currentCallId.HasValue ? (BO.TypeOfReading)_dal.Call.Read(currentCallId.Value)!.TypeOfReading : (BO.TypeOfReading)DO.TypeOfReading.None;

            return new BO.CallInList
            {
                AssignmentId = call.volunteers,
                CallId = call.CallId,
                TypeOfReading = call.TypeOfReading,
                OpeningTime = call.OpeningTime,
                RemainingTimeToEndCall = call.RemainingTimeToEndCall,
                LastVolunteerName = call.LastVolunteerName,
                TotalHandlingTime = call.TotalHandlingTime,
                Status = call.Status,
                TotalAssignments = call.TotalAssignments
            };
        });


        if (sortBy == null)
        {
            return volunteersList.OrderBy(v => v.VolunteerId).ToList(); // Sort by Id if sortBy is null
        }

        // Sorting based on the specified enum value
        var propertyInfo = typeof(BO.VolunteerInList).GetProperty(sortBy.ToString());

        if (propertyInfo != null)
        {
            return volunteersList.OrderBy(v => propertyInfo.GetValue(v, null)).ToList();
        }



        return volunteersList;

    }

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
}
