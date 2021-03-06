﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentNotes.Logic.ServiceInterfaces;

namespace StudentNotes.Logic.LogicAbstraction
{
    public abstract class RequestModelBase
    {
        public bool IsValid { get; set; }

        public abstract ResponseViewModelBase Validate();
    }
}
