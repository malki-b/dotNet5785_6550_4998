using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;

public class VolunteerInList
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public bool IsActive { get; set; }
    public int TotalHandledRequests { get; set; }
    public int TotalCanceledRequests { get; set; }
    public int TotalExpiredRequests { get; set; }
    public int HandledRequestId { get; set; }
    public TypeOfReading TypeOfReading { get; set; }
}