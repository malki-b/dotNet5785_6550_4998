using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;

/// <summary>
/// Represents an assignment of a volunteer to a call.
/// </summary>
public class CallAssignInList
{
    /// <summary>
    /// Represents the ID of the volunteer.
    /// </summary>
    public int? VolunteerId { get; set; }

    /// <summary>
    /// Represents the name of the volunteer.
    /// </summary>
    public string? VolunteerName { get; set; }

    /// <summary>
    /// Indicates the entry time for treatment.
    /// </summary>
    public DateTime EntryTimeForTreatment { get; set; }

    /// <summary>
    /// Indicates the actual end time of the treatment.
    /// </summary>
    public DateTime? EndOfTreatmentTime { get; set; }

    /// <summary>
    /// Represents the type of ending for the treatment.
    /// </summary>
    public TypeOfEnding? TypeOfEnding { get; set; }
}  