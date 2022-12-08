using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolSystem.Data.Data.Entities
{
    public class Student
    {
        public Student()
        {
            this.Notes = new List<Note>();
            this.Grades = new List<Grade>();
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

        public List<Note> Notes { get; set; }

        public List<Grade> Grades { get; set; }
    }
}
