using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static SchoolSystem.Data.Data.Constants.DataConstants;


namespace SchoolSystem.Data.Data.Entities
{
    public class Teacher
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(User))]
        [Required]
        public string UserId { get; set; }
        public User User { get; set; }

        [ForeignKey(nameof(Group))]
        public int? GroupID { get; set; }
        public Group Group { get; set; }

        [Range(typeof(decimal), TeacherMinSalary, TeacherMaxSalary)]
        public decimal Salary { get; set; }

        [ForeignKey(nameof(Subject))]
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }
    }
}
