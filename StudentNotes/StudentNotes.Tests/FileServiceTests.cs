using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudentNotes.Logic.Models;
using StudentNotes.Logic.ServiceInterfaces;

namespace StudentNotes.Tests
{
    /// <summary>
    /// Summary description for FileServiceTests
    /// </summary>
    [TestClass]
    public class FileServiceTests
    {
        public FileServiceTests()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestMethod1()
        {
            var fileService = NinjectResolver.GetInstance<IFileService>();
            Assert.AreNotEqual(0, fileService.GetRecentlyAddedFiles(2).Count());
            //
            // TODO: Add test logic here
            //
        }
    }
}
