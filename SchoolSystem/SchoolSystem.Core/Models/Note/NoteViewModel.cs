namespace SchoolSystem.Core.Models.Note
{
    public class NoteViewModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string TeacherName { get; set; }

        public string SubjectName { get; set; }

        public bool IsPositive { get; set; }

        public DateTime Date { get; set; }
    }
}
