namespace AvisosAPI.Helpers
{
    using System.Security.Cryptography;
    using System.Text;


    public static class EncriptacionHelper

    {

        #region Configuración

        // Configuración por defecto para cifrado simétrico

        private static readonly int KeySize = 32; // 256 bits

        private static readonly int IVSize = 16;  // 128 bits

        // Claves por defecto (deberías cambiarlas en producción)

        private static readonly string DefaultKey = "4a27aa972bf31c5132dbcf2d7f5e0250f7086ca34a27aa972bf31c51";

        private static readonly string DefaultIV = "8b5ef91df0fe09606482f383ace81f78";

        private static readonly string Salt = "avisos2026";

        #endregion

        #region Cifrado Simétrico (AES)

        public static string Encrypt(string plainText, string key = null, string iv = null)

        {

            if (string.IsNullOrEmpty(plainText))

                throw new ArgumentException("El texto no puede estar vacío");

            byte[] keyBytes = GetValidKey(key ?? DefaultKey);

            byte[] ivBytes = GetValidIV(iv ?? DefaultIV);

            using (Aes aesAlg = Aes.Create())

            {

                aesAlg.Key = keyBytes;

                aesAlg.IV = ivBytes;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())

                {

                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))

                    {

                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))

                        {

                            swEncrypt.Write(plainText);

                        }

                    }

                    return Convert.ToBase64String(msEncrypt.ToArray());

                }

            }

        }

        public static string Decrypt(string cipherText, string key = null, string iv = null)

        {

            if (string.IsNullOrEmpty(cipherText))

                throw new ArgumentException("El texto cifrado no puede estar vacío");

            byte[] keyBytes = GetValidKey(key ?? DefaultKey);

            byte[] ivBytes = GetValidIV(iv ?? DefaultIV);

            byte[] cipherBytes = Convert.FromBase64String(cipherText);

            using (Aes aesAlg = Aes.Create())

            {

                aesAlg.Key = keyBytes;

                aesAlg.IV = ivBytes;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherBytes))

                {

                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))

                    {

                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))

                        {

                            return srDecrypt.ReadToEnd();

                        }

                    }

                }

            }

        }

        private static byte[] GetValidKey(string key)

        {

            byte[] keyBytes = Encoding.UTF8.GetBytes(key);

            Array.Resize(ref keyBytes, KeySize);

            return keyBytes;

        }

        private static byte[] GetValidIV(string iv)

        {

            byte[] ivBytes = Encoding.UTF8.GetBytes(iv);

            Array.Resize(ref ivBytes, IVSize);

            return ivBytes;

        }

        #endregion

        #region Hashing (SHA512)

        public static string ComputeSHA512Hash(string input)

        {

            if (string.IsNullOrEmpty(input))

                throw new ArgumentException("La entrada no puede estar vacía");

            using (SHA512 sha512 = SHA512.Create())

            {

                byte[] inputBytes = Encoding.UTF8.GetBytes(input);

                byte[] hashBytes = sha512.ComputeHash(inputBytes);

                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

            }

        }

        public static string ComputeSHA512HashWithSalt(string input)

        {

            if (string.IsNullOrEmpty(input))

                throw new ArgumentException("La entrada no puede estar vacía");

            string saltedInput = input + Salt;

            return ComputeSHA512Hash(saltedInput);

        }

        public static bool VerifySHA512Hash(string input, string hash)

        {

            string inputHash = ComputeSHA512Hash(input);

            return inputHash.Equals(hash, StringComparison.OrdinalIgnoreCase);

        }

        public static bool VerifySHA512HashWithSalt(string input, string hash)

        {

            string inputHash = ComputeSHA512HashWithSalt(input);

            return inputHash.Equals(hash, StringComparison.OrdinalIgnoreCase);

        }

        #endregion

        #region Cifrado Asimétrico (RSA)

        public static (string publicKey, string privateKey) GenerateRSAKeys(int keySize = 2048)

        {

            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(keySize))

            {

                string publicKey = rsa.ToXmlString(false);

                string privateKey = rsa.ToXmlString(true);

                return (publicKey, privateKey);

            }

        }

        public static string RSAEncrypt(string plainText, string publicKey)

        {

            if (string.IsNullOrEmpty(plainText))

                throw new ArgumentException("El texto no puede estar vacío");

            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);

            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())

            {

                rsa.FromXmlString(publicKey);

                byte[] encryptedBytes = rsa.Encrypt(plainBytes, true);

                return Convert.ToBase64String(encryptedBytes);

            }

        }

        public static string RSADecrypt(string cipherText, string privateKey)

        {

            if (string.IsNullOrEmpty(cipherText))

                throw new ArgumentException("El texto cifrado no puede estar vacío");

            byte[] cipherBytes = Convert.FromBase64String(cipherText);

            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())

            {

                rsa.FromXmlString(privateKey);

                byte[] decryptedBytes = rsa.Decrypt(cipherBytes, true);

                return Encoding.UTF8.GetString(decryptedBytes);

            }

        }

        #endregion

        #region Métodos Utilitarios

        public static string GenerateRandomString(int length)

        {

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            var random = new Random();

            return new string(Enumerable.Repeat(chars, length)

                .Select(s => s[random.Next(s.Length)]).ToArray());

        }

        public static string GenerateRandomKey(int sizeInBytes)

        {

            byte[] key = new byte[sizeInBytes];

            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())

            {

                rng.GetBytes(key);

            }

            return Convert.ToBase64String(key);

        }

        #endregion

    }
}
