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
    //The IndexModel displays all of the students in the database.
    public class IndexModel : PageModel
    {
        private readonly LabTrackerContext _context;

        public IndexModel(LabTrackerContext context)
        {
            _context = context;
        }
        //Properties used for sorting and paging
        public string NameSort { get; set; }
        public string IDSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }
        //Use the PaginatedList model to create the list of students.
        public PaginatedList<Student> Student { get; set; }

        //The OnGetAsync method takes the properties set as parameters for sorting and paging the index page.
        public async Task OnGetAsync(string sortOrder, string currentFilter, string searchString, int? pageIndex)
        {
            //CurrentSort provides the Razor Page with the current sort order. The current sort order must be included in the paging
            //links to keep the sort order while paging.
            CurrentSort = sortOrder;
            //NameSort is set to "name_desc", which is the student's last name
            NameSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            //IDSort is set to "id_desc", which is the student's ID number
            IDSort = sortOrder == "ID" ? "id_desc" : "ID";
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
            IQueryable<Student> studentIQ = from s in _context.Student
                                            select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                studentIQ = studentIQ.Where(s => s.LastName.ToLower().Contains(searchString)
                                        || s.FirstMidName.ToLower().Contains(searchString));
            }

            //Switch statement to switch between Last Name and Student ID columns for sorting.
            switch (sortOrder)
            {
                case "name_desc":
                    studentIQ = studentIQ.OrderByDescending(s => s.LastName);
                    break;
                case "ID":
                    studentIQ = studentIQ.OrderBy(s => s.StudentID);
                    break;
                case "id_desc":
                    studentIQ = studentIQ.OrderByDescending(s => s.StudentID);
                    break;
                default:
                    studentIQ = studentIQ.OrderBy(s => s.LastName);
                    break;
            }
            //Set the pageSize to 10 students per page.
            int pageSize = 10;
            //Create the PaginatedList referencing the pageIndex and the pageSize
            Student = await PaginatedList<Student>.CreateAsync(
                studentIQ.AsNoTracking(), pageIndex ?? 1, pageSize);
        }
    }
}
