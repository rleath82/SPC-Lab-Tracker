using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LabTracker.Models
{
    //The CourseEnrollment model is a join table between Student and Course.
    //It is a pure join table with no payload.
    public class CourseEnrollment
    {
        public int CourseEnrollmentID { get; set; } 
        public int StudentID { get; set; }
        public int CourseID { get; set; }

        public Student Student { get; set; }
        public Course Course { get; set; }
    }
}
