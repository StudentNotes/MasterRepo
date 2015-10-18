using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudentNotes.FileTransferManager.FtpClient;
using StudentNotes.FileTransferManager.FtpClient.FileTypes;

namespace StudentNotes.Web.Tests
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
            FtpServer server = new FtpServer("91.219.122.70", "/FTP/ET - Prazan stan.mp3");
            FtpUser user = new FtpUser("robson081192", "pck5LT099r", server);

            CommonFile file = new CommonFile(@"C:\Users\Robson\Desktop\Test_FTP_dll\ET - Prazan stan.mp3");
            user.DownloadFile(file);

            file.SafeContentOnDrive();
        }

        [TestMethod]
        public void TestFileUpload()
        {
            //  83.10.110.250 
            FtpServer server = new FtpServer("91.219.122.70", "/FTP/ET - Prazan stan.mp3");
            FtpUser user = new FtpUser("robson081192", "pck5LT099r", server);

            CommonFile file = new CommonFile(@"C:\Users\Robson\Desktop\Test_FTP_dll\ET - Prazan stan.mp3");
            user.UploadFile(file);
        }

        [TestMethod]
        public void TestFileDelete()
        {
            //  83.10.110.250 
            FtpServer server = new FtpServer("91.219.122.70", "/FTP/ET - Prazan stan.mp3");
            FtpUser user = new FtpUser("robson081192", "pck5LT099r", server);

            user.DeleteFile();

            return;
        }
    }
}
