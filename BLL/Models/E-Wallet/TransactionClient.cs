using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Models
{
    public class TransactionClient
    {
        public string RecipientPhoneNumber { get; set; }
        public string SenderPhoneNumber { get; set; }

        public decimal Amount { get; set; }
        public decimal Fees { get; set; }
    }
}
