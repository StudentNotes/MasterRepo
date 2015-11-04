using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using StudentNotes.Logic.ServiceInterfaces;
using StudentNotes.Repositories.DbModels;
using StudentNotes.Repositories.Infrastructure;
using StudentNotes.Repositories.RepositoryInterfaces;

namespace StudentNotes.Logic.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserInfoRepository _userInfoRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUserRepository userRepository, IUserInfoRepository userInfoRepository,IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _userInfoRepository = userInfoRepository;
            _unitOfWork = unitOfWork;
        }
        public bool UserExists(string email)
        {
            var user = _userRepository.Get(u => u.Email == email);
            if (user == null)
            {
                return false;
            }
            return true;
        }

        public bool UserAuthorized(string email, string password)
        {
            var user = _userRepository.Get(u => u.Email == email);
            if (user == null)
            {
                return false;;
            }
            if (user.Password != EncryptPassword(password, (Guid) user.Salt))
            {
                return false;
            }
            return true;
        }

        public bool IsServiceAdmin(int userId)
        {
            var user = _userRepository.Get(u => u.UserId == userId);
            return user.IsServiceAdmin;
        }

        public int AddUser(string email, string password)
        {
            var userGuid = Guid.NewGuid();

            _userRepository.AddAndSave(new User()
            {
                Email = email,
                Password = EncryptPassword(password, userGuid),
                Salt = userGuid,
                IsServiceAdmin = false,
                UserInfo = new UserInfo()
                {
                    CreatedOn = DateTime.Now
                }
            });

            return 0;
        }

        public int AddAdmin(string email, string password)
        {
            var userGuid = Guid.NewGuid();

            _userRepository.Add(new User()
            {
                Email = email,
                Password = EncryptPassword(password, userGuid),
                Salt = userGuid,
                IsServiceAdmin = true,
                UserInfo = new UserInfo()
                {
                    CreatedOn = DateTime.Now
                }
            });

            _unitOfWork.Commit();

            return 0;
        }

        public int GetServiceUserId(string email)
        {
            var user = _userRepository.Get(u => u.Email == email);
            return user.UserId;
        }

        public string GetServiceUserName(int userId)
        {
            var userInfo = _userInfoRepository.Get(ui => ui.UserId == userId);
            return userInfo.Name;
        }

        public string GetServiceUserLastName(int userId)
        {
            var userInfo = _userInfoRepository.Get(ui => ui.UserId == userId);
            return userInfo.LastName;
        }

        public UserInfo GetAllServiceUserInfo(int userId)
        {
            var userInfo = _userInfoRepository.Get(ui => ui.UserId == userId);
            return userInfo;
        }

        public void SaveUser()
        {
            _unitOfWork.Commit();
        }

        private string EncryptPassword(string decryptedPassword, Guid salt)
        {
            string hashedPassword;
            string passwordAndSalt = decryptedPassword + salt;

            byte[] byteArray = Encoding.Unicode.GetBytes(passwordAndSalt);
            using (var sha256 = SHA256.Create())
            {
                byte[] hashedByteArray = sha256.ComputeHash(byteArray);
                hashedPassword = Encoding.Unicode.GetString(hashedByteArray);
            }

            return hashedPassword;
        }
    }
}
