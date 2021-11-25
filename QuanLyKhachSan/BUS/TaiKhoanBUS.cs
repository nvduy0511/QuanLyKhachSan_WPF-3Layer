using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DAL.Data;

namespace BUS
{
    public class TaiKhoanBUS
    {
        private static TaiKhoanBUS Instance;

        private TaiKhoanBUS()
        {

        }

        public static TaiKhoanBUS GetInstance()
        {
            if (Instance == null)
            {
                Instance = new TaiKhoanBUS();
            }
            return Instance;
        }
        public TaiKhoan kiemTraTKTonTaiKhong(string username, string pass)
        {
            string matKhau = MD5_HashBUS.GetInstance().HashMatKhauThanhMD5(pass);

            return TaiKhoanDAL.GetInstance().layTaiKhoanTuDataBase(username, matKhau);
        }
        public bool capNhatAvatar(string username,string avatar, out string error)
        {
            return TaiKhoanDAL.GetInstance().capNhatAvatar( username , avatar, out error);
        }
    }
}
