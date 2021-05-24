using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Onlinecourseattendance.Models
{
    public class Lecture
    {
        public int LectureId { get; set; }
        public int SubjectId { get; set; }
        public int UserId { get; set; }
        public DateTime? UploadingDate { get; set; }
        public byte[] LectureFormatInVideo { get; set; }
        public byte[] LectureFormatInAudio { get; set; }
        public byte[] LectureFormatInWord { get; set; }
        public string UrlName { get; set; }
    }
}