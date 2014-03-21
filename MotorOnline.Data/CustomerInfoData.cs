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
    public class CustomerInfoData
    {
        Database db;
        public CustomerInfoData() 
        {
            db = DatabaseFactory.CreateDatabase();
        }

        public List<CustomerInfo> GetNamesAutocomplete()
        {
            DbCommand cmd = db.GetStoredProcCommand("sp_getnamesautocomplete");
            List<CustomerInfo> list = new List<CustomerInfo>();
            using (cmd)
            {
                IDataReader reader = db.ExecuteReader(cmd);
                using (reader)
                {
                    int recordIdIdx = reader.GetOrdinal("RecordID");
                    int firstNameIdx = reader.GetOrdinal("FirstName");
                    int lastNameIdx = reader.GetOrdinal("LastName");
                    int middleNameIdx = reader.GetOrdinal("MiddleName");
                    int addressIdx = reader.GetOrdinal("Address");
                    int telephoneIdx = reader.GetOrdinal("Telephone");
                    int mobileNoIdx = reader.GetOrdinal("MobileNo");
                    int emailIdx = reader.GetOrdinal("Email");
                    int designationIdx = reader.GetOrdinal("Designation");
                    while (reader.Read())
                    {
                        CustomerInfo ap = new CustomerInfo();
                        ap.CustomerID = reader.IsDBNull(recordIdIdx) ? 0 : reader.GetInt32(recordIdIdx);
                        ap.FirstName = reader.IsDBNull(firstNameIdx) ? string.Empty : reader.GetString(firstNameIdx);
                        ap.LastName = reader.IsDBNull(lastNameIdx) ? string.Empty : reader.GetString(lastNameIdx);
                        ap.MiddleName = reader.IsDBNull(middleNameIdx) ? string.Empty : reader.GetString(middleNameIdx);
                        ap.Address = reader.IsDBNull(addressIdx) ? string.Empty : reader.GetString(addressIdx);
                        ap.Telephone = reader.IsDBNull(telephoneIdx) ? string.Empty : reader.GetString(telephoneIdx);
                        ap.MobileNo = reader.IsDBNull(mobileNoIdx) ? string.Empty : reader.GetString(mobileNoIdx);
                        ap.Email = reader.IsDBNull(emailIdx) ? string.Empty : reader.GetString(emailIdx);
                        ap.Designation = reader.IsDBNull(designationIdx) ? string.Empty : reader.GetString(designationIdx);
                        list.Add(ap);
                    }
                }
            }
            return list;
        }
    }
}
