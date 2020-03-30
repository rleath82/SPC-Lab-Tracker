using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabTracker.Models
{
    //The LabAssignment model is a join table for the many-to-many relationship
    //between Lab and Instructor. It is a pure join table without any payload.
    public class LabAssignment
    {
        public int InstructorID { get; set; }
        public int LabID { get; set; }
        public Instructor Instructor { get; set; }
        public Lab Lab { get; set; }
    }
}