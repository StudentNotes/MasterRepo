using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentNotes.Logic.LogicAbstraction
{
    public abstract class ResponseViewModelBase
    {
        public List<string> ErrorList { get; set; }
        public List<string> SuccessList { get; set; }

        protected ResponseViewModelBase()
        {
            ErrorList = new List<string>();
            SuccessList = new List<string>();
        }

        public void AddError(string errorMessage)
        {
            ErrorList.Add(errorMessage);
        }

        public void AddSuccess(string successMessage)
        {
            SuccessList.Add(successMessage);
        }
    }
}
