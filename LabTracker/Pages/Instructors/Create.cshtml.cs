using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using LabTracker.Models;

namespace LabTracker.Pages.Instructors
{
    //The CreateModel uses the Instructor model to create a new instructor. This is the "Register as new user" page 
    //from the sign in.
    public class CreateModel : PageModel
    {
        private readonly LabTrackerContext _context;

        public CreateModel(LabTrackerContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }
        //BindProperty sets the Instructor object in a strongly typed manner within the Razor content page for access.
        [BindProperty]
        public Instructor Instructor { get; set; }

        //Add the Instructor to database and save changes
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Instructor.Add(Instructor);
            await _context.SaveChangesAsync();

            return RedirectToPage("../Index");
        }
    }
}