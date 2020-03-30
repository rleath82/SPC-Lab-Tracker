using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LabTracker.Models;

namespace LabTracker.Pages.Labs
{
    //The IndexModel page displays all of the Labs created by the Instructor currently logged in.
    public class IndexModel : PageModel
    {
        private readonly LabTrackerContext _context;
        //BindProperty sets the InstructorID in a strongly typed manner within the Razor content page for access.
        [BindProperty]
        public int InstructorID { get; set; }

        public IndexModel(LabTrackerContext context)
        {
            _context = context;
        }
        //Create a list of Labs
        public IList<Lab> Lab { get;set; }

        //Get the Labs for the currently signed in Instructor.
        public async Task OnGetAsync()
        {
            //Use authentication to find the currently signed in Instructor
            var claimsIdentity = User.Identity as System.Security.Claims.ClaimsIdentity;
            var c = claimsIdentity.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

            InstructorID = int.Parse(c.Value);

            //Create a DbSet of LabAssignments
            DbSet<LabAssignment> LabAssignments = _context.LabAssignment;
            //Only display the Labs assigned to the currently signed in Instructor.
            Lab = await _context.Lab.Where(l => LabAssignments.Any(l2 => l2.InstructorID == int.Parse(c.Value) && l2.LabID == l.LabID)).ToListAsync();           
        }
    }
}
