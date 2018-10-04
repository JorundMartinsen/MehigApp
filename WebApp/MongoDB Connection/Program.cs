using System;
using System.Security.Cryptography;

namespace MongoDB_Connection
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }





        //This generates a new salt for salting passwords
        private static readonly RNGCryptoServiceProvider random = new RNGCryptoServiceProvider();
        private static int saltLengthLimit = 32;

        private static byte[] GetSalt()
        {
            return GetSalt(saltLengthLimit);
        }
        private static byte[] GetSalt(int maximumSaltLength)
        {
            var salt = new byte[maximumSaltLength];
            using (random)
            {
                random.GetNonZeroBytes(salt);
            }

            return salt;
        }
    }
}
