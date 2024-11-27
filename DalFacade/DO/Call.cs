
namespace DO;
/// <summary>
/// 
/// </summary>
/// <param name="Id">represents a number that identifies the call</param>
/// <param name="Address">full and real address in correct format, of the reading location</param>
/// <param name="Latitude">a number indicating how far a point on Earth is south or north of the equator</param>
/// <param name="Longitude">a number indicating how far a point on Earth is east or west of the equator</param>
/// <param name="OpeningTime">Time (date and time) when the call was opened by the manager</param>
/// <param name="VerbalDescription">Description of the reading. Detailed details on the reading(default empty)</param>
/// <param name="MaxTimeFinishRead">Time (date and time) by which the reading should be closed(default empty)</param>
/// <param name="TypeOfReading">according to the type of specific system (food preparation, food transportation, etc.)</param>
public record Call
(
    
    string Address,
    double Latitude,
    double Longitude,
    DateTime OpeningTime,
     string? VerbalDescription = null,
    DateTime? MaxTimeFinishRead = null,
    TypeOfReading TypeOfReading = TypeOfReading.FearOfHumanLife
)
{
 public int Id {  get; init; }
}