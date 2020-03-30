using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LabTracker.Models
{
    //The Student model is used to create a new student. The DatabaseGenerated attribute
    //allows StudentID to be the primary key for the student.
    public class Student
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        [Display(Name = "Student Number")]
        public int StudentID { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
        [Display(Name = "First Name")]
        public string FirstMidName { get; set; }

        [Display(Name = "Full Name")]
        public string FullName
        {
            get
            {
                return LastName + ", " + FirstMidName;
            }
        }

        //Each student can have multiple CourseEnrollments and multiple LabEnrollments
        public ICollection<CourseEnrollment> CourseEnrollments { get; set; }
        public ICollection<LabEnrollment> LabEnrollments { get; set; }
    }
}
