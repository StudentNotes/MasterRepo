﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using StudentNotes.Repositories.DbModels;

namespace StudentNotes.Logic.ServiceInterfaces
{
    public interface IUserService
    {
        bool UserExists(string email);
        bool UserAuthorized(string email, string password);
        bool IsServiceAdmin(int userId);
        int AddUser(string email, string password);
        int AddAdmin(string email, string password);
        int GetServiceUserId(string email);
        string GetServiceUserName(int userid);
        string GetServiceUserLastName(int userId);
        UserInfo GetAllServiceUserInfo(int userId);
        void SaveUser();
    }
}