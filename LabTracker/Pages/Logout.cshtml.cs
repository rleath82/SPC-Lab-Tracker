using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;

namespace LabTracker.Pages
{
    //The LogoutModel logs the Instructor out of the program. Then redirects to the Login page (/Index).
    public class LogoutModel : PageModel
    {
        public async Task<IActionResult> OnGet(int? id)
        {
            await HttpContext.SignOutAsync();
            return RedirectToPage("Index");
        }
        public async Task<IActionResult> OnPost()
        {
            await HttpContext.SignOutAsync();
            return RedirectToPage("/Index");
        }
    }
}