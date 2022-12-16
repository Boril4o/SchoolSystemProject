using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolSystem.Infrastructure.Data.Entities
{
    public class Student
    {
        public Student()
        {
            Notes = new List<Note>();
            Grades = new List<Grade>();
        }

        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(User))]
        [Required]
        public string UserId { get; set; }
        public User User { get; set; }

        [ForeignKey(nameof(Group))]
        public int? GroupId { get; set; }
        public Group Group { get; set; }

        [NotMapped]
        public double AverageGrade
        {
            get
            {
                if (Grades.Count() == 0)
                {
                    return 0;
                }

                return Grades.Average(g => g.Number);
            }
        }

        public List<Note> Notes { get; set; } = new List<Note>();

        public List<Grade> Grades { get; set; } = new List<Grade>();
    }
}
