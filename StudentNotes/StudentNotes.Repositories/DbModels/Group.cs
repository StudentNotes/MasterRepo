namespace StudentNotes.Repositories.DbModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Group")]
    public partial class Group
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Group()
        {
            FileSharedGroup = new HashSet<FileSharedGroup>();
            GroupSemester = new HashSet<GroupSemester>();
            GroupUser = new HashSet<GroupUser>();
        }

        public int GroupId { get; set; }

        [Required]
        [StringLength(25)]
        public string Name { get; set; }

        [StringLength(256)]
        public string Description { get; set; }

        public DateTime CreatedOn { get; set; }

        public int AdminId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FileSharedGroup> FileSharedGroup { get; set; }

        public virtual User User { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GroupSemester> GroupSemester { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GroupUser> GroupUser { get; set; }
    }
}
