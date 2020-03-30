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
    //The DetailsModel page displays the students currently enrolled in selected Lab. 
    public class DetailsModel : PageModel
    {
        private readonly LabTrackerContext _context;

        public DetailsModel(LabTrackerContext context)
        {
            _context = context;
        }

        public Lab Lab { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id, int? studentId)
        {
            if (id == null)
            {
                return NotFound();
            }
            //Show the LabEnrollments (Students) for the selected Lab.
            Lab = await _context.Lab
                .Include(c => c.LabEnrollments)
                    .ThenInclude(s => s.Student)                
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.LabID == id);

            if (Lab == null)
            {
                return NotFound();
            }

            //Remove the LabEnrollment from the database
            if (studentId != null)
            {
                LabEnrollment studentEnrollment= Lab.LabEnrollments.Where(x => x.LabID == id.Value && x.StudentID == studentId).First();
                _context.LabEnrollment.Remove(studentEnrollment);
                await _context.SaveChangesAsync();
                Response.Redirect("/Labs/Details/" + id.ToString());
            }
            return Page();
        }
    }
}
