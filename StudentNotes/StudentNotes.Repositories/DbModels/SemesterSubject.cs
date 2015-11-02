namespace StudentNotes.Repositories.DbModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SemesterSubject")]
    public partial class SemesterSubject
    {
        public SemesterSubject()
        {
            FileSharedGroup = new HashSet<FileSharedGroup>();
            SemesterSubjectFile = new HashSet<SemesterSubjectFile>();
        }

        public int SemesterSubjectId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public int SemesterId { get; set; }

        public virtual ICollection<FileSharedGroup> FileSharedGroup { get; set; }

        public virtual Semester Semester { get; set; }

        public virtual ICollection<SemesterSubjectFile> SemesterSubjectFile { get; set; }
    }
}
