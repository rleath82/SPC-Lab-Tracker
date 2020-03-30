using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LabTracker.Models
{
    //This is the main class that coordinates EF Core functionality for the LabTracker models.
    //The data context specifies which entities are included in the data model.
    public class LabTrackerContext : DbContext
    {
        public LabTrackerContext (DbContextOptions<LabTrackerContext> options)
            : base(options)
        {
        }

        public DbSet<Lab> Lab { get; set; }
        public DbSet<CourseEnrollment> CourseEnrollment { get; set; }
        public DbSet<LabEnrollment> LabEnrollment { get; set; }
        public DbSet<Course> Course { get; set; }
        public DbSet<Student> Student { get; set; }
        public DbSet<Instructor> Instructor { get; set; }
        public DbSet<LabAssignment> LabAssignment { get; set; }
        public DbSet<CourseAssignment> CourseAssignment { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Lab>().ToTable("Lab");
            modelBuilder.Entity<CourseEnrollment>().ToTable("CourseEnrollment");
            modelBuilder.Entity<LabEnrollment>().ToTable("LabEnrollment");
            modelBuilder.Entity<Student>().ToTable("Student");
            modelBuilder.Entity<Instructor>().ToTable("Professor");
            modelBuilder.Entity<LabAssignment>().ToTable("LabAssignment");
            modelBuilder.Entity<Course>().ToTable("Course");
            modelBuilder.Entity<CourseAssignment>().ToTable("CourseAssignment");

            //HasKey sets the properties that make up the primary key for this entity type.
            modelBuilder.Entity<LabAssignment>()
                .HasKey(l => new { l.LabID, l.InstructorID });

            modelBuilder.Entity<CourseAssignment>()
                .HasKey(c => new { c.CourseID, c.InstructorID });
        }
    }
}
