using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LabTracker.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using System.Linq;

namespace LabTracker.Pages.Labs
{
    //The EnrollStudentsModel page allows the currently signed in Instructor to enroll students 
    //to the selected Lab.
    public class EnrollStudentsModel : PageModel
    {
        private readonly LabTrackerContext _context;
        public Lab Lab { get; set; }
        //Create a SelectList of StudentNames to be displayed.
        public SelectList StudentNames { get; set; }
        //BindProperty sets the StudentIDs in a strongly typed manner within the Razor content page for access.
        [BindProperty]
        public int[] StudentIDs { get; set; }

        public EnrollStudentsModel(LabTrackerContext context)
        {
            _context = context;            
        }

        //Get the Students names to be displayed in the SelectList
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Lab = await _context.Lab.FirstOrDefaultAsync(l => l.LabID == id);
            

            if (Lab == null)
            {
                return NotFound();
            }
            //Call the GenerateSelectList method to only display the students currently not enrolled in selected lab.
            StudentNames = GenerateSelectList(id);

            return Page();
        }

        //Add LabEnrollments for selected students.
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            Lab = await _context.Lab.FindAsync(id);

            //If no student is selected, throw an error message.
            if(StudentIDs.Length == 0)
            {
                ModelState.AddModelError("No Student Selected", "You must select at least one student to enroll in this lab.");
                return Page();
            }
            //Loop through the selected StudentIDs and add the enrollments.
            foreach(int StudentId in StudentIDs)
            {
                LabEnrollment studentEnrollment = new LabEnrollment { LabID = Lab.LabID, StudentID = StudentId };
                _context.LabEnrollment.Add(studentEnrollment);
            }
            //Save changes
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        //The GenerateSelectList method creates LabEnrollments and LabAssignments DbSets. It also creates lists for labsNotEnrolled
        //and labsEnrolled for students who have and have not been enrolled in selected lab.
        private SelectList GenerateSelectList(int? id)
        {
            DbSet<LabEnrollment> LabEnrollments = _context.LabEnrollment;
            DbSet<LabAssignment> LabAssignments = _context.LabAssignment;
            //Use authentication to find the Instructor assigned to the Lab selected.
            var claimsIdentity = User.Identity as System.Security.Claims.ClaimsIdentity;
            var c = claimsIdentity.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            //Create lists for students enrolled and not yet enrolled in selected Lab.
            List<Student> labsNotEnrolled = new List<Student>();
            List<Student> labsEnrolled = new List<Student>();
            //Loop through the LabEnrollments and add students already enrolled to the labsEnrolled list.
            foreach (LabEnrollment labEnrollment in LabEnrollments)
            {
                if(labEnrollment.LabID == id)
                {
                    Student student = _context.Student.Where(s => s.StudentID == labEnrollment.StudentID).First();
                    labsEnrolled.Add(student);
                }
            }
            //Add students not yet enrolled in selected lab to the labsNotEnrolled list.
            labsNotEnrolled = _context.Student.Where(s => !labsEnrolled.Any(s2 => s2.StudentID == s.StudentID)).Where(s => LabAssignments.Any(s2 => s2.InstructorID == int.Parse(c.Value) && s2.LabID == id)).ToList();

            return new SelectList(labsNotEnrolled, "StudentID", "FullName");
        }

        
    }
}