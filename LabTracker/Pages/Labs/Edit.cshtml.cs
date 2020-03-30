using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LabTracker.Models;

namespace LabTracker.Pages.Labs
{
    //The EditModel page allows the signed in Instructor to edit the details of a selected Lab.
    public class EditModel : PageModel
    {
        private readonly LabTrackerContext _context;

        public EditModel(LabTrackerContext context)
        {
            _context = context;
        }
        //BindProperty sets the Lab object in a strongly typed manner within the Razor content page for access.
        [BindProperty]
        public Lab Lab { get; set; }

        //Get the LabID currently selected
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

        //Change and update the Lab when user clicks "Save"
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Lab).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LabExists(Lab.LabID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool LabExists(int id)
        {
            return _context.Lab.Any(e => e.LabID == id);
        }
    }
}
