using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudentNotesWeb.Models.ResourcesFinderLogic;

namespace StudentNotesWeb.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            ResourceFinder resourceFinder = new ResourceFinder();
            string resourceValue = ResourceFinder.GetResource("resourceKey_1");
            ResourceFinder.ChangeResourceLanguage("de");
        }
    }
}
