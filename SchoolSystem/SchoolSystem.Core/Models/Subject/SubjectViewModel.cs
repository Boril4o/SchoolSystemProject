using static SchoolSystem.Data.Data.Constants.DataConstants;
using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Core.Models.Subject
{
    public class SubjectViewModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(SubjectNameMaxLength)]
        public string Name { get; set; }
    }
}
