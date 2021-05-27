using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace HuCoin.Converter
{
    public class GetMainUserTransactionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is BLL.Models.TransactionClient transaction)
            {
                switch (transaction.TransactionType)
                {
                    case BLL.Enums.TransactionType.Cashin:
                        return "Deposit";
                    case BLL.Enums.TransactionType.Cashout:
                        return "Withdrawal ";
                    case BLL.Enums.TransactionType.Transfer:
                        BLL.Models.User user = transaction.Sender;
                        if (transaction.Sender.PhoneNumber == AppStatic.User.PhoneNumber) user = transaction.Recipient;

                        return $"{user.FirstName} {user.FamilyName} ({user.PhoneNumber})";
                }
            }
            return "Unkown User";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
