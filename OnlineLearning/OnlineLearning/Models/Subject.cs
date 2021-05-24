using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Onlinecourseattendance.Models
{
    public class Subject
    {
        public int SubjectId { get; set; }
        public string LevelName { get; set; }

        public string Name { get; set; }
        public DateTime? ProcessingDate { get; set; }
        public DateTime? DurationTime { get; set; }
    }
}