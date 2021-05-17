using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NBitcoin;

namespace BLL.Utils
{
    public static class RSA
    {
        public static string SignatureGenerate(string PrivateKey, string Message)
        {
            var secret = Network.Main.CreateBitcoinSecret(PrivateKey);
            var signature = secret.PrivateKey.SignMessage(Message);
            return signature;
        }
    }
}
