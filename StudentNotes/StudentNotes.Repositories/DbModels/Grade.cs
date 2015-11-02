namespace StudentNotes.Repositories.DbModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Grade")]
    public partial class Grade
    {
        public Grade()
        {
            StudySubject = new HashSet<StudySubject>();
        }

        public int GradeId { get; set; }

        [Column(TypeName = "date")]
        public DateTime Year { get; set; }

        public int SchoolId { get; set; }

        public virtual School School { get; set; }

        public virtual ICollection<StudySubject> StudySubject { get; set; }
    }
}
