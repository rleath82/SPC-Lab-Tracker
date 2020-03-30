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
    //The StudentEnrollModel page allows the signed in Instructor to enroll a single student to any number of labs from
    //the Students page.
    public class StudentEnrollModel : PageModel
    {
        private readonly LabTrackerContext _context;
        public Student Student { get; set; }
        public SelectList LabNames { get; set; }
        //BindProperty sets the labIDs in a strongly typed manner within the Razor content page for access.
        [BindProperty]
        public int[] LabIDs { get; set; }

        public StudentEnrollModel(LabTrackerContext context) 
        {
            _context = context;            
        }

        //OnGetAsync gets the selected StudentID to be enrolled in labs.
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Student = await _context.Student.FirstOrDefaultAsync(s => s.StudentID == id);

            if (Student == null)
            {
                return NotFound();
            }
            //Call the GenerateSelectList method to display the Labs the selected student is not yet enrolled in.
            LabNames = GenerateSelectList(id);
            return Page();
        }

        //OnPostAsync adds the LabEnrollments to the student selected.
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }
            //Get the StudentID 
            Student = await _context.Student.FindAsync(id);
            //If no labs are selected, throw an error message.
            if (LabIDs.Length == 0)
            {
                ModelState.AddModelError("No Lab Selected", "You must select at least one lab to enroll this student in");
                return Page();
            }
            //Loop through the LabEnrollments and enroll the student in the selected labs.
            foreach (int LabId in LabIDs)
            {
                LabEnrollment studentEnrollment = new LabEnrollment { StudentID = Student.StudentID, LabID = LabId };
                _context.LabEnrollment.Add(studentEnrollment);
            }
            await _context.SaveChangesAsync();

            return RedirectToPage("/Students/Index");
        }

        //GenerateSelectList SelectList creates lists of labEnrollments and labAssignments.
        //Uses authentication to show a list of labs that have not yet been enrolled to the selected student.
        private SelectList GenerateSelectList(int? id)
        {
            //Create DbSets of labEnrollments and labAssignments
            DbSet<LabEnrollment> labEnrollments = _context.LabEnrollment;
            DbSet<LabAssignment> labAssignments = _context.LabAssignment;
            //Use authentication to find the Instructor currently signed in
            var claimsIdentity = User.Identity as System.Security.Claims.ClaimsIdentity;
            var c = claimsIdentity.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            //Create lists of labsNotEnrolled and labsEnrolled
            List<Lab> labsNotEnrolled = new List<Lab>();
            List<Lab> labsEnrolled = new List<Lab>();

            //Loop through the labEnrollments and add all of the labs currently enrolled in to the labsEnrolled
            foreach (LabEnrollment enrolledStudent in labEnrollments)
            {
                if(enrolledStudent.StudentID == id)
                {
                    Lab lab = _context.Lab.Where(l => l.LabID == enrolledStudent.LabID).First();
                    labsEnrolled.Add(lab);
                }
            }
            //Add all of the labs not currently enrolled in to the labsNotEnrolled in where the Instructor is currently assigned.
            labsNotEnrolled = _context.Lab.Where(l => !labsEnrolled.Any(l2 => l2.LabID == l.LabID)).Where(l => labAssignments.Any(l2 => l2.InstructorID == int.Parse(c.Value) && l2.LabID == l.LabID)).ToList();

            return new SelectList(labsNotEnrolled, "LabID", "Name");
        }
    }
}
