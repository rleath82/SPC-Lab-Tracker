using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LabTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace LabTracker.Pages.Courses
{
    //The UnenrollStudentModel, when confirmed from the user, will remove the selected student enrollment from the course.
    public class UnenrollStudentModel : PageModel
    {
        private readonly LabTrackerContext _context;

        public UnenrollStudentModel(LabTrackerContext context)
        {
            _context = context;
        }
        //BindProperty sets the Course object in a strongly typed manner within the Razor content page for access.
        [BindProperty]
        public Course Course { get; set; }
        //Create an error message and StudentID property
        public string ErrorMessage { get; set; }
        public int StudentID { get; set; }
        //Get the StudentID to be removed from enrollment.
        public async Task<IActionResult> OnGetAsync(int? id, int? studentId, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            Course = await _context.Course
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CourseID == id);

            if (Course == null)
            {
                return NotFound();
            }

            return Page();
        }
        //When the user confirms the removal, remove the CourseEnrollment
        public async Task<IActionResult> OnPostAsync(int? id, int studentId)
        {
            if (id == null)
            {
                return NotFound();
            }

            Course = await _context.Course.FindAsync(id);
            CourseEnrollment studentEnrollment = _context.CourseEnrollment
                .Where(se => se.CourseID == id && se.StudentID == studentId).FirstOrDefault();
            _context.CourseEnrollment.Remove(studentEnrollment);
            //Save changes
            await _context.SaveChangesAsync();

            return RedirectToPage("./Details");
        }
    }
}