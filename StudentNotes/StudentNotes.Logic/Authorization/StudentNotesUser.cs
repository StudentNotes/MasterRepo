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
        public string Name
        {
            get { return _name; }
        }

        public string LastName
        {
            get
            {
                return _lastName;
            }
        }

        public bool IsServiceAdmin
        {
            get { return _isServiceAdmin; }
        }

        private int ID;
        private string _email;
        private string _plainTextPassword;
        private string _name;
        private string _lastName;
        private Guid _userGuid;
        private bool _isServiceAdmin;

        public StudentNotesUser(string email, string password, bool isServiceAdmin)
        {
            _email = email;
            _plainTextPassword = password;
            _isServiceAdmin = isServiceAdmin;
        }

        public StudentNotesUser(string email, string password)
        {
            _email = email;
            _plainTextPassword = password;
            _isServiceAdmin = false;
        }

        public StudentNotesUser()
        {
        }

        private string EncryptPassword(string decryptedPassword, Guid salt)
        {
            string hashedPassword;
            string passwordAndSalt = decryptedPassword + salt;

            byte[] byteArray = Encoding.Unicode.GetBytes(passwordAndSalt);
            using (var sha256 = SHA256.Create())
            {
                byte[] hashedByteArray = sha256.ComputeHash(byteArray);
                hashedPassword = System.Text.Encoding.Unicode.GetString(hashedByteArray);
            }

            return hashedPassword;
        }

        public bool UserExistsInDatabase()
        {
            using (StudentNotesContext context = new StudentNotesContext())
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

                using (StudentNotesContext context = new StudentNotesContext())
                {
                    try
                    {
                        User newStudentNotesUser = new User()
                        {
                            Email = _email,
                            Password = EncryptPassword(_plainTextPassword, _userGuid),
                            Salt = _userGuid,
                            IsServiceAdmin = _isServiceAdmin
                        };
                        context.User.Add(newStudentNotesUser);

                        context.UserInfo.Add(new UserInfo()
                        {
                            UserId = context.User.Local.ToArray().Last().UserId,
                            CreatedOn = DateTime.Now
                        });

                        context.SaveChanges();
                        return 0;
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
            using (StudentNotesContext context = new StudentNotesContext())
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
            using (var context = new StudentNotesContext())
            {
                var users = from u in context.User where u.Email == _email select u;
                var currentUser = users.First();
                ID = currentUser.UserId;
            }
            return ID;
        }

        public void SetModelName()
        {
            using (var context = new StudentNotesContext())
            {
                var users = from u in context.User where u.Email == _email select u;
                var currentUser = users.First();

                if (currentUser.UserInfo.Name == null)
                {
                    _name = "nieznajomy";
                }
                else
                {
                    _name = currentUser.UserInfo.Name;
                }
                if (currentUser.UserInfo.LastName == null)
                {
                    _lastName = "";
                }
                else
                {
                    _lastName = currentUser.UserInfo.LastName;
                }
            }
        }
        public void SetModelName(int userId)
        {
            using (var context = new StudentNotesContext())
            {
                var users = from u in context.User where u.UserId == userId select u;
                var currentUser = users.First();

                if (currentUser.UserInfo.Name == null)
                {
                    _name = "nieznajomy";
                }
                else
                {
                    _name = currentUser.UserInfo.Name;
                }
                if (currentUser.UserInfo.LastName == null)
                {
                    _lastName = "";
                }
                else
                {
                    _lastName = currentUser.UserInfo.LastName;
                }
                _isServiceAdmin = currentUser.IsServiceAdmin;
            }
        }
    }
}
