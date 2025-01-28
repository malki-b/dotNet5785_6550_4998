using Helpers;
namespace BO;

public class CallInProgress
{
    /// <summary>
    /// Represents the unique identifier for the allocation entity.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Represents the unique identifier for the related call entity.
    /// </summary>
    public int CallId { get; set; }

    /// <summary>
    /// Represents the type of the call associated with the allocation.
    /// </summary>
    public CallType CallType { get; set; }

    /// <summary>
    /// Represents a descriptive text regarding the allocation.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Represents the full address of the call.
    /// </summary>
    public string FullAddress { get; set; }

    /// <summary>
    /// Indicates the time when the call was opened.
    /// </summary>
    public DateTime OpeningTime { get; set; }

    /// <summary>
    /// Indicates the maximum time allowed for completing the call.
    /// </summary>
    public DateTime MaxCompletionTime { get; set; }

    /// <summary>
    /// Indicates the entry time for handling the call.
    /// </summary>
    public DateTime EntryTimeForHandling { get; set; }

    /// <summary>
    /// Represents the distance of the call from the handling volunteer.
    /// </summary>
    public double DistanceFromVolunteer { get; set; }

    /// <summary>
    /// Represents the status of the allocation.
    /// </summary>
    public Status Status { get; set; }

    public override string ToString() => this.ToStringProperty();
}