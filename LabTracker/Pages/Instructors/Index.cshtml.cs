using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LabTracker.Models;
using LabTracker.Models.LabViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace LabTracker.Pages.Instructors
{
    //The IndexModel page is the Instructor's dashboard when successfully signed in [Authorize]. It will give the
    //Instructor the options to edit profile, assign courses, add students, schedule labs, details, and delete profile.
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly LabTrackerContext _context;

        public IndexModel(LabTrackerContext context)
        {
            _context = context;
        }
        //Find the signed in instructor from the list of instructors
        public IList<Instructor> Instructor { get; set; }

        public async Task OnGetAsync()
        {
            Instructor = await _context.Instructor.ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await HttpContext.SignOutAsync();
            return RedirectToPage("../Index");
        }
        
    }
}
