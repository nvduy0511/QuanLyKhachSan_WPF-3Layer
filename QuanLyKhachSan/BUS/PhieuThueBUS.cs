using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DAL.Data;
using DAL.DTO;

namespace BUS
{
    public class PhieuThueBUS
    {
        private static PhieuThueBUS Instance;

        private PhieuThueBUS()
        {

        }

        public static PhieuThueBUS GetInstance()
        {
            if (Instance == null)
            {
                Instance = new PhieuThueBUS();
            }
            return Instance;
        }
        public bool addPhieuThue(PhieuThue ctpt, out string error)
        {
            return PhieuThueDAL.GetInstance().addPhieuThue(ctpt, out error);
        }

        public List<PhieuThue_Custom> getDataPhieuThue()
        {
            return PhieuThueDAL.GetInstance().getDataFromDB();
        }

        public  bool xoaPhieuThueTheoMaPhieuThue(int maPhieuThue, string error)
        {
            return PhieuThueDAL.GetInstance().xoaPhieuThueTheoMaPhieuThue(maPhieuThue, error);
        }
    }
}
