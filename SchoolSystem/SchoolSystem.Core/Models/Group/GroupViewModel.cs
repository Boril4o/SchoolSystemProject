using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Core.Models.Group
{
    public class GroupViewModel
    {
        public int Id { get; set; }

        public string Number { get; set; }

        public int People { get; set; }

        public int MaxPeople { get; set; }
    }
}
