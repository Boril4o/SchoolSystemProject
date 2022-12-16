using static SchoolSystem.Infrastructure.Data.Constants.DataConstants;
using System.ComponentModel.DataAnnotations;


namespace SchoolSystem.Core.Models.Group
{
    public class EditGroupViewModel
    {
        [Required]
        [MaxLength(GroupNumberMaxLength)]
        [MinLength(GroupNumberMinLength)]
        public string Number { get; set; } = null!;


        [Range(GroupMinPeople, GroupMaxPeople)]
        public int MaxPeople { get; set; }

        public int Id { get; set; }
    }
}
