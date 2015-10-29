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
            StudentNotesUser serviceUser = new StudentNotesUser("test1@mail.com", "dupa");
            
            //Assert.IsFalse(serviceUser.UserExistsInDatabase());

            //serviceUser.SaveUserInDatabase();

            //Assert.IsTrue(serviceUser.UserExistsInDatabase());

            serviceUser.IsServiceUser();

        }
    }
}
