using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using LabTracker.Models;

namespace LabTracker.Pages
{
    //The main IndexModel page is the sign in page for the LabTracker program.
    //The instructor will use their email and password to sign in, or select "Register
    //as a new user" to create a profile.    
    public class IndexModel : PageModel
    {
        //BindProperty sets the loginData in a strongly typed manner within the Razor content page for access.
        [BindProperty]
        public LoginData loginData { get; set; } 
        private readonly LabTrackerContext _context;

        public IndexModel(LabTrackerContext context)
        {
            _context = context;
        }

        public void OnGet()
        {

        }

        //OnPostAsync uses authentication to sign in an instructor
        public async Task<ActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                IQueryable<Instructor> InstructorIQ = from p in _context.Instructor
                                                      select p;
                //Checks the email and password authentication
                InstructorIQ = InstructorIQ.Where(p => p.Email.ToLower().Equals(loginData.Email.ToLower()) && p.Password.Equals(loginData.Password));

                var isValid = InstructorIQ.Any();
                //If the signin information is incorrect, display an error message
                if (!isValid)
                {
                    ModelState.AddModelError("", "Username and/or Password is Invalid");
                    return Page();
                }
                //Use ClaimsIdentity to validate user and sign in to program.
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, InstructorIQ.First().InstructorID.ToString()));
                identity.AddClaim(new Claim(ClaimTypes.Name, InstructorIQ.First().LastName));
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties { IsPersistent = loginData.RememberMe });
                return RedirectToPage("Instructors/Index"); //redirect to the Instructors/Index when successfully signed in.
            }
            else
            {
                return Page();
            }
        }
        //LoginData class takes the instructor's email, password, and remember me options.
        public class LoginData
        {
            [Required]
            public string Email { get; set; }

            [Required, DataType(DataType.Password)]
            public string Password { get; set; }

            public bool RememberMe { get; set; }
        }
    }
}
