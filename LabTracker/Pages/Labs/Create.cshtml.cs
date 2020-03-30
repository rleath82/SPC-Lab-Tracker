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
    //The CreateModel uses the Lab model to create a new lab.
    public class CreateModel : PageModel
    {
        private readonly LabTrackerContext _context;
        //create an instructor object to be assigned to the newly created lab.
        public Instructor Instructor { get; set; }

        public CreateModel(LabTrackerContext context)
        {
            _context = context;
        }
        //Get the InstructorID to be assigned to the lab
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Instructor = await _context.Instructor.FirstOrDefaultAsync(i => i.InstructorID == id);

            if (Instructor == null)
            {
                return NotFound();
            }

            return Page();
        }
        //BindProperty sets the Lab object in a strongly typed manner within the Razor content page for access.
        [BindProperty]
        public Lab Lab { get; set; }

        //OnPostAsync method uses authentication to assign a lab to the signed in Instructor
        public async Task<IActionResult> OnPostAsync()
        {
            //If the LabStart time is greater than or equal to the LabEnd Time, throw an error message.
            if (Lab.LabStart >= Lab.LabEnd)
            {
                ModelState.AddModelError("Lab EndDate Error", "The Start Date/Time must be less than the End Date/Time");
            }
            //if the LabStart/LabEnd time is less than the DateTime.Now, throw an error.
            if (Lab.LabStart < DateTime.Now || Lab.LabEnd < DateTime.Now)
            {
                ModelState.AddModelError("Past Date Error", "The Lab Start and End Date/Time must occur in the future");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }
            //Use authentication to find the currently signed in Instructor
            var claimsIdentity = User.Identity as System.Security.Claims.ClaimsIdentity;
            //If the currently signed in Instructor was found, add the Lab Assignment
            if (claimsIdentity != null)
            {
                _context.Lab.Add(Lab);
                var instructorID = claimsIdentity.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

                LabAssignment instructorAssignment = new LabAssignment { InstructorID = Convert.ToInt32(instructorID.Value), LabID = Lab.LabID };

                _context.LabAssignment.Add(instructorAssignment);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage("./Index");
        }
    }
}