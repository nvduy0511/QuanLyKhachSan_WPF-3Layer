using BUS;
using DAL;
using DAL.DTO;
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
    /// Interaction logic for Them_SuaPhong.xaml
    /// </summary>
    public partial class Them_SuaPhong : Window
    {
        public delegate void TryenDuLieu(PhongDTO p);
        public delegate void SuaDuLieu(PhongDTO p);

        public TryenDuLieu truyen;
        public SuaDuLieu sua;
        List<LoaiPhong> LP;
        private string soPhong;

        public Them_SuaPhong()
        {
            InitializeComponent();
            LP = new List<LoaiPhong>(LoaiPhongBUS.Instance.getDataLoaiPhong());
            cmbLoaiPhong.ItemsSource = LP;
            cmbLoaiPhong.DisplayMemberPath = "TenLoaiPhong";
            cmbLoaiPhong.SelectedValuePath = "MaLoaiPhong";
        }

        public Them_SuaPhong(PhongDTO phong) : this()
        {
            cmbLoaiPhong.DisplayMemberPath = "TenLoaiPhong";
            cmbLoaiPhong.SelectedValuePath = "MaLoaiPhong";

            txtSoPhong.IsReadOnly = true;
            txtSoPhong.Text = phong.SoPhong;
            cmbTinhTrang.Text = phong.TinhTrang;
            cmbLoaiPhong.Text = phong.LoaiPhong;
            txbTitle.Text = "Sửa thông tin phòng " + phong.SoPhong;
            soPhong = phong.SoPhong;
        }

        #region Method
        private bool KiemTra()
        {
            if (string.IsNullOrWhiteSpace(txtSoPhong.Text))
            {
                new DialogCustoms("Vui lòng nhập số phòng","Thông báo",DialogCustoms.OK).Show();
                return false;
            }
            if (string.IsNullOrWhiteSpace(cmbLoaiPhong.Text))
            {
                new DialogCustoms("Vui lòng chọn loại phòng", "Thông báo", DialogCustoms.OK).Show();
                return false;
            }
            if (string.IsNullOrWhiteSpace(cmbTinhTrang.Text))
            {
                new DialogCustoms("Vui lòng chọn tình trạng", "Thông báo", DialogCustoms.OK).Show();
                return false;
            }
            else
            {
                int so;
                if (int.TryParse(txtSoPhong.Text, out so) == true || KiemTraTenPhong() == false)
                {
                    new DialogCustoms("Vui lòng nhập đúng định dạng số phòng ", "Thông báo", DialogCustoms.OK).Show();
                    return false;
                }
            }
            return true;
        }

        public bool KiemTraTenPhong()
        {
            string str = txtSoPhong.Text;
            int so;
            if (str[0].Equals('P') && int.TryParse(str[1].ToString(), out so) && int.TryParse(str[2].ToString(), out so) && int.TryParse(str[3].ToString(), out so))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region Event
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
                PhongDTO phong = new PhongDTO()
                {
                    SoPhong = txtSoPhong.Text,
                    TinhTrang = cmbTinhTrang.Text,
                    LoaiPhong = cmbLoaiPhong.SelectedValue.ToString()
                };
                if (truyen != null)
                {
                    truyen(phong);
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
                PhongDTO phong = new PhongDTO()
                {
                    SoPhong = txtSoPhong.Text,
                    TinhTrang = cmbTinhTrang.Text,
                    LoaiPhong = cmbLoaiPhong.SelectedValue.ToString()
                };
                if (sua != null)
                {
                    sua(phong);
                }
            }
            Window wd = Window.GetWindow(sender as Button);
            wd.Close();
        }
        #endregion
    }
}
