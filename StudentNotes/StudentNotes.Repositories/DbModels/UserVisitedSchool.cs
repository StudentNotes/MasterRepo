namespace StudentNotes.Repositories.DbModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UserVisitedSchool")]
    public partial class UserVisitedSchool
    {
        public int UserVisitedSchoolId { get; set; }

        public int UserId { get; set; }

        public int SchoolId { get; set; }

        public virtual School School { get; set; }

        public virtual User User { get; set; }
    }
}
