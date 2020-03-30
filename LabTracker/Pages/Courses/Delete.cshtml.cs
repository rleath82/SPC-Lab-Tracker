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
    //The DeleteModel allows the Instructor to remove a course from the database
    public class DeleteModel : PageModel
    {
        private readonly LabTrackerContext _context;

        public DeleteModel(LabTrackerContext context)
        {
            _context = context;
        }
        //BindProperty sets the Course object in a strongly typed manner within the Razor content page for access.
        [BindProperty]
        public Course Course { get; set; }
        
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //Find the CourseID that is to be deleted
            Course = await _context.Course.FirstOrDefaultAsync(m => m.CourseID == id);

            if (Course == null)
            {
                return NotFound();
            }
            return Page();
        }
       
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Course = await _context.Course.FindAsync(id);
            //If the Course object is not null, remove the course from the database and save changes.
            if (Course != null)
            {
                _context.Course.Remove(Course);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
