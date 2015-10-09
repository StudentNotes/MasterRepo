using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudentNotesFileTransferManager.FtpClient;
using StudentNotesFileTransferManager.FtpClient.FileTypes;

namespace StudentNotesWeb.Tests
{
    [TestClass]
    public class FtpClientTest
    {
        [TestMethod]
        public void GlobalUnitTest()
        {
            Assert.AreEqual(0, TestServerCreation());
            Assert.AreEqual(0, TestClientCreation());
        }

        [TestMethod]
        public int TestClientCreation()
        {
            FtpServer server = new FtpServer("192.168.1.1");
            FtpUser user = new FtpUser("robson", "4019551", server);

            return user.ToString() == "Ftp user credentials\nlogin: robson\npassword: 4019551\nserver address: 192.168.1.1" ? 0 : 1;
        }

        [TestMethod]
        public int TestServerCreation()
        {
            FtpServer server = new FtpServer("192.168.1.1");

            return server.ToString() == "This file server uses the FTP protocol, and it's Url is: 192.168.1.1" ? 0 : 1;
        }

        [TestMethod]
        public void TestFileDownload()
        {
            FtpServer server = new FtpServer("192.168.1.1");
            FtpUser user = new FtpUser("robson", "4019551", server);

            CommonFile file = new CommonFile("ftp_info.txt", "/FTP/");
            user.DownloadFile(file);

            return;
        }
    }
}
