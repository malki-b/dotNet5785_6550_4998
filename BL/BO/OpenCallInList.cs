using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    internal class OpenCallInList
    {
        public int Id { get; set; }
        public CallType CallType { get; set; }
        public string? Description { get; set; }
        public string FullAddress { get; set; }
        public DateTime OpeningTime { get; set; }
        public DateTime? MaxEndTime { get; set; }
        public double VolunteerDistance { get; set; }
    }
}
