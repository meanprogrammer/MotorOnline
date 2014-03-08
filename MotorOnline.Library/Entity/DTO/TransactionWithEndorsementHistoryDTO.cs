using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MotorOnline.Library.Entity
{
    public class TransactionWithEndorsementHistoryDTO
    {
        public Transaction Transaction { get; set; }
        public Dictionary<string, EndorsementHistory> History { get; set; }
    }
}
