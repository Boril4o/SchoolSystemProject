using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static SchoolSystem.Data.Data.Constants.DataConstants;

namespace SchoolSystem.Data.Data.Entities
{
    public class Group
    {
        public Group()
        {
            this.Students = new List<Student>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(ClassNumberMaxLength)]
        public string Number { get; set; }

        [ForeignKey(nameof(Teacher))]
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }

        public List<Student> Students { get; set; }
    }
}
