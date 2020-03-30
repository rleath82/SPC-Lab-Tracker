using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LabTracker.Models
{
    //The Instructor model creates a single instructor that will be able to create/assign
    //courses, labs, and students when logged in.
    public class Instructor
    {
        public int InstructorID { get; set; }  

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstMidName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation passwords do not match.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Full Name")]
        public string FullName
        {
            get
            {
                return "Professor " + LastName;
            }
        }

        //An Instructor can have many LabAssignments and many CourseAssignments
        public ICollection<LabAssignment> LabAssignments { get; set; }
        public ICollection<CourseAssignment> CourseAssignments { get; set; }
    }
}
