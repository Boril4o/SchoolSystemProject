﻿using static SchoolSystem.Data.Data.Constants.DataConstants;
using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Core.Models.Subject
{
    public class EditSubjectViewModel
    {
        [Required]
        [MaxLength(SubjectNameMaxLength)]
        public string Name { get; set; }
    }
}