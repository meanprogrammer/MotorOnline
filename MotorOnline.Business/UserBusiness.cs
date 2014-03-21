using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MotorOnline.Library.Entity;
using MotorOnline.Data;

namespace MotorOnline.Business
{
    public class UserBusiness
    {
        UsersData data;

        public UserBusiness() 
        {
            data = new UsersData();
        }

        public bool SaveUser(User user) 
        {
            return data.SaveUser(user);
        }

        public User AuthenticateUser(string username, string password)
        {
            return data.AuthenticateUser(username, password);
        }

        public bool UpdateUser(User user) 
        {
            return data.UpdateUser(user);
        }

        public bool DeleteUser(int userId)
        {
            return data.DeleteUser(userId);
        }

        public List<User> GetAllUsers()
        {
            return data.GetAllUsers();
        }

        public List<DropDownListItem> GetRolesOptions()
        {
            return data.GetRolesOptions();
        }
    }
}
