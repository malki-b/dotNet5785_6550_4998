using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;

public class CallInList
{
    //public int Id { get; init; }
    //public required string CourseNumber { get; set; }
    //public required string CourseName { get; set; }
    //public Year? InYear { get; init; }
    //public SemesterNames? InSemester { get; init; }
    //public override string ToString() => this.ToStringProperty();

    public int? Id { get; set; }
    public int CallId { get; set; }
    public CallType CallType { get; set; }
    public DateTime OpeningTime { get; set; }
    public TimeSpan? RemainingTimeToEndCall { get; set; }
    public string? LastVolunteerName { get; set; }
    public TimeSpan? TotalHandlingTime { get; set; }
    public Status Status { get; set; }
    public int TotalAssignments { get; set; }
}
