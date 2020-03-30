using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabTracker.Models.LabViewModels
{
    //InstructorIndexData model displays data from five different tables. 
    public class InstructorIndexData
    {
        public IEnumerable<Instructor> Instructors { get; set; }
        public IEnumerable<Course> Courses { get; set; }
        public IEnumerable<CourseEnrollment> CourseEnrollments { get; set; }
        public IEnumerable<LabEnrollment> LabEnrollments { get; set; }
        public IEnumerable<Lab> Labs { get; set; }
    }
}
