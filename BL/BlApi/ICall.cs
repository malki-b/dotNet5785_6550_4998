using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi;

public interface ICall
{
    /// <summary>
    /// Method to request call counts based on their statuses.
    /// </summary>
    /// <returns>An array with counts of calls for each status.</returns>
    public int[] RequestCallCounts();

    /// <summary>
    /// Method to request a filtered and sorted list of calls.
    /// </summary>
    /// <param name="filterBy">The enum field to filter the list (nullable).</param>
    /// <param name="filterValue">The value to filter by (nullable).</param>
    /// <param name="sortBy">The enum field to sort the list (nullable).</param>
    /// <returns>A collection of CallInList entities.</returns>
    public IEnumerable<CallInList> RequestCallsList(CallField? filterBy, object? filterValue, CallField? sortBy);

    /// <summary>
    /// Method to request details of a specific call by its ID.
    /// </summary>
    /// <param name="callId">The ID of the call.</param>
    /// <returns>The Call entity with its assignments.</returns>
    /// <exception cref="CallNotFoundException"></exception>
    public Call RequestCallDetails(int callId);

    /// <summary>
    /// Method to update details of a specific call.
    /// </summary>
    /// <param name="call">The Call object with updated details.</param>
    /// <exception cref="CallNotFoundException"></exception>
    public void UpdateCallDetails(Call call);

    /// <summary>
    /// Method to delete a call by its ID.
    /// </summary>
    /// <param name="callId">The ID of the call to delete.</param>
    /// <exception cref="CallNotFoundException"></exception>
    /// <exception cref="CallCannotBeDeletedException"></exception>
    public void DeleteCall(int callId);

    /// <summary>
    /// Method to add a new call.
    /// </summary>
    /// <param name="call">The Call object to add.</param>
    public void AddCall(Call call);

    /// <summary>
    /// Method to request closed calls handled by a specific volunteer.
    /// </summary>
    /// <param name="volunteerId">The ID of the volunteer.</param>
    /// <param name="filterBy">Filter by call type (nullable).</param>
    /// <param name="sortBy">Sort the list by a specific field (nullable).</param>
    public IEnumerable<ClosedCallInList> RequestClosedCallsByVolunteer(int volunteerId, CallType? filterBy, ClosedCallField? sortBy);

    /// <summary>
    /// Method to request open calls available for selection by a volunteer.
    /// </summary>
    /// <param name="volunteerId">The ID of the volunteer.</param>
    /// <param name="filterBy">Filter by call type (nullable).</param>
    /// <param name="sortBy">Sort the list by a specific field (nullable).</param>
    public IEnumerable<OpenCallInList> RequestOpenCallsForSelection(int volunteerId, CallType? filterBy, OpenCallField? sortBy);

    /// <summary>
    /// Method to update the status of a call's treatment completion.
    /// </summary>
    /// <param name="volunteerId">The ID of the volunteer.</param>
    /// <param name="assignmentId">The ID of the assignment being marked as completed.</param>

    public void UpdateCallCompletion(int volunteerId, int assignmentId);

    /// <summary>
    /// Method to update the cancellation of treatment for a call.
    /// </summary>
    /// <param name="requesterId">The ID of the requester.</param>
    /// <param name="assignmentId">The ID of the assignment being canceled.</param>

    public void UpdateCallCancellation(int requesterId, int assignmentId);

    /// <summary>
    /// Method to select a call for treatment by a volunteer.
    /// </summary>
    /// <param name="volunteerId">The ID of the volunteer.</param>
    /// <param name="callId">The ID of the call to be treated.</param>
    public void SelectCallForTreatment(int volunteerId, int callId);
}
