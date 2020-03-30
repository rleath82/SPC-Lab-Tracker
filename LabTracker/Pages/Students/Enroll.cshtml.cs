using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LabTracker.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using System.Linq;

namespace LabTracker.Pages.Students
{
    //The EnrollModel allows the instructor to enroll a selected student in a number of courses. The courses must
    //be assigned to the signed in instructor first, however.
    public class EnrollModel : PageModel
    {
        private readonly LabTrackerContext _context;
        public Student Student { get; set; }
        //Create a SelectList CourseNames for all of the courses to list for selection.
        public SelectList CourseNames { get; set; }
        //BindProperty sets the CourseIDs in a strongly typed manner within the Razor content page for access.
        [BindProperty]
        public int[] CourseIDs { get; set; }

        public EnrollModel(LabTrackerContext context)
        {
            _context = context;
        }

        //Get the StudentID for the CourseEnrollments
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Student = await _context.Student.FirstOrDefaultAsync(s => s.StudentID == id);

            if (Student == null)
            {
                return NotFound();
            }
            //Call the GenerateSelectList method to display the courses not yet enrolled in.
            CourseNames = GenerateSelectList(id);
            return Page();
        }

        //Enrolls selected student in courses
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            Student = await _context.Student.FindAsync(id);
            //If no courses are selected, throw an error message.
            if (CourseIDs.Length == 0)
            {
                ModelState.AddModelError("No Course Selected", "You must select at least one course to enroll this student in");
                return Page();
            }
            //Loop through the CourseIds and add the enrollments to the selected student.
            foreach (int CourseId in CourseIDs)
            {
                CourseEnrollment studentEnrollment = new CourseEnrollment { StudentID = Student.StudentID, CourseID = CourseId };
                _context.CourseEnrollment.Add(studentEnrollment);
            }
            //Save changes
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        //GenerateSelectList method creates DbSets of studentEnrollments and instructorAssignments.
        //Uses authentication to display only the courses assigned to the signed in instructor.
        //Then creates coursesNotEnrolled and courseEnrolled lists.
        private SelectList GenerateSelectList(int? id)
        {
            DbSet<CourseEnrollment> studentEnrollments = _context.CourseEnrollment;
            DbSet<CourseAssignment> instructorAssignments = _context.CourseAssignment;
            //Use authentication
            var claimsIdentity = User.Identity as System.Security.Claims.ClaimsIdentity;
            var c = claimsIdentity.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

            //Create lists.
            List<Course> courseNotEnrolled = new List<Course>();
            List<Course> courseEnrolled = new List<Course>();

            //Loop through the studentEnrollments and if the student is already enrolled in a course, add that course to the 
            //courseEnrolled list.
            foreach (CourseEnrollment enrolledStudent in studentEnrollments)
            {
                if (enrolledStudent.StudentID == id)
                {
                    Course course= _context.Course.Where(x => x.CourseID == enrolledStudent.CourseID).First();
                    courseEnrolled.Add(course);
                }
            }
            //Add courses not yet enrolled in where the instructor is currently assigned to the courseNotEnrolled list.
            courseNotEnrolled = _context.Course.Where(l => !courseEnrolled.Any(l2 => l2.CourseID == l.CourseID)).Where(l => instructorAssignments.Any(l2 => l2.InstructorID == int.Parse(c.Value) && l2.CourseID == l.CourseID)).ToList();

            return new SelectList(courseNotEnrolled.OrderBy(x => x.Title), "CourseID", "Title");
        }
    }
}
