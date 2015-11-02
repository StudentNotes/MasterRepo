namespace StudentNotes.Repositories.DbModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FileSharedGroup")]
    public partial class FileSharedGroup
    {
        public int FileSharedGroupId { get; set; }

        public int FileId { get; set; }

        public int GroupId { get; set; }

        public int SemesterSubjectId { get; set; }

        public virtual File File { get; set; }

        public virtual Group Group { get; set; }

        public virtual SemesterSubject SemesterSubject { get; set; }
    }
}
