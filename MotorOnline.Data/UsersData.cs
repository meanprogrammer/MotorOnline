using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using MotorOnline.Library.Entity;
using System.Data.Common;
using System.Data;

namespace MotorOnline.Data
{
    public class UsersData
    {
        Database db;
        public UsersData() {
            db = DatabaseFactory.CreateDatabase();
        }

        public bool SaveUser(User user)
        {
            DbCommand cmd = db.GetStoredProcCommand("sp_saveuser");
            int result = 0;
            using(cmd)
	        {
                db.AddInParameter(cmd,"@Username", System.Data.DbType.String, user.Username);
                db.AddInParameter(cmd,"@Password",  System.Data.DbType.String, user.Password);
                db.AddInParameter(cmd,"@LastName", System.Data.DbType.String, user.LastName);
                db.AddInParameter(cmd,"@FirstName", System.Data.DbType.String, user.FirstName);
                db.AddInParameter(cmd,"@MI",  System.Data.DbType.String, user.MI);
                db.AddInParameter(cmd,"@RoleID", System.Data.DbType.Int32, user.RoleID);
                db.AddInParameter(cmd,"@LastActivityDate", System.Data.DbType.DateTime, user.LastActivityDate);
                result = db.ExecuteNonQuery(cmd);
	        }
            return result > 0;
        }

        public User AuthenticateUser(string username, string password)
        {
            DbCommand cmd = db.GetStoredProcCommand("sp_authenticateuser");
            db.AddInParameter(cmd, "@Username", System.Data.DbType.String, username);
            db.AddInParameter(cmd, "@Password", System.Data.DbType.String, password);

            IDataReader reader = db.ExecuteReader(cmd);
            User user = null;
            using (reader)
            {
                int userIdIdx = reader.GetOrdinal("UserID");
                int usernameIdx = reader.GetOrdinal("Username");
                int firsnameIdx = reader.GetOrdinal("FirstName");
                int miIdx = reader.GetOrdinal("MI");
                int lastnameIdx = reader.GetOrdinal("LastName");
                int lastactivityIdx = reader.GetOrdinal("LastActivityDate");
                int roleNameIdx = reader.GetOrdinal("RoleName");
                int canAddTransactionIdx = reader.GetOrdinal("CanAddTransaction");
                int canEditTransactionIdx = reader.GetOrdinal("CanEditTransaction");
                int canViewTransactionIdx = reader.GetOrdinal("CanViewTransaction");
                int canDeleteTransactionIdx = reader.GetOrdinal("CanDeleteTransaction");
                int canPostTransactionIdx = reader.GetOrdinal("CanPostTransaction");
                int canAddUserIdx = reader.GetOrdinal("CanAddUser");
                int canEditUserIdx = reader.GetOrdinal("CanEditUser");
                int canDeleteUserIdx = reader.GetOrdinal("CanDeleteUser");
                int canEditPerilsIdx = reader.GetOrdinal("CanEditPerils");
                int canEndorseIdx = reader.GetOrdinal("CanEndorse");

                while (reader.Read())
                {
                    user = new User();
                    user.UserID = reader.GetInt32(userIdIdx);
                    user.Username = reader.GetString(usernameIdx);
                    user.FirstName = reader.IsDBNull(firsnameIdx) ? string.Empty : reader.GetString(firsnameIdx);
                    user.LastName = reader.IsDBNull(lastactivityIdx) ? string.Empty : reader.GetString(lastnameIdx);
                    user.MI = reader.IsDBNull(miIdx) ? string.Empty : reader.GetString(miIdx);

                    user.UserRole = new UserRole()
                    {
                        RoleName = reader.GetString(roleNameIdx),
                        CanAddTransaction = reader.GetBoolean(canAddTransactionIdx),
                        CanEditTransaction = reader.GetBoolean(canEditTransactionIdx),
                        CanViewTransaction = reader.GetBoolean(canViewTransactionIdx),
                        CanDeleteTransaction = reader.GetBoolean(canDeleteTransactionIdx),
                        CanPostTransaction = reader.GetBoolean(canPostTransactionIdx),

                        CanAddUser = reader.GetBoolean(canAddUserIdx),
                        CanEditUser = reader.GetBoolean(canEditUserIdx),
                        CanDeleteUser = reader.GetBoolean(canDeleteUserIdx),
                        CanEditPerils = reader.GetBoolean(canEditPerilsIdx),
                        CanEndorse = reader.GetBoolean(canEndorseIdx)
                    };
                }
            }
            cmd.Dispose();
            return user;
        }

        public bool UpdateUser(User user)
        {
            DbCommand cmd = db.GetStoredProcCommand("sp_updateuser");
            int result = 0;
            using (cmd)
            {
                db.AddInParameter(cmd, "@Username", DbType.String, user.Username);
                db.AddInParameter(cmd, "@LastName", DbType.String, user.LastName);
                db.AddInParameter(cmd, "@FirstName", DbType.String, user.FirstName);
                db.AddInParameter(cmd, "@MI", DbType.String, user.MI);
                db.AddInParameter(cmd, "@RoleID", DbType.Int32, user.RoleID);
                db.AddInParameter(cmd, "@UserID", DbType.Int32, user.UserID);
                result = db.ExecuteNonQuery(cmd);
            }
            return result > 0;
        }

        public bool DeleteUser(int userId)
        {
            DbCommand cmd = db.GetStoredProcCommand("sp_deleteuser");
            int result = 0;
            using (cmd)
            {
                db.AddInParameter(cmd, "@UserID", DbType.Int32, userId);
                result = db.ExecuteNonQuery(cmd);
            }
            return result > 0;
        }

        public List<User> GetAllUsers()
        {
            DbCommand cmd = db.GetStoredProcCommand("sp_getallusers");
            List<User> users = new List<User>();

            IDataReader reader = db.ExecuteReader(cmd);
            User user = null;
            using (reader)
            {
                int userIdIdx = reader.GetOrdinal("UserID");
                int usernameIdx = reader.GetOrdinal("Username");
                int firsnameIdx = reader.GetOrdinal("FirstName");
                int miIdx = reader.GetOrdinal("MI");
                int lastnameIdx = reader.GetOrdinal("LastName");
                int lastactivityIdx = reader.GetOrdinal("LastActivityDate");
                int roleIdIdx = reader.GetOrdinal("RoleID");
                int roleNameIdx = reader.GetOrdinal("RoleName");
                int canAddTransactionIdx = reader.GetOrdinal("CanAddTransaction");
                int canEditTransactionIdx = reader.GetOrdinal("CanEditTransaction");
                int canViewTransactionIdx = reader.GetOrdinal("CanViewTransaction");
                int canDeleteTransactionIdx = reader.GetOrdinal("CanDeleteTransaction");
                int canPostTransactionIdx = reader.GetOrdinal("CanPostTransaction");
                int canAddUserIdx = reader.GetOrdinal("CanAddUser");
                int canEditUserIdx = reader.GetOrdinal("CanEditUser");
                int canDeleteUserIdx = reader.GetOrdinal("CanDeleteUser");
                int canEditPerilsIdx = reader.GetOrdinal("CanEditPerils");
                int canEndorseIdx = reader.GetOrdinal("CanEndorse");

                while (reader.Read())
                {
                    user = new User();
                    user.UserID = reader.GetInt32(userIdIdx);
                    user.Username = reader.GetString(usernameIdx);
                    user.FirstName = reader.IsDBNull(firsnameIdx) ? string.Empty : reader.GetString(firsnameIdx);
                    user.LastName = reader.IsDBNull(lastactivityIdx) ? string.Empty : reader.GetString(lastnameIdx);
                    user.MI = reader.IsDBNull(miIdx) ? string.Empty : reader.GetString(miIdx);
                    user.LastActivityDate = reader.GetDateTime(lastactivityIdx);
                    user.RoleID = reader.GetInt32(roleIdIdx);
                    user.UserRole = new UserRole()
                    {
                        RoleName = reader.GetString(roleNameIdx),
                        CanAddTransaction = reader.GetBoolean(canAddTransactionIdx),
                        CanEditTransaction = reader.GetBoolean(canEditTransactionIdx),
                        CanViewTransaction = reader.GetBoolean(canViewTransactionIdx),
                        CanDeleteTransaction = reader.GetBoolean(canDeleteTransactionIdx),
                        CanPostTransaction = reader.GetBoolean(canPostTransactionIdx),

                        CanAddUser = reader.GetBoolean(canAddUserIdx),
                        CanEditUser = reader.GetBoolean(canEditUserIdx),
                        CanDeleteUser = reader.GetBoolean(canDeleteUserIdx),
                        CanEditPerils = reader.GetBoolean(canEditPerilsIdx),
                        CanEndorse = reader.GetBoolean(canEndorseIdx)
                    };
                    users.Add(user);
                }
            }
            cmd.Dispose();
            return users;
        }

        public List<DropDownListItem> GetRolesOptions()
        {
            DbCommand cmd = db.GetStoredProcCommand("sp_getrolesoption");
            IDataReader reader = db.ExecuteReader(cmd);
            List<DropDownListItem> roles = new List<DropDownListItem>();
            using (reader)
            {
                int valueIdx = reader.GetOrdinal("VALUE");
                int textIdx = reader.GetOrdinal("TEXT");
                while (reader.Read())
                {
                    DropDownListItem mg = new DropDownListItem();
                    mg.Value = reader.GetInt32(valueIdx).ToString();
                    mg.Text = reader.GetString(textIdx);

                    roles.Add(mg);
                }
            }
            cmd.Dispose();
            return roles;
        }
    }
}
