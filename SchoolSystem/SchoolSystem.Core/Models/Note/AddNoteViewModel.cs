using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static SchoolSystem.Data.Data.Constants.DataConstants;

namespace SchoolSystem.Core.Models.Note
{
    public class AddNoteViewModel
    {
        [Required]
        [MaxLength(NoteTitleMaxLength)]
        public string Title { get; set; }

        [Required]
        [MaxLength(NoteDescriptionMaxLength)]
        public string Description { get; set; }

        public int TeacherId { get; set; }

        public int StudentId { get; set; }

        public int SubjectId { get; set; }

        public bool IsPositive { get; set; }

        public IEnumerable<SchoolSystem.Data.Data.Entities.Subject> Subjects { get; set; } =
          new List<SchoolSystem.Data.Data.Entities.Subject>();
    }
}
