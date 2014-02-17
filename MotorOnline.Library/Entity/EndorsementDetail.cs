using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MotorOnline.Library.Entity
{
    public class EndorsementDetail
    {
        public int ParentTransactionID { get; set; }
        public int NewTransactionID { get; set; }
        public string EndorsementText { get; set; }
        public DateTime DateEndorsed { get; set; }
        public DateTime EffectivityDate { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
