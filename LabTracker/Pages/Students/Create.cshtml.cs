using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using LabTracker.Models;

namespace LabTracker.Pages.Students
{
    //The CreateModel uses the Student Model to create a new Student.
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
        //BindProperty sets the Student object in a strongly typed manner within the Razor content page for access.
        [BindProperty]
        public Student Student { get; set; }

        //Adds the newly created student to the database
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var emptyStudent = new Student();

            if (await TryUpdateModelAsync<Student>(
                emptyStudent,
                "student",      //Prefix for form value
                s => s.StudentID, s => s.FirstMidName, s => s.LastName))
            {
                _context.Student.Add(emptyStudent);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            return null;
        }
    }
}