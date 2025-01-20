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
        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime OpeningTime { get; set; }
        public TypeOfReading TypeOfReading { get; set; } = TypeOfReading.FearOfHumanLife;
        public string? VerbalDescription { get; set; } = null;
        public DateTime? MaxTimeFinishRead { get; set; } = null;

        public override string ToString() => this.ToStringProperty();
    }
}