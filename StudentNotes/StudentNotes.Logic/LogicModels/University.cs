using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentNotes.Logic.DBModels;

namespace StudentNotes.Logic.LogicModels
{
    public class University
    {
        public int Id { get; private set; }
        public string Name { get; private set; }


        public University(int id)
        {
            Id = id;
            using (var context = new StudentNotesContext())
            {
                var schoolName = (from s in context.School where s.SchoolId == id select s).ToList().FirstOrDefault().Name;
                if (schoolName == null)
                {
                    //  No school found
                    return;
                }
                Name = schoolName;
            }
        }
        public University(string name)
        {
            Name = name;
        }
    }
}
