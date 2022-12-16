using System.ComponentModel.DataAnnotations;
using static SchoolSystem.Infrastructure.Data.Constants.DataConstants;

namespace SchoolSystem.Core.Models.Group
{
    public class AddGroupViewModel
    {
        [Required]
        [MaxLength(GroupNumberMaxLength)]
        [MinLength(GroupNumberMinLength)]
        public string Number { get; set; } = null!;

        [Range(GroupMinPeople, GroupMaxPeople)]
        public int MaxPeople { get; set; }
    }
}
