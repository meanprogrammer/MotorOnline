
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MotorOnline
{
    public class TransactionComputation
    {
        public TransactionComputation() {
            this.NetComputationDetails = new ComputationDetails();
            this.GrossComputationDetails = new ComputationDetails();
        }

        public int TransactionID { get; set; }
        public ComputationDetails NetComputationDetails { get; set; }
        public ComputationDetails GrossComputationDetails { get; set; }
    }
}