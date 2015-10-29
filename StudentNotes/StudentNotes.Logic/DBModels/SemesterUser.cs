namespace StudentNotes.Logic.DBModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SemesterUser")]
    public partial class SemesterUser
    {
        public int SemesterUserId { get; set; }

        public int SemesterId { get; set; }

        public int UserId { get; set; }

        public virtual Semester Semester { get; set; }

        public virtual User User { get; set; }
    }
}
