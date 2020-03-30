using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LabTracker.Models;

namespace LabTracker.Pages.Students
{
    //The UnenrollCourseModel page confirms the removal of a course enrollment.
    public class UnenrollCourseModel : PageModel
    {
        private readonly LabTrackerContext _context;

        public UnenrollCourseModel(LabTrackerContext context)
        {
            _context = context;
        }
        //BindProperty sets the Student object in a strongly typed manner within the Razor content page for access.
        [BindProperty]
        public Student Student { get; set; }
        public string ErrorMessage { get; set; }
        public int CourseID { get; set; }

        //OnGetAsync gets the StudentID and CourseID for the enrollment removal.
        public async Task<IActionResult> OnGetAsync(int? id, int? courseId, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            Student = await _context.Student
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.StudentID == id);

            if (Student == null)
            {
                return NotFound();
            }

            return Page();
        }

        //The OnPostAsync method removes the CourseEnrollment
        public async Task<IActionResult> OnPostAsync(int? id, int courseId)
        {
            if (id == null)
            {
                return NotFound();
            }

            Student = await _context.Student.FindAsync(id);
            CourseEnrollment studentEnrollment = _context.CourseEnrollment
                .Where(ce => ce.StudentID == id && ce.CourseID == courseId).FirstOrDefault();
            _context.CourseEnrollment.Remove(studentEnrollment);
            //Save changes
            await _context.SaveChangesAsync();

            return RedirectToPage("./Details");
        }
    }
}
