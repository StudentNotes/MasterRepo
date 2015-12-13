using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudentNotes.Logic.ResourcesFinderLogic;
using StudentNotes.Logic.ViewModels.Validation;

namespace StudentNotes.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void ResourceResolverTestMethod()
        {
            ResourceFinder resourceFinder = new ResourceFinder();
            string resourceValue = ResourceFinder.GetResource("resourceKey_1");
            ResourceFinder.ChangeResourceLanguage("de");
        }

        [TestMethod]
        public void FormInputValidatorTestMethod()
        {
            Assert.IsNotNull(FormInputValidator.ValidateEmail("*@mail.com"));
            Assert.AreEqual("Nie wprowadziłeś adresu email", FormInputValidator.ValidateEmail(""));

            Assert.IsNull(FormInputValidator.ValidatePassword("3Cytrynki"));
            Assert.IsNull(FormInputValidator.ValidatePassword("3cytrynki"));
            Assert.IsNotNull(FormInputValidator.ValidatePassword("12345678"));
            Assert.IsNotNull(FormInputValidator.ValidatePassword("3cytryn"));
            Assert.IsNotNull(FormInputValidator.ValidatePassword("cytrynki"));

            Assert.AreEqual("Nie wprowadziłeś hasła", FormInputValidator.ValidatePassword(""));
            Assert.AreEqual("Wprowadziłeś niepoprawne hasło. Prawidłowe hasło składa sie przynajmniej z 8 znaków i musi zawierać przynajmniej jedną cyfrę, literę i nie może zawierać znaków specjalnych.", 
                FormInputValidator.ValidatePassword("haslo"));
            Assert.AreEqual("Wprowadziłeś niepoprawne hasło. Prawidłowe hasło składa sie przynajmniej z 8 znaków i musi zawierać przynajmniej jedną cyfrę, literę i nie może zawierać znaków specjalnych.",
                FormInputValidator.ValidatePassword("12345678"));

            Assert.IsNull(FormInputValidator.ValidatePasswordConfirmation("haslo", "haslo"));
            Assert.AreEqual("Nie potwierdziłeś hasła", FormInputValidator.ValidatePasswordConfirmation("hasło", ""));
            Assert.AreEqual("Hasła nie są identyczne!", FormInputValidator.ValidatePasswordConfirmation("haslo", "hasło"));

        }
    }
}
