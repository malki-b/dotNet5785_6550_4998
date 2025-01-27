
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Helpers;


namespace BO;
/// <summary>
/// 
/// </summary>
/// <param name="Id">Call entity running identifier</param>
/// <param name="CallType">according to the type of specific system (food preparation, food transportation, etc.)</param>
/// <param name="VerbalDescription">Description of the reading. Detailed details on the reading(default empty)</param>
/// <param name="FullAddress">Full address of the call</param>
/// <param name="Latitude">a number indicating how far a point on Earth is south or north of the equator</param>
/// <param name="Longitude">a number indicating how far a point on Earth is east or west of the equator</param>
/// <param name="OpeningTime">Time (date and time) when the call was opened by the manager</param>
/// <param name="MaxEndTime">Time (date and time) by which the reading should be closed(default empty)</param>
/// <param name="CallStatus">according to the type of specific system (food preparation, food transportation, etc.)</param>

public class Call
{

    public int Id { get; set; }
    public CallType CallType { get; set; }
    public string VerbalDescription { get; set; }
    public string FullAddress { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public DateTime OpeningTime { get; set; }
    public DateTime MaxEndTime { get; set; }
    public Status CallStatus { get; set; }
    public List<BO.CallAssignInList> CallAssignments { get; set; }
    public override string ToString() => this.ToStringProperty();

}


