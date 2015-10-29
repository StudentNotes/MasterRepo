using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentNotes.FileTransferManager.FtpClient.FtpBehavior
{
    public abstract class FtpBehaviorBase
    {
        public int GetResponseCode(string responseMessage)
        {
            return Int32.Parse(responseMessage.Substring(0, 3));
        }
    }
}
