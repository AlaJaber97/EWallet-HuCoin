using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace HuCoin
{
    public static class AppStatic
    {
        public static string Token {private get; set; }
        public static BLL.Models.User User => BLL.Services.JWT.GetUser(Token);
        public static BLL.Models.Wallet Wallet { get; set; }
        public static readonly string HuCoinPinCodeKey = "HuCoinPinCode";
        internal static readonly string LastUserLoginKey = "LastUserLogin";

        public static AuthenticationHeaderValue GetAuthenticationHeader()
        {
            return new AuthenticationHeaderValue("Bearer", Token);
        }
    }
}
