﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static SchoolSystem.Data.Data.Constants.DataConstants;

namespace SchoolSystem.Data.Data.Entities
{
    public class Note
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(NoteTitleMaxLength)]
        public string Title { get; set; }

        [Required]
        [MaxLength(NoteDescriptionMaxLength)]
        public string Description { get; set; }

        [ForeignKey(nameof(Teacher))]
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }

        [ForeignKey(nameof(Student))]
        public int StudentId { get; set; }
        public Student Student { get; set; }

        [ForeignKey(nameof(Subject))]
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }
    }
}