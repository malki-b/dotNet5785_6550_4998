using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;


public class ClosedCallInList
{
    /// <summary>
    /// Represents the unique identifier for the closed call entity.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Represents the type of the call.
    /// </summary>
    public CallType CallType { get; set; }

    /// <summary>
    /// Represents the full address of the call.
    /// </summary>
    public string FullAddress { get; set; }

    /// <summary>
    /// Indicates the time when the call was opened.
    /// </summary>
    public DateTime OpeningTime { get; set; }

    /// <summary>
    /// Indicates the entry time for handling the call.
    /// </summary>
    public DateTime EntryTimeForHandling { get; set; }

    /// <summary>
    /// Indicates the actual end time of the handling.
    /// </summary>
    public DateTime? ActualHandlingEndTime { get; set; }

    /// <summary>
    /// Represents the type of ending for the call.
    /// </summary>
    public TypeOfEnding? TypeOfEnding { get; set; }
}

