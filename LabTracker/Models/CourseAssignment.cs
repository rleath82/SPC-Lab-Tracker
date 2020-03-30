using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabTracker.Models
{
    //The CourseAssignment model is a join table for the many-to-many relationship
    //between Course and Instructor. It is a pure join table without any payload.
    public class CourseAssignment
    {
        public int InstructorID { get; set; }
        public int CourseID { get; set; }
        public Instructor Instructor { get; set; }
        public Course Course { get; set; }
    }
}
