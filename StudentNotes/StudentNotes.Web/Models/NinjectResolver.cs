using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;

namespace StudentNotes.Web.Models
{
    public static class NinjectResolver
    {
        private static IKernel _kernel;

        public static IKernel GetKernel()
        {
            return _kernel ?? (_kernel = new StandardKernel());
        }

        public static T GetInstance<T>()
        {
            return _kernel.Get<T>();
        }

    }
}
