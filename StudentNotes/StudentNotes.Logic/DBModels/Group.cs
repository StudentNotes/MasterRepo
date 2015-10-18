//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace StudentNotesDal.DBModels
{
    using System;
    using System.Collections.Generic;
    
    public partial class Group
    {
        public Group()
        {
            this.FileSharedGroup = new HashSet<FileSharedGroup>();
            this.GroupSemester = new HashSet<GroupSemester>();
        }
    
        public int GroupId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public int AdminId { get; set; }
    
        public virtual ICollection<FileSharedGroup> FileSharedGroup { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<GroupSemester> GroupSemester { get; set; }
    }
}