using System;
using StudentNotes.Logic.LogicAbstraction;
using StudentNotes.Logic.ViewModels.Validation;
using StudentNotes.Repositories.DbModels;

namespace StudentNotes.Logic.LogicModels
{
    public class SecureUserModel
    {
        public ResponseViewModelBase Response { get; set; }

        public int UserId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Profession { get; set; }
        public string PhoneNumber { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Street { get; set; }
        public string Gender { get; set; }
        public string PicturePath { get; set; }
        public DateTime CreatedOn { get; set; }

        public SecureUserModel()
        {
            Response = new ResponseMessageViewModel();
        }

        public SecureUserModel(User user)
        {
            Response = new ResponseMessageViewModel();

            UserId = user.UserId;
            Email = user.Email;
            Name = user.UserInfo.Name;
            LastName = user.UserInfo.LastName;
            Profession = user.UserInfo.Profession;
            PhoneNumber = user.UserInfo.PhoneNumber;
            Country = user.UserInfo.Country;
            City = user.UserInfo.City;
            PostalCode = user.UserInfo.PostalCode;
            Street = user.UserInfo.Street;
            PicturePath = user.UserInfo.PicturePath;
            CreatedOn = user.UserInfo.CreatedOn;
            Gender = user.UserInfo.Gender;
        }
    }
}
