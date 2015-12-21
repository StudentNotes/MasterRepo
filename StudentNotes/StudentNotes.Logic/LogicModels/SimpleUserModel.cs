using System.Web.WebPages;
using StudentNotes.Repositories.DbModels;

namespace StudentNotes.Logic.LogicModels
{
    public class SimpleUserModel
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string FullName { get; private set; }

        public SimpleUserModel() { }

        public SimpleUserModel(User user)
        {
            UserId = user.UserId;
            Email = user.Email;
            Name = user.UserInfo.Name;
            LastName = user.UserInfo.LastName;
            if (Name.IsEmpty() && LastName.IsEmpty())
            {
                FullName = "Nieznajomy";
            }
            else
            {
                FullName = $"{Name} {LastName}";
            }
        }
    }
}
