using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SchoolSystem.Data.Data.Entities;

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
            //Grade
            builder
                .Entity<Grade>()
                .HasOne(g => g.Subject)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Grade>()
                .HasOne(g => g.Teacher)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Grade>()
                .HasOne(g => g.Student)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            //Note
            builder
                .Entity<Note>()
                .HasOne(n => n.Subject)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Note>()
                .HasOne(n => n.Teacher)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Note>()
                .HasOne(n => n.Student)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            //Teacher
            builder
                .Entity<Teacher>()
                .HasOne(t => t.User)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Teacher>()
                .HasOne(t => t.Group)
                .WithOne(g => g.Teacher)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Teacher>()
                .HasOne(t => t.Subject)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            //Student
            builder
              .Entity<Student>()
              .HasOne(s => s.User)
              .WithMany()
              .OnDelete(DeleteBehavior.Restrict);

            builder
               .Entity<Student>()
               .HasOne(s => s.Group)
               .WithMany()
               .OnDelete(DeleteBehavior.Restrict);


            base.OnModelCreating(builder);
        }
    }
}