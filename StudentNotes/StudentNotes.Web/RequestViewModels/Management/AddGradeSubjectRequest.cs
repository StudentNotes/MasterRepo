using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentNotes.Logic.LogicAbstraction;
using StudentNotes.Logic.ViewModels.Validation;

namespace StudentNotes.Web.RequestViewModels.Management
{
    public class AddGradeSubjectRequest : RequestModelBase
    {
        public int GradeId { get; set; }
        public string StudySubjectName { get; set; }
        public string StudySubjectDescription { get; set; }
        public string SemesterNumber { get; set; }

        public override ResponseViewModelBase Validate()
        {
            var response = new ResponseMessageViewModel();


            IsValid = true;
            return response;
        }
    }
}
