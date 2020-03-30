using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LabTracker.Models;

namespace LabTracker.Pages.Instructors
{
    //The DeleteModel allows the instructor currently signed in to delete their profile
    //from the database
    public class DeleteModel : PageModel
    {
        private readonly LabTrackerContext _context;

        public DeleteModel(LabTrackerContext context)
        {
            _context = context;
        }
        //BindProperty sets the Instructor object in a strongly typed manner within the Razor content page for access.
        [BindProperty]
        public Instructor Instructor { get; set; }

        //Get the InstructorID to be deleted
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Instructor = await _context.Instructor.FirstOrDefaultAsync(m => m.InstructorID == id);

            if (Instructor == null)
            {
                return NotFound();
            }
            return Page();
        }

        //Confirm the deletion from the database and save changes
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Instructor = await _context.Instructor.FindAsync(id);

            if (Instructor != null)
            {
                _context.Instructor.Remove(Instructor);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
