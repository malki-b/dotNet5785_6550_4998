using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;


    public class ClosedCallInList
    {
        public int Id { get; set; }
        public CallType CallType { get; set; }
        public string FullAddress { get; set; }
        public DateTime OpeningTime { get; set; }
        public DateTime EntryTimeForHandling { get; set; }
        public DateTime? ActualHandlingEndTime { get; set; }
        public TypeOfEnding? TypeOfEnding { get; set; }
    }

