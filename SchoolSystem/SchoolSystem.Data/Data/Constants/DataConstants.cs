using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Data.Data.Constants
{
    public class DataConstants
    {
        //Subject
        public const int SubjectNameMaxLength = 20;
        public const int SubjectNameMinLength = 3;

        //Class
        public const int ClassNumberMaxLength = 10;
        public const int ClassNumberMinLength = 2;
        
        //User
        public const int UserFirstNameMaxLength = 50;
        public const int UserFirstNameMinLength = 1;

        public const int UserLastNameMaxLength = 50;
        public const int UserLastNameMinLength = 1;

        public const int UserMaxAge = 99;
        public const int UserMinAge = 7;

        //Note
        public const int NoteTitleMaxLength = 20;
        public const int NoteTitleMinLength = 3;

        public const int NoteDescriptionMaxLength = 300;
        public const int NoteDescriptionMinLength = 3;

        //Teacher
        public const string TeacherMaxSalary = "10000";
        public const string TeacherMinSalary = "800";
    }
}
