using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LabTracker.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace LabTracker.Pages.Courses
{
    //The EnrollStudentsModel page allows an Instructor to enroll students in courses. The Instructor can only
    //enroll students into courses already assigned to the currently signed in Instructor.
    public class EnrollStudentsModel : PageModel
    {
        private readonly LabTrackerContext _context;
        //The course object
        public Course Course { get; set; }
        //create a SelectList of StudentNames to display to be enrolled in a course
        public SelectList StudentNames { get; set; }
        //BindProperty sets the StudentIDs in a strongly typed manner within the Razor content page for access.
        //Create an array of StudentIDs
        [BindProperty]
        public int[] StudentIDs { get; set; }

        //Constructor that initializes the StudentNames SelectList and orders the names by LastName
        public EnrollStudentsModel(LabTrackerContext context)
        {
            _context = context;
            StudentNames = new SelectList(_context.Student.OrderBy(x => x.LastName), "StudentID", "FullName");
        }
        //Get the students to be enrolled
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Course = await _context.Course.FirstOrDefaultAsync(c => c.CourseID == id);

            if (Course == null)
            {
                return NotFound();
            }
            //Call the GenerateSelectList method to display the students not yet enrolled in the selected course.
            StudentNames = GenerateSelectList(id);
            return Page();
        }

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

            Course = await _context.Course.FindAsync(id);

            if (StudentIDs.Length == 0)
            {
                ModelState.AddModelError("No Student Selected", "You must select at least one student to enroll in course.");
                return Page();
            }
            //Loop through StudentIDs and enroll each selected student to the selected course.
            foreach(int StudentId in StudentIDs)
            {
                CourseEnrollment studentEnrollment = new CourseEnrollment { CourseID = Course.CourseID, StudentID = StudentId };
                _context.CourseEnrollment.Add(studentEnrollment);
            }
            //Save changes
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
        //GenerateSelectList SelectList creates lists of studentEnrollments and instructorAssignments.
        //Uses authentication to show a list of students that have not yet been enrolled in selected course.
        private SelectList GenerateSelectList(int? id)
        {
            //Create DbSets of studentEnrollments and instructorAssignments
            DbSet<CourseEnrollment> studentEnrollments = _context.CourseEnrollment;
            DbSet<CourseAssignment> instructorAssignments = _context.CourseAssignment;
            //Use authentication to reference the instructor currently signed in
            var claimsIdentity = User.Identity as System.Security.Claims.ClaimsIdentity;
            var c = claimsIdentity.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            //create lists of studentNotEnrolled and studentEnrolled
            List<Student> studentNotEnrolled = new List<Student>();
            List<Student> studentEnrolled = new List<Student>();
            //Loop through the studentEnrollments and add students already enrolled in selected course to the
            //studentEnrolled list
            foreach (CourseEnrollment enrolledStudent in studentEnrollments)
            {
                if (enrolledStudent.StudentID == id)
                {
                    Student student = _context.Student.Where(x => x.StudentID == enrolledStudent.StudentID).First();
                    studentEnrolled.Add(student);
                }
            }
            //Create list of students not yet enrolled in a course where the instructor is assigned
            studentNotEnrolled = _context.Student.Where(l => !studentEnrolled.Any(l2 => l2.StudentID == l.StudentID)).Where(l => instructorAssignments.Any(l2 => l2.InstructorID == int.Parse(c.Value) && l2.CourseID == id)).ToList();

            return new SelectList(studentNotEnrolled.OrderBy(x => x.LastName), "StudentID", "FullName");
        }
    }
}