using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LabTracker.Models;
using LabTracker.Models.LabViewModels;

namespace LabTracker.Pages
{
    //The DashboardModel displays all of the Instructors in the database, what courses they are assigned to, 
    //the students enrolled in those courses, the labs assigned to each instructor, and the students enrolled in
    //those labs.
    public class DashboardModel : PageModel
    {
        private readonly LabTrackerContext _context;

        public DashboardModel(LabTrackerContext context)
        {
            _context = context;
        }
        //Use the InstructorIndexData model to display the information for the instructors
        public InstructorIndexData Instructor { get; set; }

        //Set properties
        public int InstructorID { get; set; }
        public int CourseID { get; set; }
        public int LabID { get; set; }
        public int StudentID { get; set; }

        //The OnGetAsync method lists the instructors and allows to select each to show their information.
        public async Task OnGetAsync(int? id, int? courseID, int? labID, int? studentID)
        {
            Instructor = new InstructorIndexData();
            //Show the instructors and their LabAssignments, CourseAssignments, and Students enrolled.
            Instructor.Instructors = await _context.Instructor
                .Include(i => i.LabAssignments)
                    .ThenInclude(i => i.Lab)
                .Include(i => i.CourseAssignments)
                    .ThenInclude(i => i.Course)
                .OrderBy(i => i.LastName)
                .ToListAsync();

            //List the Instructors with their labs and courses.
            if (id != null)
            {
                InstructorID = id.Value;
                Instructor instructor = Instructor.Instructors.Where(
                    i => i.InstructorID == id.Value).Single();
                Instructor.Courses = instructor.CourseAssignments.Select(s => s.Course).OrderBy(s => s.Title);
                Instructor.Labs = instructor.LabAssignments.Select(l => l.Lab);
            }
            //When a course is selected, show the students enrolled in that course.
            if (courseID != null)
            {
                courseID = courseID.Value;
                var selectedCourse = Instructor.Courses.Where(x => x.CourseID == courseID).Single();
                await _context.Entry(selectedCourse).Collection(x => x.CourseEnrollments).LoadAsync();
                foreach (CourseEnrollment enrollment in selectedCourse.CourseEnrollments)
                {
                    await _context.Entry(enrollment).Reference(x => x.Student).LoadAsync();
                }
                Instructor.CourseEnrollments = selectedCourse.CourseEnrollments;
            }

            //When a lab is selected, show the students enrolled in that lab with their sign in and out information.
            if (labID != null)
            {
                labID = labID.Value;
                var selectedLab = Instructor.Labs.Where(x => x.LabID == labID).Single();
                await _context.Entry(selectedLab).Collection(x => x.LabEnrollments).LoadAsync();
                foreach (LabEnrollment enrollment in selectedLab.LabEnrollments)
                {
                    await _context.Entry(enrollment).Reference(x => x.Student).LoadAsync();
                }
                Instructor.LabEnrollments = selectedLab.LabEnrollments;
            }
        }
    }
}