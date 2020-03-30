using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LabTracker.Models;

namespace LabTracker.Pages.Instructors
{
    //The EditModel page allows the currently signed in Instructor to change their profile information,
    //such as their email, First/Last names, and password.
    public class EditModel : PageModel
    {
        private readonly LabTrackerContext _context;

        public EditModel(LabTrackerContext context)
        {
            _context = context;
        }
        //BindProperty sets the Instructor object in a strongly typed manner within the Razor content page for access.
        [BindProperty]
        public Instructor Instructor { get; set; }

        //Get the InstructorID of the current instructor signed in
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
            //Use authentication to identify the current instructor signed in.
            var claimsIdentity = User.Identity as System.Security.Claims.ClaimsIdentity;
            if(claimsIdentity != null)
            {
                var c = claimsIdentity.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

                if (c == null || !c.Value.ToString().Equals(Instructor.InstructorID.ToString()))
                {
                    return NotFound();
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()        
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            //Update the information for the instructor
            _context.Attach(Instructor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InstructorExists(Instructor.InstructorID))
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

        private bool InstructorExists(int id)
        {
            return _context.Instructor.Any(e => e.InstructorID == id);
        }
    }
}
