using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace HuCoin
{
    public static class AppStatic
    {
        public static string Token {private get; set; }
        public static BLL.Models.Wallet Wallet { get; set; }
        public static readonly string HuCoinPinCodeKey = "HuCoinPinCode";

        public static AuthenticationHeaderValue GetAuthenticationHeader()
        {
            return new AuthenticationHeaderValue("Bearer", Token);
        }
    }
}
