namespace StudentNotes.Logic.DBModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Group")]
    public partial class Group
    {
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

        public virtual ICollection<FileSharedGroup> FileSharedGroup { get; set; }

        public virtual User User { get; set; }

        public virtual ICollection<GroupSemester> GroupSemester { get; set; }

        public virtual ICollection<GroupUser> GroupUser { get; set; }
    }
}
