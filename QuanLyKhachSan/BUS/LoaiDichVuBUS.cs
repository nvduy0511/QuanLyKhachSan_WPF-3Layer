using DAL;
using DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class LoaiDichVuBUS
    {
        private static LoaiDichVuBUS instance;

        public static LoaiDichVuBUS Instance
        {
            get
            {
                if (instance == null) instance = new LoaiDichVuBUS();
                return instance;
            }
        }

        private LoaiDichVuBUS() { }
        public List<LoaiDV> getDataLoaiDV()
        {
            return LoaiDichVuDAL.Instance.getData();
        }

        public bool addLoaiDV(LoaiDV loaiDV)
        {
            return LoaiDichVuDAL.Instance.addDataLoaiDV(loaiDV);
        }

        public bool xoaLoaiDV(LoaiDV loaiDV)
        {
            return LoaiDichVuDAL.Instance.xoaLoaiDV(loaiDV);
        }

        public bool capNhatDataLoaiDV(LoaiDV loaiDV)
        {
            return LoaiDichVuDAL.Instance.capnhatLoaiDV(loaiDV);
        }
        public bool KiemTraTenLoaiDV(LoaiDV loaiDV)
        {
            return LoaiDichVuDAL.Instance.KiemTraTenLoaiDichVu(loaiDV);
        }
    }
}
