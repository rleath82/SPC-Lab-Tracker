using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LabTracker.Models;

namespace LabTracker.Pages.Students
{
    //The DetailsModel page displays the selected student's information as well as the courses and labs 
    //currently enrolled in.
    public class DetailsModel : PageModel
    {
        private readonly LabTrackerContext _context;

        public DetailsModel(LabTrackerContext context)
        {
            _context = context;
        }
        //Create a student property
        public Student Student { get; set; }

        //Gets the StudentID information
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //Show the CourseEnrollments and LabEnrollments
            Student = await _context.Student
                .Include(e => e.CourseEnrollments)
                    .ThenInclude(c => c.Course)
                .Include(l => l.LabEnrollments)
                    .ThenInclude(l => l.Lab)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.StudentID == id);

            if (Student == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
