using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;

/// <summary>
/// Represents a call assignment in the list.
/// </summary>
public class CallInList
{
    /// <summary>
    /// Represents the identifier of the assignment entity.
    /// </summary>
    public int? AssignmentId { get; set; }

    /// <summary>
    /// Represents the unique identifier of the related call.
    /// </summary>
    public int CallId { get; set; }

    /// <summary>
    /// Represents the type of the call.
    /// </summary>
    public CallType CallType { get; set; }

    /// <summary>
    /// Indicates the opening time of the call.
    /// </summary>
    public DateTime OpeningTime { get; set; }

    /// <summary>
    /// Represents the total remaining time until the call ends.
    /// </summary>
    public TimeSpan? RemainingTimeToEndCall { get; set; }

    /// <summary>
    /// Represents the name of the last volunteer assigned.
    /// </summary>
    public string? LastVolunteerName { get; set; }

    /// <summary>
    /// Represents the total time taken to handle the call.
    /// </summary>
    public TimeSpan? TotalHandlingTime { get; set; }

    /// <summary>
    /// Represents the current status of the call.
    /// </summary>
    public Status Status { get; set; }

    /// <summary>
    /// Represents the total number of assignments related to the call.
    /// </summary>
    public int TotalAssignments { get; set; }
}
