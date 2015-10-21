//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace StudentNotes.Logic.DBModels
{
    using System;
    using System.Collections.Generic;
    
    public partial class File
    {
        public File()
        {
            this.FileSharedGroup = new HashSet<FileSharedGroup>();
            this.UserSharedFile = new HashSet<UserSharedFile>();
        }
    
        public int FileId { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Path { get; set; }
        public string Size { get; set; }
        public System.DateTime UploadDate { get; set; }
        public bool IsShared { get; set; }
        public string FileTags { get; set; }
        public int UserId { get; set; }
    
        public virtual User User { get; set; }
        public virtual ICollection<FileSharedGroup> FileSharedGroup { get; set; }
        public virtual ICollection<UserSharedFile> UserSharedFile { get; set; }
    }
}
