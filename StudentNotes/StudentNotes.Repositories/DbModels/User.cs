namespace StudentNotes.Repositories.DbModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("User")]
    public partial class User
    {
        public User()
        {
            File = new HashSet<File>();
            Group = new HashSet<Group>();
            GroupUser = new HashSet<GroupUser>();
            SemesterUser = new HashSet<SemesterUser>();
            UserSharedFile = new HashSet<UserSharedFile>();
            UserVisitedSchool = new HashSet<UserVisitedSchool>();
        }

        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        public string Password { get; set; }

        public Guid? Salt { get; set; }

        public bool IsServiceAdmin { get; set; }

        public virtual ICollection<File> File { get; set; }

        public virtual ICollection<Group> Group { get; set; }

        public virtual ICollection<GroupUser> GroupUser { get; set; }

        public virtual ICollection<SemesterUser> SemesterUser { get; set; }

        public virtual ICollection<UserSharedFile> UserSharedFile { get; set; }

        public virtual ICollection<UserVisitedSchool> UserVisitedSchool { get; set; }
    }
}
