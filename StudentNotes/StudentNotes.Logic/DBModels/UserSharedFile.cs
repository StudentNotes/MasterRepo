namespace StudentNotes.Logic.DBModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UserSharedFile")]
    public partial class UserSharedFile
    {
        public int UserSharedFileId { get; set; }

        public int UserId { get; set; }

        public int FileId { get; set; }

        public virtual File File { get; set; }

        public virtual User User { get; set; }
    }
}
