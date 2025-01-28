
namespace BlImplementation;
using BlApi;
using BO;
using System.Collections.Generic;

internal class CallImplementation : ICall
{
    private readonly DalApi.IDal _dal = DalApi.Factory.Get;
    public void AddCall(Call call)
    {
        throw new NotImplementedException();
    }

    public void DeleteCall(int callId)
    {
        throw new NotImplementedException();
    }

    public int[] RequestCallCounts()
    {
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
