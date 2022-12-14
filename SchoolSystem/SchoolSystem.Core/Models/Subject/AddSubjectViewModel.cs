using static SchoolSystem.Infrastructure.Data.Constants.DataConstants;
using System.ComponentModel.DataAnnotations;


namespace SchoolSystem.Core.Models.Subject
{
    public class AddSubjectViewModel
    {
        [Required]
        [MaxLength(SubjectNameMaxLength)]
        [MinLength(SubjectNameMinLength)]
        public string Name { get; set; } = null!;
    }
}
