using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;



        public class EncryptionHelper
        {
            private byte[] keyAndIvBytes;

             public EncryptionHelper(string key)
            {
                // You'll need a more secure way of storing this, I this isn't
                // a real key
                keyAndIvBytes = new byte[UTF8Encoding.UTF8.GetBytes(key).Length];
                keyAndIvBytes = UTF8Encoding.UTF8.GetBytes(key);
            }

            public  string ByteArrayToHexString(byte[] ba)
            {

                return BitConverter.ToString(ba).Replace("-", "");
            }

            public  byte[] StringToByteArray(string hex)
            {
                return Enumerable.Range(0, hex.Length)
                                 .Where(x => x % 2 == 0)
                                 .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                                 .ToArray();
            }

            public  string DecodeAndDecrypt(string cipherText)
            {
                string DecodeAndDecrypt = AesDecrypt(StringToByteArray(cipherText));
                return (DecodeAndDecrypt);
            }

            public  string EncryptAndEncode(string plaintext)
            {
                return ByteArrayToHexString(AesEncrypt(plaintext));
            }

            public  string AesDecrypt(Byte[] inputBytes)
            {
                Byte[] outputBytes = inputBytes;

                string plaintext = string.Empty;

                using (MemoryStream memoryStream = new MemoryStream(outputBytes))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, GetCryptoAlgorithm().CreateDecryptor(keyAndIvBytes, keyAndIvBytes), CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(cryptoStream))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

                return plaintext;
            }

            public  byte[] AesEncrypt(string inputText)
            {
                byte[] inputBytes = UTF8Encoding.UTF8.GetBytes(inputText);//AbHLlc5uLone0D1q

                byte[] result = null;
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, GetCryptoAlgorithm().CreateEncryptor(keyAndIvBytes, keyAndIvBytes), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(inputBytes, 0, inputBytes.Length);
                        cryptoStream.FlushFinalBlock();

                        result = memoryStream.ToArray();
                    }
                }

                return result;
            }


            private  RijndaelManaged GetCryptoAlgorithm()
            {
                RijndaelManaged algorithm = new RijndaelManaged();
                //set the mode, padding and block size
                algorithm.Padding = PaddingMode.PKCS7;
                algorithm.Mode = CipherMode.CBC;
                algorithm.KeySize = 128;
                algorithm.BlockSize = 128;
                return algorithm;
            }
        }
   
