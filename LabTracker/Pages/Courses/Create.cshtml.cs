using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using LabTracker.Models;

namespace LabTracker.Pages.Courses
{
    //The CreateModel uses the Course Model to create a new course.
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
        //BindProperty sets the Course object in a strongly typed manner within the Razor content page for access.
        [BindProperty]
        public Course Course { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            //Add the newly created course and save changes
            _context.Course.Add(Course);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}