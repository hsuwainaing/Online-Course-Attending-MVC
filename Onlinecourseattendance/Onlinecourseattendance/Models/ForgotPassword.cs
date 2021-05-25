using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Onlinecourseattendance.Models
{
    public class ForgotPassword
    {
        public int ID { get; set; }
        public int UserId { get; set; }
        public int Token { get; set; }
        public DateTime? ProcessingTime { get; set; }
        
        public int EntityId
        {
            get { return ID; }
            set { value = ID; }
        }

    }
}