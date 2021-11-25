using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class MD5_HashBUS
    {
        private static MD5_HashBUS Instance;

        private MD5_HashBUS()
        {

        }


        public static MD5_HashBUS GetInstance()
        {
            if (Instance == null)
            {
                Instance = new MD5_HashBUS();
            }
            return Instance;
        }
        public string HashMatKhauThanhMD5(string pass)
        {
            byte[] temp = ASCIIEncoding.ASCII.GetBytes(pass);
            byte[] hasData = new MD5CryptoServiceProvider().ComputeHash(temp);

            StringBuilder sb = new StringBuilder();

            foreach (var item in hasData)
            {
                sb.Append(item.ToString("x2"));
            }
            return sb.ToString();
        }
    }
}
