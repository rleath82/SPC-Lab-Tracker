using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LabTracker.Models;

namespace LabTracker.Pages.Courses
{
    //The IndexModel displays the courses assigned to the signed in Instructor. 
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
        //Properties used for sorting and paging 
        public string NumSort { get; set; }
        public string TitleSort { get; set; } 
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }
        //Use the PaginatedList model to create the list of courses.
        public PaginatedList<Course> Course { get; set; } 
        //The OnGetAsync method takes the properties set as parameters for sorting and paging the index page. 
        public async Task OnGetAsync(string sortOrder, string currentFilter, string searchString, int? pageIndex)
        {
            //Use authentication to display the courses for the currently signed in Instructor.
            var claimsIdentity = User.Identity as System.Security.Claims.ClaimsIdentity;
            var ci = claimsIdentity.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

            InstructorID = int.Parse(ci.Value);
            //CurrentSort provides the Razor Page with the current sort order. The current sort order must be included in the paging
            //links to keep the sort order while paging.
            CurrentSort = sortOrder;
            //NumSort is set to "num_desc", which is the Course Prefix
            NumSort = String.IsNullOrEmpty(sortOrder) ? "num_desc" : "";
            //TitleSort is set to "title_desc", which is the Course Title
            TitleSort = sortOrder == "Title" ? "title_desc" : "Title";
            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            //CurrentFilter provides the Razor Page with the current filter string.
            CurrentFilter = searchString;
            //Create an IQueryable<Course> before the switch statement.
            IQueryable<Course> courseIQ = from c in _context.Course.Where(i => i.CourseAssignments.Any(i2 => i2.InstructorID == int.Parse(ci.Value)))
                                            select c;

            if (!String.IsNullOrEmpty(searchString))
            {
                courseIQ = courseIQ.Where(c => c.CourseNum.ToLower().Contains(searchString)
                                        || c.Title.ToLower().Contains(searchString));
            }
            //Switch statement to switch between Course Prefix and Title columns for sorting.
            switch (sortOrder)
            {
                case "num_desc":
                    courseIQ = courseIQ.OrderByDescending(c => c.CourseNum);
                    break;
                case "Title":
                    courseIQ = courseIQ.OrderBy(c => c.Title);
                    break;
                case "title_desc":
                    courseIQ = courseIQ.OrderByDescending(c => c.Title);
                    break;
                default:
                    courseIQ = courseIQ.OrderBy(c => c.CourseNum);
                    break;
            }
            //Set the pageSize to 10 courses per page.
            int pageSize = 10;
            //Create the PaginatedList referencing the pageIndex and the pageSize
            Course = await PaginatedList<Course>.CreateAsync(
                courseIQ.AsNoTracking(), pageIndex ?? 1, pageSize);
        }
    }
}
