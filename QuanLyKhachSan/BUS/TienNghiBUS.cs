using DAL;
using DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class TienNghiBUS
    {
        private static TienNghiBUS instance;

        public static TienNghiBUS Instance
        {
            get
            {
                if (instance == null) instance = new TienNghiBUS();
                return instance;
            }
        }

        private TienNghiBUS() { }
        public List<TienNghi> getDataTienNghi()
        {
            return TienNghiDAL.Instance.getData();
        }
        public bool addTienNghi(TienNghi tn)
        {
            return TienNghiDAL.Instance.addTienNghi(tn);
        }

        public bool xoaTienNghi(TienNghi tn)
        {
            return TienNghiDAL.Instance.xoaTienNghi(tn);
        }

        public bool capNhatTienNghi(TienNghi tn)
        {
            return TienNghiDAL.Instance.capnhatTienNghi(tn);
        }
        public bool KiemTraTenTienNghi(TienNghi tn)
        {
            return TienNghiDAL.Instance.KiemTraTenTienNghi(tn);
        }
    }
}
