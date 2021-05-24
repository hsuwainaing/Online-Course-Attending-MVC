using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Onlinecourseattendance.Models
{
    public class UserInfo
    {
        public int Id { get; set; }
        public int UserID { get; set; }
        public int PhoneNo { get; set; }
        public string Address { get; set; }

        public string NRICNo { get; set; }
        public string PassportNo { get; set; }
        public string Gender { get; set; }
        public string Country { get; set; }
        public Boolean IsTeacher { get; set; }
        public DateTime? ProcessingDate { get; set; }
    }
}