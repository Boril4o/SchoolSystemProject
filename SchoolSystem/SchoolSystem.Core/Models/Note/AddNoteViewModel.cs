using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static SchoolSystem.Infrastructure.Data.Constants.DataConstants;

namespace SchoolSystem.Core.Models.Note
{
    public class AddNoteViewModel
    {
        [Required]
        [MaxLength(NoteTitleMaxLength)]
        [MinLength(NoteTitleMinLength)]
        public string Title { get; set; }

        [Required]
        [MaxLength(NoteDescriptionMaxLength)]
        [MinLength(NoteDescriptionMinLength)]
        public string Description { get; set; }

        public int TeacherId { get; set; }

        public int StudentId { get; set; }

        public int SubjectId { get; set; }

        public bool IsPositive { get; set; }

        public IEnumerable<Infrastructure.Data.Entities.Subject>? Subjects { get; set; } =
          new List<Infrastructure.Data.Entities.Subject>();
    }
}
