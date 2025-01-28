using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;

public class VolunteerInList
{
    /// <summary>
    /// Represents the ID of the volunteer.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Represents the full name of the volunteer (first and last name).
    /// </summary>
    public string FullName { get; set; }

    /// <summary>
    /// Indicates whether the volunteer is active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Represents the total number of requests handled by the volunteer.
    /// </summary>
    public int TotalHandledRequests { get; set; }

    /// <summary>
    /// Represents the total number of requests canceled by the volunteer.
    /// </summary>
    public int TotalCanceledRequests { get; set; }

    /// <summary>
    /// Represents the total number of requests that expired for the volunteer.
    /// </summary>
    public int TotalExpiredRequests { get; set; }

    /// <summary>
    /// Represents the ID of the request currently being handled by the volunteer, if applicable.
    /// </summary>
    public int? HandledRequestId { get; set; }

    /// <summary>
    /// Represents the type of the request being handled by the volunteer.
    /// </summary>
    public TypeOfReading TypeOfReading { get; set; }
}