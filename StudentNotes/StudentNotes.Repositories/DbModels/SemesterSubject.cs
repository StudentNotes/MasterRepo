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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FileSharedGroup> FileSharedGroup { get; set; }

        public virtual Semester Semester { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SemesterSubjectFile> SemesterSubjectFile { get; set; }
    }
}
