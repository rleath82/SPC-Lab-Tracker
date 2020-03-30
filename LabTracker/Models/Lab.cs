using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LabTracker.Models
{
    //The Lab model creates a new lab with required entries for Name, Campus, Building, Room
    //and Start/End dates/times
    public class Lab
    {
        public int LabID { get; set; }

        [Required]
        [Display(Name = "Lab Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Campus")]
        public string Campus { get; set; }
        
        [Required]
        [Display(Name = "Building")]
        public string Building { get; set; }

        [Required]
        [Display(Name = "Room")]
        public int Room { get; set; }

        [Required]
        [Display(Name = "Lab Start Date/Time")]
        public DateTime LabStart { get; set; }

        [Required]
        [Display(Name = "Lab End Date/Time")]
        public DateTime LabEnd { get; set; }

        //A Lab can have many LabEnrollments and many LabAssignments
        public ICollection<LabEnrollment> LabEnrollments { get; set; } 
        public ICollection<LabAssignment> LabAssignments { get; set; }
    }
}
