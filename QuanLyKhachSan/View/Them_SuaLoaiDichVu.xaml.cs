using DAL;
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

namespace GUI.View
{
    /// <summary>
    /// Interaction logic for Them_SuaLoaiDichVu.xaml
    /// </summary>
    public partial class Them_SuaLoaiDichVu : Window
    {
        public delegate void truyenData(LoaiDV loaiDV);
        public delegate void suaData(LoaiDV loaiDV);


        public truyenData truyenLoaiDV;
        public suaData suaLoaiDV;

        private string maLoai;
        public Them_SuaLoaiDichVu()
        {
            InitializeComponent();
        }

        public Them_SuaLoaiDichVu(LoaiDV loaiDV) : this()
        {
            txtTenLoaiDV.Text = loaiDV.TenLoaiDV;
            txbTitle.Text = "Sửa thông tin" + loaiDV.MaLoaiDV;

            maLoai = loaiDV.MaLoaiDV.ToString();
        }

        private void btnHuy_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnThem_Click(object sender, RoutedEventArgs e)
        {

            if (!KiemTra())
            {
                return;
            }
            else
            {
                LoaiDV loaiDV = new LoaiDV()
                {
                    TenLoaiDV = txtTenLoaiDV.Text,


                };
                if (truyenLoaiDV != null)
                {
                    truyenLoaiDV(loaiDV);
                }
            }
            Window wd = Window.GetWindow(sender as Button);
            wd.Close();
        }

        private void btnCapNhat_Click(object sender, RoutedEventArgs e)
        {

            if (!KiemTra())
            {
                return;
            }
            else
            {
                LoaiDV loaiDV = new LoaiDV()
                {
                    MaLoaiDV = int.Parse(maLoai.ToString()),
                    TenLoaiDV = txtTenLoaiDV.Text,
                };
                if (suaLoaiDV != null)
                {
                    suaLoaiDV(loaiDV);
                }
            }
            Window wd = Window.GetWindow(sender as Button);
            wd.Close();
        }

        private bool KiemTra()
        {
            if (string.IsNullOrWhiteSpace(txtTenLoaiDV.Text))
            {
                new DialogCustoms("Vui lòng nhập tên loại dịch vụ","Thông báo",DialogCustoms.OK).Show();
                return false;
            }
            else
            {
                int so;
                if (int.TryParse(txtTenLoaiDV.Text, out so) == true)
                {
                    new DialogCustoms("Vui lòng nhập đúng định dạng tên loại dịch vụ", "Thông báo", DialogCustoms.OK).Show();
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
    }
}
