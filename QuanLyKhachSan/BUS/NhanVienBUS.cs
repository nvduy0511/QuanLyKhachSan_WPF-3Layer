using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using DAL.Data;
using DAL;

namespace BUS
{
    public class NhanVienBUS
    {
        private static NhanVienBUS Instance;

        private NhanVienBUS()
        {

        }

        public static NhanVienBUS GetInstance()
        {
            if (Instance == null)
            {
                Instance = new NhanVienBUS();
            }
            return Instance;
        }

        //Lấy danh sách nhân viên từ DB
        public List<NhanVien> getDataNhanVien()
        {
            return NhanVienDAL.GetInstance().getDataNhanVien();
        }

        //Thêm nhân viên vào DB
        public bool addNhanVien(NhanVien nv)
        {
            return  NhanVienDAL.GetInstance().addDataNhanVien(nv);
        }

        //Sửa nhân viên 
        public bool updateNhanVien(NhanVien nv)
        {
            return NhanVienDAL.GetInstance().updateDataNhanVien(nv);
        }
        //Xóa nhân viên
        public bool deleteNhanVien(NhanVien nv)
        {
            return NhanVienDAL.GetInstance().deleteDataNhanVien(nv);
        }
        //lấy nhân viên theo mã  nhân viên
        public string layNhanVienTheoMaNV(int maNV)
        {
            NhanVien nv = NhanVienDAL.GetInstance().layNhanVienTheoMaNV(maNV);
            if (nv != null)
                return nv.HoTen;
            else
                return "Uknow";
        }
    }
}
