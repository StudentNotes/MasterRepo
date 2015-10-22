using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudentNotes.Logic.Authorization;

namespace StudentNotesWeb.Tests
{
    [TestClass]
    public class StudentNotesUserTest
    {
        [TestMethod]
        public void TestUserAuth()
        {
            StudentNotesUser serviceUser = new StudentNotesUser("test@mail.com", "s3cr3tpass");
            
            Assert.IsFalse(serviceUser.UserExistsInDatabase());

            serviceUser.SaveUserInDatabase();

            Assert.IsTrue(serviceUser.UserExistsInDatabase());

        }
    }
}
