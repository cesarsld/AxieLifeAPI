using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Text;
using Nethereum.Signer;

namespace AxieTournamentApi.Models.Cryptography
{
    public class CryptographyModule
    {
        private static readonly int saltLengthLimit = 32;


        public static bool GetSignedMessage(string sentSignature, string sentAddress)
        {
            var msg = "Axie Tournament";
            var msgHash = Encoding.UTF8.GetBytes(msg);
            var signer = new EthereumMessageSigner();
            //var signature = signer.HashAndSign(msg, msg);
            var address = signer.EcRecover(msgHash, sentSignature);
            if (address.ToLower() == sentAddress.ToLower())
                return true;
            else
                return false;
        }

        public static byte[] GenerateSalt()
        {
            var salt = new byte[saltLengthLimit];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetNonZeroBytes(salt);
            }
            return salt;
        }

        public static byte[] GenerateHash(string password, byte[] salt) => GenerateHash(Encoding.UTF8.GetBytes(password), salt);

        public static byte[] GenerateHash(byte[] password, byte[] salt)
        {
            var sha256 =  SHA256.Create();
            var saltedPw = password.Concat(salt).ToArray();

            return sha256.ComputeHash(saltedPw);
        }

        public static bool ConfirmPassword(string password, byte[] salt, byte[] validHash)
        {
            var computedHash = GenerateHash(password, salt);

            return validHash.SequenceEqual(computedHash);
        }

    }
}
