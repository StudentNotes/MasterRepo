using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StudentNotes.Logic.ViewModels.Validation
{
    public class FormInputValidator
    {
        private static Regex _emailRegex = new Regex(@"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$");
        private static Regex _passwordRegex = new Regex(@"(?!^[0-9]*$)(?!^[a-zA-Z]*$)^([a-zA-Z0-9]{8,50})$");

        public static string ValidateEmail(string email)
        {
            if (string.IsNullOrEmpty(email) || email.Length == 0)
            {
                return "Nie wprowadziłeś adresu email";
            }
            return _emailRegex.IsMatch(email) ? null : "Wprowadziłeś nieprawidłowy adres email.";
        }

        public static string ValidatePassword(string password)
        {
            if (string.IsNullOrEmpty(password) || password.Length == 0)
                return "Nie wprowadziłeś hasła";

            if (_passwordRegex.IsMatch(password))
            {
                return null;
            }
            return "Wprowadziłeś niepoprawne hasło. Prawidłowe hasło składa sie przynajmniej z 8 znaków i musi zawierać przynajmniej jedną cyfrę, literę i nie może zawierać znaków specjalnych.";
        }

        public static string ValidatePasswordConfirmation(string password, string confirmedPassword)
        {
            if (string.IsNullOrEmpty(confirmedPassword) || confirmedPassword.Length == 0)
            {
                return "Nie potwierdziłeś hasła";
            }
            if (password == confirmedPassword)
            {
                return null;
            }
            return "Hasła nie są identyczne!";
        }
    }
}
