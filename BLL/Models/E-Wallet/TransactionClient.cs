using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Models
{
    public class TransactionClient
    {
        public Guid ID { get; set; }
        public BLL.Models.User Recipient { get; set; }
        public BLL.Models.User Sender { get; set; }
        public BLL.Enums.TransactionType TransactionType
        {
            get
            {
                if (string.IsNullOrEmpty(Sender?.PhoneNumber)) return BLL.Enums.TransactionType.Cashin;
                if (string.IsNullOrEmpty(Recipient?.PhoneNumber)) return BLL.Enums.TransactionType.Cashout;
                return BLL.Enums.TransactionType.Transfer;
            }
        }
        public (string ImageSource, string TextColor) UiTransaction
        {
            get
            {
                return TransactionType switch
                {
                    Enums.TransactionType.Cashin => ("arrow_up.png", "#00DD80"),
                    Enums.TransactionType.Cashout => ("arrow_down.png", "#FF7C48"),
                    Enums.TransactionType.Transfer => ("arrow_transfer.png", "#000000"),
                    _ => default,
                };
            }
        }
        public decimal Amount { get; set; }
        public decimal Fees { get; set; }
        public DateTime Date { get; set; }
    }
}
