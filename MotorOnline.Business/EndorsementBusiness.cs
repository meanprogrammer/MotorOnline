using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MotorOnline.Library.Entity;
using MotorOnline.Data;

namespace MotorOnline.Business
{
    public class EndorsementBusiness
    {
        public Endorsement GetOneEndorsement(int endorsementCode)
        {
            return DataFacade.Data.EndorsementData.GetOneEndorsement(endorsementCode);
        }

        public EndorsementDetail GetEndorsementDetail(int id)
        {
            return DataFacade.Data.EndorsementData.GetEndorsementDetail(id);
        }

        public bool SaveEndorsementDetails(int transactionId, int newTransactionId,
                    string endorsementText, DateTime dateEndorsed,
                    DateTime effectivityDate, DateTime expiryDate,
                    int endorsementType)
        {
            return DataFacade.Data.EndorsementData.SaveEndorsementDetails(transactionId,
                newTransactionId, endorsementText, dateEndorsed, effectivityDate,
                expiryDate, endorsementType);
        }

        public List<Endorsement> GetAllEndorsement()
        {
            return DataFacade.Data.EndorsementData.GetAllEndorsement();
        }

        public Dictionary<string, EndorsementHistory> GetEndorsementHistory(int transactionId)
        {
            return DataFacade.Data.EndorsementData.GetEndorsementHistory(transactionId);
        }

        public int SaveTransactionWithUpdatedCOCNo(int transactionId, string newPolicyNo,
                string newCOCNo, out int newId)
        {
            return DataFacade.Data.EndorsementData.SaveTransactionWithUpdatedCOCNo(transactionId,
                newPolicyNo, newCOCNo, out newId);
        }

        public int SaveTransactionWithUpdatedInsuredName(int transactionId,
                           string newPolicyNo, int customerId, out int newId,
                           string newLastName, string newFirstName, string newMI)
        {
            return DataFacade.Data.EndorsementData.SaveTransactionWithUpdatedInsuredName(
                transactionId, newPolicyNo, customerId, out newId, newLastName, newFirstName,
                newMI);
        }

        public int SaveTransactionWithUpdatePolicyDate(int transactionId,
         string newPolicyNo, out int newId, DateTime from, DateTime to)
        {
            return DataFacade.Data.EndorsementData.SaveTransactionWithUpdatePolicyDate(
                transactionId, newPolicyNo, out newId, from, to);
        }

        public int SaveTransactionWithUpdatedVehicleDescription(int transactionId,
             string newPolicyNo, out int newId, int carcompany, string carmake,
             int carseries, string engineSeries)
        {
            return DataFacade.Data.EndorsementData.SaveTransactionWithUpdatedVehicleDescription(
                transactionId, newPolicyNo, out newId, carcompany, carmake,
                carseries, engineSeries);
        }

        public int SaveTransactionWithUpdatedMortgagee(int transactionId,
                string newPolicyNo, out int newId,
                string mortgagee)
        { 
            return DataFacade.Data.EndorsementData.SaveTransactionWithUpdatedMortgagee(
                transactionId, newPolicyNo, out newId, mortgagee);
        }

        public int SaveTransactionWithNewOwner(int transactionId, string newPolicyNo, out int newId,
            int typeofinsurance, string designation, string lastname, string firstname, string mi, string multicorpname)
        { 
            return DataFacade.Data.EndorsementData.SaveTransactionWithNewOwner(
                transactionId, newPolicyNo, out newId, typeofinsurance, 
                designation, lastname, firstname, mi, multicorpname);
        }

        public int SaveTransactionWithUpdatedAddress(int transactionId,
            string newPolicyNo, int customerId, out int newId,
            string newAddress)
        {
            return DataFacade.Data.EndorsementData.SaveTransactionWithUpdatedAddress(
                transactionId, newPolicyNo, customerId, out newId, newAddress);
        }

        public int SaveTransactionWithDeleteMortgagee(int transactionId,
            string newPolicyNo, out int newId)
        {
            return DataFacade.Data.EndorsementData.SaveTransactionWithDeleteMortgagee(transactionId,
                            newPolicyNo, out newId);
        }
    }
}
