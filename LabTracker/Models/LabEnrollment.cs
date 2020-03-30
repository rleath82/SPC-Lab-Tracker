using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LabTracker.Models
{
    //The CourseEnrollment model is a join table between Student and Lab.
    //It takes a DateTime stamp for when students sign in and out of labs.
    public class LabEnrollment
    {
        public int LabEnrollmentID { get; set; }
        public int LabID { get; set; }
        public int StudentID { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Login Date/Time")]
        public DateTime? LabSignIn { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Logout Date/Time")]
        public DateTime? LabSignOut { get; set; }

        //One lab and one student per enrollment
        public Lab Lab { get; set; }
        public Student Student { get; set; }
    }
}
