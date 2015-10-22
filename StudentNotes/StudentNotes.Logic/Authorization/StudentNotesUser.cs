using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Permissions;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using StudentNotes.Logic.DBModels;

namespace StudentNotes.Logic.Authorization
{
    public class StudentNotesUser
    {
        private int ID;
        private string _email;
        private string _plainTextPassword;
        private Guid _userGuid;

        public StudentNotesUser(string email, string password)
        {
            _email = email;
            _plainTextPassword = password;
        }

        private string EncryptPassword(string decryptedPassword, Guid salt)
        {
            string hashedPassword;
            string passwordAndSalt = decryptedPassword + salt;

            byte[] byteArray = Encoding.Unicode.GetBytes(passwordAndSalt);
            using (var sha256 = SHA256.Create())
            {
                hashedPassword = sha256.ComputeHash(byteArray).ToString();
            }

            return hashedPassword;
        }

        public bool UserExistsInDatabase()
        {
            using (robson081192_StudentNotesDBEntities context = new robson081192_StudentNotesDBEntities())
            {
                var users = from u in context.User where u.Email == _email select u;
                if (users.Any())
                {
                    return true;
                }
            }
            return false;
        }

        public int SaveUserInDatabase()
        {
            if (!UserExistsInDatabase())
            {
                _userGuid = Guid.NewGuid();

                using (robson081192_StudentNotesDBEntities context = new robson081192_StudentNotesDBEntities())
                {
                    try
                    {
                        context.User.Add(new User()
                        {
                            Email = _email,
                            Password = EncryptPassword(_plainTextPassword, _userGuid),
                            Salt = _userGuid
                        });
                        context.UserInfo.Add(new UserInfo()
                        {
                            CreatedOn = DateTime.Today
                        });
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Error while updating the database...sorry");
                    }
                }
            }

            return 1;   //  User exists allready in database :/
        }

        public bool IsServiceUser()
        {
            using (robson081192_StudentNotesDBEntities context = new robson081192_StudentNotesDBEntities())
            {
                var users = from u in context.User where u.Email == _email select u;
                if (users.Any())
                {
                    User user = users.First();
                    if (user.Password == EncryptPassword(_plainTextPassword, (Guid)user.Salt))
                    {
                        return true;
                    }
                    return false;
                }
                return false;
            }
        }

        public int GetStudentNotesUserId()
        {
            return ID;
        }
    }
}
