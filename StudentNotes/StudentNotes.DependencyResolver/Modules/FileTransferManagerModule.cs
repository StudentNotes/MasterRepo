using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;
using StudentNotes.FileTransferManager.Base;
using StudentNotes.FileTransferManager.FtpClient;
using StudentNotes.FileTransferManager.FtpClient.FileTypes;

namespace StudentNotes.DependencyResolver.Modules
{
    public class FileTransferManagerModule : NinjectModule
    {
        public override void Load()
        {
            Bind<FileServerFile>().To<CommonFile>();
            Bind<FileServer>().To<FtpServer>();
            Bind<FileServerUser>().To<FtpUser>();
        }
    }
}
