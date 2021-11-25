using DAL;
using DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class LoaiPhongBUS
    {
        private static LoaiPhongBUS instance;

        public static LoaiPhongBUS Instance
        {
            get
            {
                if (instance == null) instance = new LoaiPhongBUS();
                return instance;
            }
        }

        private LoaiPhongBUS() { }
        public List<LoaiPhong> getDataLoaiPhong()
        {
            return LoaiPhongDAL.Instance.getDataLoaiPhong();
        }

        public bool addLoaiPhong(LoaiPhong loaiPhong)
        {
            return LoaiPhongDAL.Instance.addLoaiPhong(loaiPhong);
        }

        public bool xoaLoaiPhong(LoaiPhong loaiPhong)
        {
            return LoaiPhongDAL.Instance.xoaLoaiPhong(loaiPhong);
        }

        public bool capNhatDataLoaiPhong(LoaiPhong loaiPhong)
        {
            return LoaiPhongDAL.Instance.capnhatLoaiPhong(loaiPhong);
        }

        public bool KiemTraTrungTen(LoaiPhong loaiPhong)
        {
            return LoaiPhongDAL.Instance.KiemTraTenLoaiPhong(loaiPhong);
        }
    }
}
