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
    
    public partial class GroupUser
    {
        public int GroupUserId { get; set; }
        public int UserId { get; set; }
        public int GroupSemesterId { get; set; }
    
        public virtual GroupSemester GroupSemester { get; set; }
        public virtual User User { get; set; }
    }
}