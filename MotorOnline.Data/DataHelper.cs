using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace MotorOnline.Data
{
    public class DataHelper
    {
        public static void DisposeIfNotNull(DbCommand cmd) {
            if (cmd != null) { cmd.Dispose(); }
        }

        public static int SafeSQLExecuteNonQuery(Database db, string sql)
        { 
            int result = 0;
            DbCommand cmd = db.GetSqlStringCommand(sql);
            using (cmd)
            {
                result = db.ExecuteNonQuery(cmd);
            }
            return result;
        }
    }
}
