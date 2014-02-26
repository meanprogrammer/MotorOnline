using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MotorOnline.Library.Entity
{
    public class TransactionSearchResponseDTO
    {
        public IEnumerable<TransactionSearchDTO> Data { get; set; }
        public int PageCount { get; set; }
        public int CurrentPage { get; set; }
    }
}
