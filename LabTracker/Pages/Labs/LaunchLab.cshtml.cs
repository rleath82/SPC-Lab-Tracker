using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LabTracker.Models;
using Microsoft.AspNetCore.Authentication;

namespace LabTracker.Pages.Labs
{
    //The LaunchLabModel page allows the Instructor to launch the selected lab.
    //The Lab, when launched, will be displayed in a Bootstrap modal page, where the 
    //student can sign in and out accordingly.
    public class LaunchLabModel : PageModel
    {
        private readonly LabTrackerContext _context;
        //BindProperty sets the StudentID, SuccessLogin, and SuccessLogout in a strongly typed manner within the Razor content page for access.
        [BindProperty]
        public int StudentID { get; set; }

        [BindProperty]
        public bool SuccessLogin { get; set; }

        [BindProperty]
        public bool SuccessLogout { get; set; }
        
        //Create a list of LabEnrollments
        public List<LabEnrollment> LabEnrollments { get; set; }

        [BindProperty]
        public Lab Lab { get; set; }

        public LaunchLabModel(LabTrackerContext context)
        {
            _context = context;
        }
        //OnGetAsync gets the LabID to be launched
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            string page = Request.Headers["Referer"].ToString().Split('/').LastOrDefault();
            if (page != null)
            {
                if (page.ToLower() != "labs") { return NotFound(); }
            }

            Lab = await _context.Lab.FirstOrDefaultAsync(m => m.LabID == id);

            if (Lab == null)
            {
                return NotFound();
            }
            return Page();
        }

        //OnPostAsync launches the lab, and handles all inputs from the users 
        public async Task<IActionResult> OnPostAsync(int? id, string command)
        {
            //If the "End Lab" button is clicked, end the lab and sign out the instructor. Redirect to the login page.
            if (command.Equals("EndLab"))
            {
                await HttpContext.SignOutAsync();
                return RedirectToPage("/Index");
            }

            Lab = await _context.Lab.Where(l => l.LabID == id).FirstAsync();
            //Find LabEnrollment when student signs in or out of lab.
            LabEnrollment LabEnrollment = await _context.LabEnrollment.Where(e => e.LabID == id && e.StudentID == StudentID).FirstOrDefaultAsync();
            //if the student has not been enrolled in the lab, throw an error message. Calls the LabEnrollmentExists method
            if (LabEnrollment == null || !LabEnrollmentExists(LabEnrollment.LabEnrollmentID))
            { ModelState.AddModelError("Error", "You have not been enrolled in this lab, please contact your instructor."); return Page(); }

            //If the student successfully signs into lab, add the DateTime.Now stamp in LabEnrollments
            if (command.Equals("Login"))
            {
                //If the student tries to sign in after already signing in, throw an error message.
                if (LabEnrollment.LabSignIn != null) { ModelState.AddModelError("Error", "You have already signed into this lab."); return Page(); }
                LabEnrollment.LabSignIn = DateTime.Now;
                _context.LabEnrollment.Update(LabEnrollment);
                SuccessLogin = true;
            }
            //If the student successfully signs out of the lab, add the DateTime.Now stamp in LabEnrollments
            if (command.Equals("Logout"))
            {
                //If the student tries to sign out after already signing out, throw an error message.
                if (LabEnrollment.LabSignOut != null) { ModelState.AddModelError("Error", "You have already signed out of this lab."); return Page(); }
                LabEnrollment.LabSignOut = DateTime.Now;
                _context.LabEnrollment.Update(LabEnrollment);
                SuccessLogout = true;
            }
            //Save changes
            await _context.SaveChangesAsync();

            return Page();
        }
        //Determine if the student is enrolled in the selected lab.
        private bool LabEnrollmentExists(int id)
        {
            return _context.LabEnrollment.Any(e => e.LabEnrollmentID == id);
        }
    }
}
