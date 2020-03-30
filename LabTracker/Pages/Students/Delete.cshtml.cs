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
    //The DeleteModel page allows the signed in Instructor to remove a student from the database.
    public class DeleteModel : PageModel
    {
        private readonly LabTrackerContext _context;

        public DeleteModel(LabTrackerContext context)
        {
            _context = context;
        }
        //BindProperty sets the Student object in a strongly typed manner within the Razor content page for access.
        [BindProperty]
        public Student Student { get; set; }
        public string ErrorMessage { get; set; }

        //Get the StudentID to be removed.
        public async Task<IActionResult> OnGetAsync(int? id, bool? saveChangesError = false)
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

            if (saveChangesError.GetValueOrDefault())
            {
                ErrorMessage = "Delete failed. Try again.";
            }

            return Page();
        }

        //Remove the student from the database.
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                            .AsNoTracking()
                            .FirstOrDefaultAsync(m => m.StudentID == id);

            if (student == null)
            {
                return NotFound();
            }
            //try statement to remove the student and save changes.
            try
            {
                _context.Student.Remove(student);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction("./Delete",
                                     new { id, saveChangesError = true });
            }
        }
    }
}
