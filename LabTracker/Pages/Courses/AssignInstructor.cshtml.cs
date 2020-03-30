using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LabTracker.Models;

namespace LabTracker.Pages.Courses
{
    //The AssignInstructor page uses the CourseAssignment Model to assign courses to instructors.
    public class AssignInstructorModel : PageModel
    {
        private readonly LabTrackerContext _context;
        //Create Instructor object
        public Instructor Instructor { get; set; }
        //Create a SelectList CourseNames to generate a list of courses
        public SelectList CourseNames { get; set; }

        //BindProperty sets the CourseIDs in a strongly typed manner within the Razor content page for access.
        [BindProperty]
        public int[] CourseIDs { get; set; }

        public AssignInstructorModel(LabTrackerContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            //Retrieve the InstructorID to assign courses
            Instructor = await _context.Instructor.FirstOrDefaultAsync(i => i.InstructorID == id);

            if (Instructor == null)
            {
                return NotFound();
            }
            //Call the GenerateSelectList method with the InstructorID parameter. This list will only display the 
            //courses that have not yet been assigned to the Instructor who is signed in.
            CourseNames = GenerateSelectList(id);

            return Page();
        }

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

            Instructor = await _context.Instructor.FindAsync(id);
            //if the instructor has not selected a course and clicks the "Assign" button, display an error message.
            if(CourseIDs.Length == 0)
            {
                ModelState.AddModelError("No Course Selected", "You must select at least one course.");
                return Page();
            }
            //Loop to add all the courses selected by the Instructor for assignment.
            foreach(int CourseId in CourseIDs)
            {
                CourseAssignment instructorAssignment = new CourseAssignment { InstructorID = Instructor.InstructorID, CourseID = CourseId };
                _context.CourseAssignment.Add(instructorAssignment);
            }
            //Save changes
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        //The GenerateSelectList method uses Authentication to display a list of courses that have not
        //been assigned to the InstructorID parameter passed to it.
        private SelectList GenerateSelectList(int? id) 
        {
            //create a DbSet of courseAssignments
            DbSet<CourseAssignment> courseAssignments = _context.CourseAssignment;
            //Use authentication to reference the Instructor currently signed in
            var claimsIdentity = User.Identity as System.Security.Claims.ClaimsIdentity;
            var c = claimsIdentity.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            //Create two lists: coursesAssigned holds the courses already assigned to the Instructor signed in
            //coursesNotAssigned holds the courses that have not been assigned yet
            List<Course> coursesAssigned = new List<Course>();
            List<Course> coursesNotAssigned = new List<Course>();
            //Loop through the courseAssignments
            foreach (CourseAssignment assignedInstructor in courseAssignments)
            {
                //if the InstructorID is already assigned to a course, put that course in the coursesAssigned list
                if (assignedInstructor.InstructorID == id)
                {
                    Course Course = _context.Course.Where(ca => ca.CourseID == assignedInstructor.CourseID).First();
                    coursesAssigned.Add(Course);
                }

            }

            //Get all of the courses WHERE the Instructor is not assigned and add to the coursesNotAssigned list.
            coursesNotAssigned = _context.Course.Where(l => !coursesAssigned.Any(l2 => l2.CourseID == l.CourseID)).OrderBy(x => x.Title).ToList();
            return new SelectList(coursesNotAssigned, "CourseID", "Title");
        }
    }
}
