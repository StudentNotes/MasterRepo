namespace StudentNotes.Repositories.DbModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SemesterSubjectFile")]
    public partial class SemesterSubjectFile
    {
        public int SemesterSubjectFileId { get; set; }

        public int SemesterSubjectId { get; set; }

        public int FileId { get; set; }

        public virtual File File { get; set; }

        public virtual SemesterSubject SemesterSubject { get; set; }
    }
}
