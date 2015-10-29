using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentNotes.Logic.Consts
{
    public static class LogicConstants
    {
        public static string FtpServerAddress { get { return "91.219.122.70"; } }
        public static string FtpUserLogin { get { return "robson081192"; } }
        public static string FtpUserPassword { get { return "pck5LT099r"; } }

        public static string GetFtpServerFileTarget(string email, string targetDirectory)
        {
            return string.Format("/FTP/{0}/{1}", email, targetDirectory);
        }
    }
}
