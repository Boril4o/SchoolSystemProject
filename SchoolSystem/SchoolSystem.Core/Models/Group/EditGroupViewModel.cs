using static SchoolSystem.Data.Data.Constants.DataConstants;
using System.ComponentModel.DataAnnotations;


namespace SchoolSystem.Core.Models.Group
{
    public class EditGroupViewModel
    {
        [Required]
        [MaxLength(GroupNumberMaxLength)]
        public string Number { get; set; }


        [Range(GroupMinPeople, GroupMaxPeople)]
        public int MaxPeople { get; set; }

        public int Id { get; set; }
    }
}
