using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentNotes.Logic.ViewModels.Validation;
using StudentNotes.Repositories.DbModels;

namespace StudentNotes.Logic.ViewModels.Common
{
    public class UniversityViewModel
    {
        public List<School> Universities { get; set; }
        public ResponseMessageViewModel Response { get; set; }

        public UniversityViewModel()
        {
            Universities = new List<School>();
            Response = new ResponseMessageViewModel();
        }
    }
}
