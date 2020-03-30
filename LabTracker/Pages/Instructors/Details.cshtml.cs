using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LabTracker.Models;
using LabTracker.Models.LabViewModels;  //add VM

namespace LabTracker.Pages.Instructors
{
    //The DetailsModel will display the Courses assigned to the Instructor signed in, as well
    //as the students enrolled in each course (with removal options). It also displays the labs created by the signed in
    //Instructor, with the enrolled students in each lab.
    public class DetailsModel : PageModel
    {
        private readonly LabTrackerContext _context;

        public DetailsModel(LabTrackerContext context)
        {
            _context = context;
        }
        //Use the InstructorIndexData model to display the information for the currently signed in instructor
        public InstructorIndexData Instructor { get; set; }
        //Set properties for InstructorID, CourseID, and LabID
        public int InstructorID { get; set; }
        public int CourseID { get; set; }
        public int LabID { get; set; }

        //OnGetAsync method accepts optional route data for the ID of the selected instructor.
        public async Task OnGetAsync(int? id, int? courseID, int? labID, int? removeCourseID, int? removeEnrollmentStudent, int? removeEnrollmentCourse)
        {
            Instructor = new InstructorIndexData();
            //Create a DbSet of CourseAssignments
            DbSet<CourseAssignment> CourseAssignments = _context.CourseAssignment;
            //Use authentication to select the currently signed in Instructor
            var claimsIdentity = User.Identity as System.Security.Claims.ClaimsIdentity;
            var c = claimsIdentity.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            //Show the CourseAssignments (Courses) and LabAssignments (Labs) for the Instructor signed in
            Instructor.Instructors = await _context.Instructor.Where(x => x.CourseAssignments.Any(x2 => x2.InstructorID == int.Parse(c.Value) && x2.InstructorID == x.InstructorID))
                    .Include(i => i.CourseAssignments) //which brings in the courses taught
                        .ThenInclude(i => i.Course)
                    .Include(l => l.LabAssignments)
                        .ThenInclude(l => l.Lab)                    
                    .OrderBy(i => i.LastName)
                    .ToListAsync();

            
            //When an InstructorID is present, display the information for the CourseAssignments and LabAssignments.
            if (id != null)
            {
                InstructorID = id.Value;
                Instructor instructor = Instructor.Instructors.Where(
                    i => i.InstructorID == id.Value).Single();
                Instructor.Courses = instructor.CourseAssignments.Select(s => s.Course).OrderBy(s => s.Title);
                Instructor.Labs = instructor.LabAssignments.Select(l => l.Lab);
            }
            //The following code populates the view's model's CourseEnrollments property when a course is selected.
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

            //The following code populates the view's model's LabEnrollments property when a lab is selected.
            if (labID != null)
            {
                labID = labID.Value;
                var selectedLab = Instructor.Labs.Where(x => x.LabID == labID).Single();
                await _context.Entry(selectedLab).Collection(x => x.LabEnrollments).LoadAsync();
                foreach(LabEnrollment enrollment in selectedLab.LabEnrollments)
                {
                    await _context.Entry(enrollment).Reference(x => x.Student).LoadAsync();
                }
                Instructor.LabEnrollments = selectedLab.LabEnrollments;
            }
            //When the user selects to remove a courseAssignment, remove the assignment from the database.
            if(removeCourseID != null)
            {
                CourseAssignment courseAssignment = await _context.CourseAssignment.Where(x => x.InstructorID == InstructorID && x.CourseID == removeCourseID).FirstOrDefaultAsync();
                _context.CourseAssignment.Remove(courseAssignment);
                await _context.SaveChangesAsync();
            }
            //When the user selects to remove a courseEnrollment, remove the enrollment from the database.
            if(removeEnrollmentStudent != null && removeEnrollmentCourse != null)
            {
                CourseEnrollment courseEnrollment = await _context.CourseEnrollment.Where(x => x.StudentID == removeEnrollmentStudent && x.CourseID == removeEnrollmentCourse).FirstOrDefaultAsync();
                _context.CourseEnrollment.Remove(courseEnrollment);
                await _context.SaveChangesAsync();
                Response.Redirect("/Instructors/Details/" + InstructorID + "?courseID=" + removeEnrollmentCourse.ToString());
            }
        }
    }
}
