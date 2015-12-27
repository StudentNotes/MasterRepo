using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;
using StudentNotes.FileManager.Base;
using StudentNotes.FileManager.FtpClient;

namespace StudentNotes.DependencyResolver.Modules
{
    public class FileTransferManagerModule : NinjectModule
    {
        public override void Load()
        {
            //Bind<FileServerFile>().To<CommonFile>();
            //Bind<FileServer>().To<FtpServer>();
            //Bind<FileServerUser>().To<FtpUser>();
            Bind<FileServerClient>().To<FtpClient>();
        }
    }
}
