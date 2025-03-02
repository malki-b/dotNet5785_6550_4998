using Helpers;
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
    /// Gets the unique identifier for the call entity.
    /// </summary>
    public int Id { get;  set; } // Not nullable

    /// <summary>
    /// Gets the type of the call.
    /// </summary>
    public TypeOfReading Type { get;  set; } // Not nullable

    /// <summary>
    /// Gets or sets the textual description of the call.
    /// This property can be null.
    /// </summary>
    public string? Description { get; set; } // Nullable

    /// <summary>
    /// Gets the full address of the call.
    /// </summary>
    public string FullAddress { get;  set; } // Not nullable

    /// <summary>
    /// Gets the opening time of the call.
    /// </summary>
    public DateTime OpeningTime { get;  set; } // Not nullable

    /// <summary>
    /// Gets or sets the maximum time for completing the call.
    /// This property can be null.
    /// </summary>
    public DateTime? MaxCompletionTime { get; set; } // Nullable

    /// <summary>
    /// Gets the distance of the call from the volunteer.
    /// This property is calculated in the business layer.
    /// </summary>
    public double DistanceFromVolunteer { get;  set; } // Not nullable

    public override string ToString() => this.ToStringProperty();
}