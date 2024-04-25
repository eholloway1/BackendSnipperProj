/*
 * Taken from MultiverseLearning Products cSharp crypto-cupcakes repo files
*/

using System.Security.Cryptography;
using System.Text;

namespace Snipper.server.Utilities
{
    public class EncryptUtility
    {
        public readonly byte[] Key;
        public readonly byte[] IV;

        public EncryptUtility(IConfiguration configuration)
        {
            //Read secret keys through dependency injeciton - if none exist, set to default strings
            Key = Encoding.UTF8.GetBytes(configuration["AES_KEY"] ?? "07fda64e90d3779d2e170c79b957725c");
            IV = Encoding.UTF8.GetBytes(configuration["AES_IV"] ?? "40959bcce5a3f765");

        }

        public string Encrypt(string plaintext)
        {
            byte[] encrypted;

            //creat and AesCryptoServiceProvider object with the specified key and IV
            using (Aes aes = Aes.Create())
            {
                aes.Key = Key;
                aes.IV = IV;

                //Create an encryptor to perform the stream transform
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                //create the streams used for encryption
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                        {
                            //write all data to the stream
                            sw.Write(plaintext);
                        }
                        encrypted = ms.ToArray();
                    }
                }
            }
            return Convert.ToBase64String(encrypted);
        }

        public string Decrypt(string input)
        {
            //declare strng to hold decryptedText text
            string decryptedText = "";

            //create an AesCryptoServiceProvider object with the specified key and IV
            using (Aes aes = Aes.Create())
            {
                aes.Key = Key;
                aes.IV = IV;

                //creat a decryptor to perform the stream transform
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                //create the streams used for decryption
                using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(input)))
                {
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader sr = new StreamReader(cs))
                        {
                            //read the decrypted bytes from the decrypting stream and place them in a string
                            decryptedText = sr.ReadToEnd();
                        }
                    }
                }
            }
            return decryptedText;
        }
    }
}
