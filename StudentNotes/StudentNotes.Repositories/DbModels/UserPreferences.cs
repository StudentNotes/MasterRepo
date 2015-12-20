namespace StudentNotes.Repositories.DbModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class UserPreferences
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserId { get; set; }

        public int? LastUploadDays { get; set; }

        [StringLength(15)]
        public string MaxFileSize { get; set; }

        [StringLength(20)]
        public string SearchMethod { get; set; }

        public virtual User User { get; set; }
    }
}
