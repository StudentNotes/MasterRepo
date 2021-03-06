﻿using StudentNotes.Logic.LogicAbstraction;
using StudentNotes.Logic.ViewModels.Authorization;

namespace StudentNotes.Web.RequestViewModels.Group
{
    public class ShowGroupSubjectsRequest : RequestModelBase
    {
        public int SemesterId { get; set; }
        public int GroupId { get; set; }
        public string SemesterName { get; set; }
        public string GroupName { get; set; }

        public override ResponseViewModelBase Validate()
        {
            LoginViewModel response = new LoginViewModel();
            IsValid = true;

            return response;
        }
    }
}
