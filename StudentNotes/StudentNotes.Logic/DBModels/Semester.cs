namespace StudentNotes.Logic.DBModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Semester")]
    public partial class Semester
    {
        public Semester()
        {
            GroupSemester = new HashSet<GroupSemester>();
            SemesterSubject = new HashSet<SemesterSubject>();
            SemesterUser = new HashSet<SemesterUser>();
        }

        public int SemesterId { get; set; }

        public int SemesterNumber { get; set; }

        public int StudySubjectId { get; set; }

        public virtual ICollection<GroupSemester> GroupSemester { get; set; }

        public virtual StudySubject StudySubject { get; set; }

        public virtual ICollection<SemesterSubject> SemesterSubject { get; set; }

        public virtual ICollection<SemesterUser> SemesterUser { get; set; }
    }
}
