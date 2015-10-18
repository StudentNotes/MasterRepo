using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;

namespace StudentNotes.DependencyResolver.Modules
{
    public class FileTransferManagerModule : NinjectModule
    {
        public override void Load()
        {
            throw new NotImplementedException();
            //Bind<IBottomClass>().To<BottomClass>();
        }
    }
}
