namespace StudentNotes.Repositories.DbModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("File")]
    public partial class File
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public File()
        {
            FileSharedGroup = new HashSet<FileSharedGroup>();
            SemesterSubjectFile = new HashSet<SemesterSubjectFile>();
            UserSharedFile = new HashSet<UserSharedFile>();
        }

        public int FileId { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(40)]
        public string Category { get; set; }

        [StringLength(256)]
        public string Path { get; set; }

        [StringLength(15)]
        public string Size { get; set; }

        public DateTime UploadDate { get; set; }

        public bool IsShared { get; set; }

        [StringLength(256)]
        public string FileTags { get; set; }

        public int UserId { get; set; }

        public virtual User User { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FileSharedGroup> FileSharedGroup { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SemesterSubjectFile> SemesterSubjectFile { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserSharedFile> UserSharedFile { get; set; }
    }
}
