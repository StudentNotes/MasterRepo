namespace StudentNotes.Repositories.DbModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("School")]
    public partial class School
    {
        public School()
        {
            Grade = new HashSet<Grade>();
            UserVisitedSchool = new HashSet<UserVisitedSchool>();
        }

        public int SchoolId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Column(TypeName = "text")]
        public string Description { get; set; }

        public virtual ICollection<Grade> Grade { get; set; }

        public virtual ICollection<UserVisitedSchool> UserVisitedSchool { get; set; }
    }
}
