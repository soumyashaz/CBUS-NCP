using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Configuration;

namespace CBUSA.Areas.Admin.Models
{
    public static class Encryption
    {
        static public string EncodeTo64(string toEncode)
        {
            byte[] toEncodeAsBytes
                  = System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);
            string returnValue
                  = System.Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }

        static public string DecodeFrom64(string encodedData)
        {
            byte[] encodedDataAsBytes
                = System.Convert.FromBase64String(encodedData);
            string returnValue =
               System.Text.ASCIIEncoding.ASCII.GetString(encodedDataAsBytes);
            return returnValue;
        }
    }





    //public static class Encrypt
    //{
    //    // This size of the IV (in bytes) must = (keysize / 8).  Default keysize is 256, so the IV must be
    //    // 32 bytes long.  Using a 16 character string here gives us 32 bytes when converted to a byte array.
    //    private const string initVector = "pemgail9uzpgzl88";
    //    // This constant is used to determine the keysize of the encryption algorithm
    //    private const int keysize = 256;
    //    //Encrypt

    //    private const string passPhrase = WebConfigurationManager.AppSettings["PasswordPhase"];


    //    public static string EncryptString(string plainText)
    //    {
    //        byte[] initVectorBytes = Encoding.UTF8.GetBytes(initVector);
    //        byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
    //        PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
    //        byte[] keyBytes = password.GetBytes(keysize / 8);
    //        RijndaelManaged symmetricKey = new RijndaelManaged();
    //        symmetricKey.Mode = CipherMode.CBC;
    //        ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
    //        MemoryStream memoryStream = new MemoryStream();
    //        CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
    //        cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
    //        cryptoStream.FlushFinalBlock();
    //        byte[] cipherTextBytes = memoryStream.ToArray();
    //        memoryStream.Close();
    //        cryptoStream.Close();
    //        return Convert.ToBase64String(cipherTextBytes);
    //    }
    //    //Decrypt
    //    public static string DecryptString(string cipherText)
    //    {
    //        byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
    //        byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
    //        PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
    //        byte[] keyBytes = password.GetBytes(keysize / 8);
    //        RijndaelManaged symmetricKey = new RijndaelManaged();
    //        symmetricKey.Mode = CipherMode.CBC;
    //        ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
    //        MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
    //        CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
    //        byte[] plainTextBytes = new byte[cipherTextBytes.Length];
    //        int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
    //        memoryStream.Close();
    //        cryptoStream.Close();
    //        return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
    //    }
    //}


}