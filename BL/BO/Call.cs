using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Helpers;

namespace BO
{
    public class Call
    {
        public int Id { get; set; }
        public CallType CallType { get; set; }
        public string Description { get; set; }
        public string FullAddress { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime OpeningTime { get; set; }
        public DateTime MaxEndTime { get; set; }
        public Status CallStatus { get; set; }
        public List<BO.CallAssignInList> CallAssignments { get; set; }
        public override string ToString() => this.ToStringProperty();

    }

}
}