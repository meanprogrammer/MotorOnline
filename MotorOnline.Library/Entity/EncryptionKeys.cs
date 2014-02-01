using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MotorOnline.Library.Entity
{
    public class EncryptionKeys
    {
        public byte[] IV { get; set; }
        public byte[] Key { get; set; }
    }
}
