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
    //The UnenrollLabModel page confirms the removal of the LabEnrollment for a selected student.
    public class UnenrollLabModel : PageModel
    {
        private readonly LabTrackerContext _context;

        public UnenrollLabModel(LabTrackerContext context)
        {
            _context = context;
        }
        //BindProperty sets the Student object in a strongly typed manner within the Razor content page for access.
        [BindProperty]
        public Student Student { get; set; }
        public string ErrorMessage { get; set; }
        public int LabID { get; set; }  

        //The OnGetAsync method gets the StudentID and LabID for the LabEnrollment removal
        public async Task<IActionResult> OnGetAsync(int? id, int? labId, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            Student = await _context.Student
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.StudentID == id);

            if (Student == null)
            {
                return NotFound();
            }

            return Page();
        }

        //The OnPostAsync removes the LabEnrollment 
        public async Task<IActionResult> OnPostAsync(int? id, int labId)
        {
            if (id == null)
            {
                return NotFound();
            }

            Student = await _context.Student.FindAsync(id);
            LabEnrollment studentEnrollment = _context.LabEnrollment
                .Where(le => le.StudentID == id && le.LabID == labId).FirstOrDefault();
            _context.LabEnrollment.Remove(studentEnrollment);
            //Save changes
            await _context.SaveChangesAsync();

            return RedirectToPage("./Details");
        }
    }
}
