using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Onlinecourseattendance.Models
{
    public class UserMaking
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string LevelName { get; set; }
        public int Subject { get; set; }
        public int Rating { get; set; }
        public DateTime? EndDate { get; set; }
    }
}