using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentNotes.Logic.ServiceInterfaces;

namespace StudentNotes.Logic.LogicAbstraction
{
    public abstract class RequestModelBase
    {
        protected readonly ISchoolService SchoolService;

        public bool IsValid { get; set; }

        protected RequestModelBase() { }

        protected RequestModelBase(ISchoolService schoolService)
        {
            SchoolService = schoolService;
        }

        public abstract ResponseViewModelBase Validate();
    }
}
