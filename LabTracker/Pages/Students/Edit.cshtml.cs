using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LabTracker.Models;

namespace LabTracker.Pages.Students
{
    //The EditModel allows the signed in Instructor to edit a student's first and last name.
    public class EditModel : PageModel
    {
        private readonly LabTrackerContext _context;

        public EditModel(LabTrackerContext context)
        {
            _context = context;
        }
        //BindProperty sets the Student object in a strongly typed manner within the Razor content page for access.
        [BindProperty]
        public Student Student { get; set; }

        //Get the selected student's StudentID
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Student = await _context.Student.FindAsync(id);

            if (Student == null)
            {
                return NotFound();
            }
            return Page();
        }

        //Update and save changes to the Student
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var studentToUpdate = await _context.Student.FindAsync(id);

            if (await TryUpdateModelAsync<Student>(
                studentToUpdate,
                "student",
                s => s.FirstMidName, s => s.LastName))
            {
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            return Page();
        }
    }
}
