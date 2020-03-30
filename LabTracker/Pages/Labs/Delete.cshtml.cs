using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LabTracker.Models;

namespace LabTracker.Pages.Labs
{
    //The DeleteModel page allows the signed in Instructor to delete a Lab they created.
    public class DeleteModel : PageModel
    {
        private readonly LabTrackerContext _context;

        public DeleteModel(LabTrackerContext context)
        {
            _context = context;
        }
        //BindProperty sets the Lab object in a strongly typed manner within the Razor content page for access.
        [BindProperty]
        public Lab Lab { get; set; }
        
        //Get the LabID to be deleted
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Lab = await _context.Lab.FirstOrDefaultAsync(m => m.LabID == id);

            if (Lab == null)
            {
                return NotFound();
            }
            return Page();
        }

        //Remove the lab from the database and save changes
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Lab = await _context.Lab.FindAsync(id);

            if (Lab != null)
            {
                _context.Lab.Remove(Lab);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
