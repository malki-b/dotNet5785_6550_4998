
namespace DO;
/// <summary>
/// 
/// </summary>
/// <param name="Id">A number that uniquely identifies the allocation entity</param>
/// <param name="CallId">A number that identifies the call that the volunteer chose to handle</param>
/// <param name="VolunteerId">ID number of the volunteer who chose to take care of the reading</param>
/// <param name="EntryTimeForTreatment">Time (date and time) when the current call was processed</param>
/// <param name="EndOfTreatmentTime">Time (date and time) when the current volunteer finished handling the current call(default empty)</param>
/// <param name="TypeOfEnding">The manner in which the handling of the current call was completed by the current volunteer (default empty)</param>
public record Assignment
(
    int CallId,
    int VolunteerId,
    DateTime EntryTimeForTreatment,
    DateTime? EndOfTreatmentTime = null,
    TypeOfEnding? TypeOfEnding = TypeOfEnding.None
)
{
 public int Id { get; init; }
    /// <summary>
    /// Default constructor for Assignment
    /// </summary>
    public Assignment() : this( 0, 0, DateTime.Now) { }

}