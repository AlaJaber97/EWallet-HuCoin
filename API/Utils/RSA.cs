using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NBitcoin;

namespace API.Utils
{
    public static class RSA
    {
        public static string SignatureGenerate(string PrivateKey, string Message)
        {
            var secret = Network.Main.CreateBitcoinSecret(PrivateKey);
            var signature = secret.PrivateKey.SignMessage(Message);
            return signature;
        }
        public static BLL.Models.Credential KeyGenerate()
        {
            var privateKey = new Key();
            var bitcoinSecret = privateKey.GetBitcoinSecret(Network.Main);
            var publicKey = bitcoinSecret.GetAddress(ScriptPubKeyType.Legacy);

            return new BLL.Models.Credential
            {
                PrivateKey = bitcoinSecret.ToString(),
                PublicKey = publicKey.ToString()
            };
        }
    }
}
