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
    public class CarDetailsData
    {
        Database db;
        public CarDetailsData() 
        {
            db = DatabaseFactory.CreateDatabase();
        }

        public CarDetail GetCarDetailByTransactionID(int transactionId)
        {
            DbCommand cmd = db.GetStoredProcCommand("sp_get_cardetailbytransactionid");
            db.AddInParameter(cmd, "@TransactionID", System.Data.DbType.Int32, transactionId);
            CarDetail detail = new CarDetail();
            using (cmd)
            {
                IDataReader reader = db.ExecuteReader(cmd);
                using (reader)
                {
                    int transactionIdIdx = reader.GetOrdinal("TransactionID");
                    int carCompanyIdx = reader.GetOrdinal("CarCompany");
                    int carYearIdx = reader.GetOrdinal("CarYear");
                    int carSeriesIdx = reader.GetOrdinal("CarSeries");
                    int carMakeIdx = reader.GetOrdinal("CarMake");
                    int carTypeOfBodyIdx = reader.GetOrdinal("CarTypeOfBody");
                    int typeOfCoverIdx = reader.GetOrdinal("TypeOfCover");
                    int engineSeriesIdx = reader.GetOrdinal("EngineSeries");
                    int motorTypeIdx = reader.GetOrdinal("MotorType");
                    int engineNoIdx = reader.GetOrdinal("EngineNo");
                    int colorIdx = reader.GetOrdinal("Color");
                    int conductionNoIdx = reader.GetOrdinal("ConductionNo");
                    int chassisNoIdx = reader.GetOrdinal("ChassisNo");
                    int plateNoIdx = reader.GetOrdinal("PlateNo");
                    int accessoriesIdx = reader.GetOrdinal("Accessories");
                    int carCompanyNameIdx = reader.GetOrdinal("CarCompanyName");
                    int carMakeNameIdx = reader.GetOrdinal("CarMakeName");
                    int authenticationNoIdx = reader.GetOrdinal("AuthenticationNo");
                    int cocNoIdx = reader.GetOrdinal("COCNo");
                    int yearModelIdx = reader.GetOrdinal("yearModel");
                    int coverNameIdx = reader.GetOrdinal("coverName");
                    while (reader.Read())
                    {
                        detail.TransactionID = reader.IsDBNull(transactionIdIdx) ? 0 : reader.GetInt32(transactionIdIdx);
                        detail.TypeOfCover = reader.IsDBNull(typeOfCoverIdx) ? 0 : reader.GetInt32(typeOfCoverIdx);
                        detail.CarCompany = reader.IsDBNull(carCompanyIdx) ? 0 : reader.GetInt32(carCompanyIdx);
                        detail.CarYear = reader.IsDBNull(carYearIdx) ? 0 : reader.GetInt32(carYearIdx);
                        detail.CarSeries = reader.IsDBNull(carSeriesIdx) ? 0 : reader.GetInt32(carSeriesIdx);
                        detail.CarMake = reader.IsDBNull(carMakeIdx) ? string.Empty : reader.GetString(carMakeIdx);
                        detail.CarTypeOfBodyID = reader.IsDBNull(carTypeOfBodyIdx) ? 0 : reader.GetInt32(carTypeOfBodyIdx);
                        detail.EngineSeries = reader.IsDBNull(engineSeriesIdx) ? string.Empty : reader.GetString(engineSeriesIdx);
                        detail.MotorType = reader.IsDBNull(motorTypeIdx) ? string.Empty : reader.GetString(motorTypeIdx);
                        detail.EngineNo = reader.IsDBNull(engineNoIdx) ? string.Empty : reader.GetString(engineNoIdx);
                        detail.Color = reader.IsDBNull(colorIdx) ? string.Empty : reader.GetString(colorIdx);
                        detail.ConductionNo = reader.IsDBNull(conductionNoIdx) ? string.Empty : reader.GetString(conductionNoIdx);
                        detail.ChassisNo = reader.IsDBNull(chassisNoIdx) ? string.Empty : reader.GetString(chassisNoIdx);
                        detail.PlateNo = reader.IsDBNull(plateNoIdx) ? string.Empty : reader.GetString(plateNoIdx);
                        detail.Accessories = reader.IsDBNull(accessoriesIdx) ? string.Empty : reader.GetString(accessoriesIdx);
                        detail.CarCompanyText = reader.IsDBNull(carCompanyNameIdx) ? string.Empty : reader.GetString(carCompanyNameIdx);
                        detail.CarMakeText = reader.IsDBNull(carMakeNameIdx) ? string.Empty : reader.GetString(carMakeNameIdx);
                        detail.AuthenticationNo = reader.IsDBNull(authenticationNoIdx) ? string.Empty : reader.GetString(authenticationNoIdx);
                        detail.COCNo = reader.IsDBNull(cocNoIdx) ? string.Empty : reader.GetString(cocNoIdx);
                        detail.CarYearText = reader.IsDBNull(yearModelIdx) ? "0" : reader.GetInt32(yearModelIdx).ToString();
                        detail.TypeOfCoverText = reader.IsDBNull(coverNameIdx) ? string.Empty : reader.GetString(coverNameIdx);
                    }
                }
            }
            return detail;
        }


    }
}
