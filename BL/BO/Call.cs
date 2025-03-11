
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Helpers;


namespace BO;
/// <summary>
/// Represents a call entity in the system.
/// </summary>
public class Call
{

    //private readonly DalApi.IDal _dal = DalApi.Factory.Get;
    /// <summary>
    /// Represents a number that identifies the call.
    /// </summary>
    public int CallId { get; init; }

    /// <summary>
    /// Represents the type of call.
    /// </summary>
    public TypeOfReading TypeOfReading { get; set; }

    /// <summary>
    /// Provides a description of the call.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Contains the full address of the call location.
    /// </summary>
    public string Address { get; set; }

    /// <summary>
    /// Represents the latitude of the call location.
    /// </summary>
    public double Latitude { get; set; }

    /// <summary>
    /// Represents the longitude of the call location.
    /// </summary>
    public double Longitude { get; set; }

    /// <summary>
    /// Indicates the opening time of the call.
    /// </summary>
    public DateTime OpeningTime { get; set; }

    /// <summary>
    /// Indicates the maximum allowed end time for the call.
    /// </summary>
    public DateTime? MaxEndTime { get; set; }

    /// <summary>
    /// Represents the current status of the call.
    /// </summary>
    public Status CallStatus { get; set; }

    /// <summary>
    /// Represents a list of assignments related to the call.
    /// </summary>
    public List<BO.CallAssignInList> CallAssignments { get; set; }//?

    public override string ToString() => this.ToStringProperty();
}