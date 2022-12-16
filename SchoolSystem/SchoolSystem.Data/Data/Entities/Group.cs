using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static SchoolSystem.Infrastructure.Data.Constants.DataConstants;

namespace SchoolSystem.Infrastructure.Data.Entities
{
    public class Group
    {
        public Group()
        {
            Students = new List<Student>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(GroupNumberMaxLength)]
        public string Number { get; set; }

        //[ForeignKey(nameof(Teacher))]
        //public int TeacherId { get; set; }
        //public Teacher Teacher { get; set; }

        [Range(GroupMinPeople, GroupMaxPeople)]
        public int MaxPeople { get; set; }

        public List<Student> Students { get; set; } = new List<Student>();
    }
}
