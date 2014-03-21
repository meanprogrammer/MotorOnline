using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MotorOnline.Library.Entity;
using MotorOnline.Data;

namespace MotorOnline.Business
{
    public class TransactionBusiness
    {
        public bool SaveTransaction(Transaction transaction, out int transactionId)
        {
            return DataFacade.Data.TransactionData.SaveTransaction(transaction, out transactionId);
        }

        public int PostTransaction(int transactionId)
        {
            return DataFacade.Data.TransactionData.PostTransaction(transactionId);
        }

        public Transaction GetTransactionById(int transactionId)
        {
            return DataFacade.Data.TransactionData.GetTransactionById(transactionId);
        }


        public bool UpdateTransaction(Transaction transaction)
        {
            return DataFacade.Data.TransactionData.UpdateTransaction(transaction);
        }

        public Dictionary<string, List<DropDownListItem>> LoadAllSearchFilters()
        {
            return DataFacade.Data.TransactionData.LoadAllSearchFilters();
        }

        public IEnumerable<TransactionSearchDTO> SearchTransaction(string whereClause)
        {
            return DataFacade.Data.TransactionData.SearchTransaction(whereClause);
        }
    }
}
