using System;
using System.Security.Cryptography;
using System.Text;
using StudentNotes.Logic.LogicModels;
using StudentNotes.Logic.ServiceInterfaces;
using StudentNotes.Logic.ViewModels.Common;
using StudentNotes.Repositories.DbModels;
using StudentNotes.Repositories.Infrastructure;
using StudentNotes.Repositories.RepositoryInterfaces;

namespace StudentNotes.Logic.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserInfoRepository _userInfoRepository;
        private readonly IUserPreferencesRepository _userPreferencesRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUserRepository userRepository, IUserInfoRepository userInfoRepository, IUserPreferencesRepository userPreferencesRepository,  IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _userInfoRepository = userInfoRepository;
            _userPreferencesRepository = userPreferencesRepository;
            _unitOfWork = unitOfWork;
        }
        public bool UserExists(string email)
        {
            var user = _userRepository.Get(u => u.Email == email);
            return user != null;
        }

        public bool UserExists(int userId)
        {
            var user = _userRepository.GetById(userId);
            return user != null;
        }

        public bool UserAuthorized(string email, string password)
        {
            var user = _userRepository.Get(u => u.Email == email);
            if (user != null) return user.Salt != null && user.Password == EncryptPassword(password, (Guid) user.Salt);
            return false;
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
                    CreatedOn = DateTime.Now,
                    Gender = "undefined"
                },
                UserPreferences = new UserPreferences()
                {
                    LastUploadDays = 30,
                    MaxFileSize = "5242880",
                    SearchMethod = "mixed"
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
                    CreatedOn = DateTime.Now,
                    Gender = "undefined"
                },
                UserPreferences = new UserPreferences()
                {
                    LastUploadDays = 30,
                    MaxFileSize = "5242880",
                    SearchMethod = "mixed"
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

        public UserPreferencesViewModel GetUserPreferences(int userId)
        {
            var user = _userRepository.GetById(userId);
            var userPreferences = new UserPreferencesViewModel(user);

            return userPreferences;
        }

        public void AddAvatar(int userId, string path)
        {
            var user = _userInfoRepository.GetById(userId);
            user.PicturePath = path;
            _userInfoRepository.Commit();
        }

        public bool UpdateUserInfo(SecureUserModel model)
        {
            var dataChanged = false;
            var user = _userRepository.GetById(model.UserId);
      
            if (user.UserInfo.Name != model.Name)
            {
                user.UserInfo.Name = model.Name;
                dataChanged = true;
            }
            if (user.UserInfo.LastName != model.LastName)
            {
                user.UserInfo.LastName = model.LastName;
                dataChanged = true;
            }
            if (user.UserInfo.Gender != model.Gender)
            {
                user.UserInfo.Gender = model.Gender;
                dataChanged = true;
            }
            if (user.UserInfo.Profession != model.Profession)
            {
                user.UserInfo.Profession = model.Profession;
                dataChanged = true;
            }
            if (user.UserInfo.PhoneNumber != model.PhoneNumber)
            {
                user.UserInfo.PhoneNumber = model.PhoneNumber;
                dataChanged = true;
            }
            if (user.UserInfo.PostalCode != model.PostalCode)
            {
                user.UserInfo.PostalCode = model.PostalCode;
                dataChanged = true;
            }
            if (user.UserInfo.City != model.City)
            {
                user.UserInfo.City = model.City;
                dataChanged = true;
            }
            if (user.UserInfo.Street != model.Street)
            {
                user.UserInfo.Street = model.Street;
                dataChanged = true;
            }

            if (user.UserInfo.Country != model.Country)
            {
                user.UserInfo.Country = model.Country;
                dataChanged = true;
            }
   
            _unitOfWork.Commit();

            return dataChanged;
        }

        public bool UpdateUserPreferences(UserPreferencesViewModel model)
        {
            var preferencesChanged = false;

            var user = _userRepository.GetById(model.UserId);

            if (user.UserPreferences.LastUploadDays != model.NewestTime)
            {
                user.UserPreferences.LastUploadDays = model.NewestTime;
                preferencesChanged = true;
            }
            //if (user.UserPreferences.MaxFileSize != model.FileSize)
            //{
            //    user.UserPreferences.MaxFileSize = model.FileSize;
            //    preferencesChanged = true;
            //}

            if (user.UserPreferences.SearchMethod != model.SearchMethod)
            {
                user.UserPreferences.SearchMethod = model.SearchMethod;
                preferencesChanged = true;
            }

            _unitOfWork.Commit();

            return preferencesChanged;
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
