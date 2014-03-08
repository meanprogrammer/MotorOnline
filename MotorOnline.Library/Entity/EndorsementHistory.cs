using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MotorOnline.Library.Entity
{
    public class EndorsementHistory
    {
        public EndorsementHistory() 
        {
            this.Endorsement = new EndorsementDetail();
        }

        public EndorsementDetail Endorsement { get; set; }
        public string EndorsementTitle { get; set; }
    }
}
