using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LabTracker.Models
{
    //The Course model automatically creates a primary key "CourseID"
    //The user, when creating a new course, will be required to input the course
    //prefix and course title.
    public class Course
    {
        public int CourseID { get; set; } 

        [Required]
        [Display(Name = "Course Prefix")]
        public string CourseNum { get; set; }

        [Required]
        [Display(Name = "Course Title")]
        [StringLength(60, MinimumLength = 3)]
        public string Title { get; set; }

        //A course can be related to any number of CourseEnrollments and CourseAssignments
        public ICollection<CourseEnrollment> CourseEnrollments { get; set; }        
        public ICollection<CourseAssignment> CourseAssignments { get; set; }
    }
}
