using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LabTracker.Models;

namespace LabTracker.Pages.Courses
{
    //The DetailsModel shows the Course information (Prefix, Title), as well as the students
    //currently enrolled and the Instructor assigned.
    public class DetailsModel : PageModel
    {
        private readonly LabTrackerContext _context;

        public DetailsModel(LabTrackerContext context)
        {
            _context = context;
        }
        //BindProperty sets the InstructorID in a strongly typed manner within the Razor content page for access.
        [BindProperty]
        public int InstructorID { get; set; }
        //Create the Course object.
        public Course Course { get; set; }

        //The OnGetAsync method uses authentication to display the course information for the instructor currently
        //signed in.
        public async Task<IActionResult> OnGetAsync(int? id, int? instructorId)
        {
            //Create a DbSet of CourseAssignments
            DbSet<CourseAssignment> CourseAssignments = _context.CourseAssignment;
            //Use authentication to reference the Instructor currently signed in
            var claimsIdentity = User.Identity as System.Security.Claims.ClaimsIdentity;
            var c = claimsIdentity.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            //InstructorID is equal to the value of the claimsIdentity (Instructor signed in)
            InstructorID = int.Parse(c.Value);
            //Show all student enrollments for selected course, then show instructor assignments.
            Course = await _context.Course
                .Include(ce => ce.CourseEnrollments)
                    .ThenInclude(s => s.Student)
                .Include(i => i.CourseAssignments)
                    .ThenInclude(x => x.Instructor)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.CourseID == id);            
            //When the remove link is clicked, remove the CourseAssignment from the Instructor currently signed in.
            if (instructorId != null)
            {
                CourseAssignment instructorAssignment = Course.CourseAssignments.Where(x => x.CourseID == id.Value && x.InstructorID == instructorId).First();
                _context.CourseAssignment.Remove(instructorAssignment);
                await _context.SaveChangesAsync();
                Response.Redirect("/Courses/Details/" + id.ToString());
            }
            return Page();
        }
    }
}
