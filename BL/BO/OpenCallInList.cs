using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;

internal class OpenCallInList
{
    /// <summary>
    /// Represents the ID of the call.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Represents the type of the call.
    /// </summary>
    public CallType CallType { get; set; }

    /// <summary>
    /// Represents a description of the call.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Represents the full address related to the call.
    /// </summary>
    public string FullAddress { get; set; }

    /// <summary>
    /// Indicates the time when the call was opened.
    /// </summary>
    public DateTime OpeningTime { get; set; }

    /// <summary>
    /// Indicates the maximum end time for the call.
    /// </summary>
    public DateTime? MaxEndTime { get; set; }

    /// <summary>
    /// Represents the type of ending for the call.
    /// </summary>
    public TypeOfEnding? TypeOfEnding { get; set; }
}
