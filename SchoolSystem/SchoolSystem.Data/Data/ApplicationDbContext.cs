using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SchoolSystem.Data.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Data.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {
        }

        public DbSet<Group> Groups { get; set; }

        public DbSet<Grade> Grades { get; set; }

        public DbSet<Note> Notes { get; set; }

        public DbSet<Student> Students { get; set; }

        public DbSet<Subject> Subjects { get; set; }

        public DbSet<Teacher> Teachers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
              .Entity<Grade>()
              .HasOne(g => g.Student)
              .WithMany(s => s.Grades)
              .HasForeignKey(g => g.StudentId)
              .OnDelete(DeleteBehavior.Restrict);

            builder
               .Entity<Note>()
               .HasOne(n => n.Student)
               .WithMany(s => s.Notes)
               .HasForeignKey(n => n.StudentId)
               .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Note>()
                .HasOne(n => n.Teacher)
                .WithMany()
                .HasForeignKey(n => n.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Student>()
                .HasOne(s => s.Group)
                .WithMany(g => g.Students)
                .HasForeignKey(s => s.GroupId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Grade>()
                .HasOne(g => g.Teacher)
                .WithMany()
                .HasForeignKey(g => g.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);



            base.OnModelCreating(builder);
        }

    }
}
