using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NBitcoin;

namespace API.Utils
{
    public static class RSA
    {
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
        public static string SignatureGenerate(string PrivateKey, string Message)
        {
            var secret = Network.Main.CreateBitcoinSecret(PrivateKey);
            var signature = secret.PrivateKey.SignMessage(Message);
            return signature;
        }
        public static bool Verify(string pubKey, string originalMessage, string signedMessage)
        {
            var address = BitcoinAddress.Create(pubKey, Network.Main);
            var privateKey = address as IPubkeyHashUsable;
            var isVerified = privateKey.VerifyMessage(originalMessage, signedMessage);
            return isVerified;
        }
    }
}
