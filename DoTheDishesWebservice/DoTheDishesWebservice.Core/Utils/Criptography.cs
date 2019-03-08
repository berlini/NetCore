using System;
using System.Security.Cryptography;
using System.Text;

namespace DoTheDishesWebservice.Core.Utils
{
    public static class Criptography
    {
        public static string GetSHA1(string strPlain)
        {
            UnicodeEncoding UE = new UnicodeEncoding();

            byte[] HashValue, MessageBytes = UE.GetBytes(strPlain);

            SHA1Managed SHhash = new SHA1Managed();

            string strHex = "";

            HashValue = SHhash.ComputeHash(MessageBytes);

            foreach (byte b in HashValue)
            {
                strHex += string.Format("{0:x2}", b);
            }

            return strHex;
        }
    }
}
