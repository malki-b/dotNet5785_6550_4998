
namespace BlImplementation;
using BlApi;
using BO;
using System.Collections.Generic;

internal class CallImplementation : ICall
{
    private readonly DalApi.IDal _dal = DalApi.Factory.Get;
    public void Create(Call call)
    {
        throw new NotImplementedException();
    }

    public void Delete(int callId)
    {
        throw new NotImplementedException();
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

    public IEnumerable<CallInList> RequestCallsList(CallField? filterBy, object? filterValue, CallField? sortBy)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<ClosedCallInList> RequestClosedCallsByVolunteer(int volunteerId, CallType? filterBy, CallField? sortBy)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<OpenCallInList> RequestOpenCallsForSelection(int volunteerId, CallType? filterBy, CallField? sortBy)
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
