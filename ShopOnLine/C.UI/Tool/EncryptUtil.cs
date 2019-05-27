using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace C.UI.Tool
{
   public static class EncryptUtil
    {
        // 1 
        /// <summary>
        /// Mã hóa chuỗi theo dạng MD5 ( chuỗi đầu vào: 123456 ->Chuỗi trả về e1-0a-dc-39-49-ba-59-ab-be-56-e0-57-f2-0f-88-3e)
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string EncryptMD5(string key)
        {
            UTF8Encoding Unic = new UTF8Encoding();
            byte[] bytes = Unic.GetBytes(key);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] ketqua = md5.ComputeHash(bytes);

            return BitConverter.ToString(ketqua);
        }


        // 2
        public static string EncryptSHA(string Password)
        {
            System.Text.UnicodeEncoding encoding = new System.Text.UnicodeEncoding();
            byte[] hashBytes = encoding.GetBytes(Password);

            //Compute the SHA-1 hash
            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();

            byte[] cryptPassword = sha1.ComputeHash(hashBytes);

            return BitConverter.ToString(cryptPassword);
        }

        // 3
        /// <summary>
        /// Mã hóa chuỗi theo dạng MD5 ( chuỗi đầu vào: 123456 ->Chuỗi trả về E10ADC3949BA59ABBE56E057F20F883E)
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static string EncryptMD5_(string txt)
        {
            String str = "";
            Byte[] buffer = System.Text.Encoding.UTF8.GetBytes(txt);
            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            buffer = md5.ComputeHash(buffer);
            foreach (Byte b in buffer)
            {
                str += b.ToString("X2");
            }
            return str;
        }

        // 4
        /// <summary>
        /// Mã hóa chuỗi theo dạng MD5 ( chuỗi đầu vào: 123456 ->Chuỗi trả về e10adc3949ba59abbe56e057f20f883e)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string EncryptData(string data)
        {
            return BitConverter.ToString(encrypt(data)).Replace("-", "").ToLower();
        }

        public static byte[] encrypt(string data)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider md5Hasher = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] hashedBytes;
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            hashedBytes = md5Hasher.ComputeHash(encoder.GetBytes(data));
            return hashedBytes;
        }
    }
}

