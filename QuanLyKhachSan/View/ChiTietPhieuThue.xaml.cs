using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DAL.DTO;
using DAL;
using BUS;
namespace GUI.View
{
    /// <summary>
    /// Interaction logic for ChiTietPhieuThue.xaml
    /// </summary>
    public partial class ChiTietPhieuThue : Window
    {
        List<CT_PhieuThue> lsCTPT;
        public ChiTietPhieuThue()
        {
            InitializeComponent();
        }
        public ChiTietPhieuThue(PhieuThue_Custom ptct):this()
        {
            txblTenKH.Text = ptct.TenKH;
            txblNgayLapHD.Text = ptct.NgayLapPhieu.ToString();
            txblTenNV.Text = ptct.TenNV;
            txblTieuDe.Text += ptct.MaPhieuThue.ToString();
            lsCTPT = new List<CT_PhieuThue>();
            lsCTPT = CT_PhieuThueBUS.GetInstance().getCTPhieuThueTheoMaPT(ptct.MaPhieuThue);
            lvCTPT.ItemsSource = lsCTPT;
        }

        private void click_Thoat(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
