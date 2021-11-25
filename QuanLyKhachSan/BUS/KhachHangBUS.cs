using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DAL.Data;

namespace BUS
{
    public class KhachHangBUS
    {
        private static KhachHangBUS Instance;

        private KhachHangBUS()
        {

        }

        public static KhachHangBUS GetInstance()
        {
            if (Instance == null)
            {
                Instance = new KhachHangBUS();
            }
            return Instance;
        }

        public List<KhachHang> GetKhachHangs()
        {
            return KhachHangDAL.GetInstance().getData();
        }

        public bool addKhachHang(KhachHang kh, out string error)
        {
            return KhachHangDAL.GetInstance().addKhachHang(kh,out error);
        }

        //kiểm tra khách hàng đã tồn tại chưa dựa vào CCCD nếu rồi thì trả ra KhachHang nếu chưa thì trả ra null
        public int kiemTraTonTaiKhachHang(string cccd)
        {
            KhachHang kh = KhachHangDAL.GetInstance().kiemTraTonTaiKhachHang(cccd);
            if(kh != null)
            {
                return kh.MaKH;
            }
            else
            {
                return -1;
            }
        }

        public bool capNhatDataKhachHang(KhachHang khachHang)
        {
            return KhachHangDAL.GetInstance().capnhatKhachHang(khachHang);
        }

        public bool xoaDataKhachHang(KhachHang khachHang)
        {
            return KhachHangDAL.GetInstance().xoaKhachHang(khachHang);
        }

        public string layTenKhachHangTheoMaPT(int? maPhieuThue)
        {
            return KhachHangDAL.GetInstance().layTenKhachHangTheoMaPT(maPhieuThue);
        }
    }
}
