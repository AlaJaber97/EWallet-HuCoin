using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Models
{
    public class TransactionMiner
    {
        public string Recipient { get; set; }
        public string Sender { get; set; }
        public string Signature { get; set; }

        public decimal Amount { get; set; }
        public decimal Fees { get; set; }

        public override string ToString() =>
            $"{Amount:0.00000000}{Recipient}{Sender}";
    }
}
