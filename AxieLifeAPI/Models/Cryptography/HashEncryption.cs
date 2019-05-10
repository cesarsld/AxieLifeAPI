using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Text;

namespace AxieTournamentApi.Models.Cryptography
{
    public class HashEncryption
    {
        private static readonly int saltLengthLimit = 32;

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
