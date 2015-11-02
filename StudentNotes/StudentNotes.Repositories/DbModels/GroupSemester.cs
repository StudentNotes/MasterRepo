namespace StudentNotes.Repositories.DbModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GroupSemester")]
    public partial class GroupSemester
    {
        public int GroupSemesterId { get; set; }

        public int GroupId { get; set; }

        public int SemesterId { get; set; }

        public virtual Group Group { get; set; }

        public virtual Semester Semester { get; set; }
    }
}
