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
    /// Interaction logic for Them_SuaChiTietTienNghi.xaml
    /// </summary>
    public partial class Them_SuaChiTietTienNghi : Window
    {
        public delegate void truyenData(CT_TienNghiDTO CTTienNghi);
        public delegate void suaData(CT_TienNghiDTO CTTienNghi);


        public truyenData truyenCT;
        public suaData suaCT;
        List<TienNghi> TienNghis;
        private string maCT;
        public Them_SuaChiTietTienNghi()
        {
            InitializeComponent();
            TaiDanhSach();
        }

        public Them_SuaChiTietTienNghi(CT_TienNghiDTO ct) : this()
        {
            cmbSoPhong.DisplayMemberPath = "SoPhong";
            cmbSoPhong.SelectedValuePath = "SoPhong";

            cmbTienNghi.DisplayMemberPath = "TenTN";
            cmbTienNghi.SelectedValuePath = "MaTN";
            cmbTienNghi.IsReadOnly = true;


            cmbTienNghi.Text = ct.TenTN;
            cmbSoPhong.Text = ct.SoPhong;
            txtSoLuong.Text = ct.SoLuong.ToString();
            txbTitle.Text = "Sửa thông tin " + ct.MaCT;
            maCT = ct.MaCT.ToString();
        }
        private void TaiDanhSach()
        {
            TienNghis = new List<TienNghi>(TienNghiBUS.Instance.getDataTienNghi());
            cmbSoPhong.ItemsSource = PhongBUS.GetInstance().getDataPhong();
            cmbTienNghi.ItemsSource = TienNghis;
            cmbSoPhong.DisplayMemberPath = "SoPhong";
            cmbSoPhong.SelectedValuePath = "SoPhong";
            cmbTienNghi.DisplayMemberPath = "TenTN";
            cmbTienNghi.SelectedValuePath = "MaTN";
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
                CT_TienNghiDTO CTTienNghi = new CT_TienNghiDTO()
                {
                    SoPhong = cmbSoPhong.SelectedValue.ToString(),
                    SoLuong = int.Parse(txtSoLuong.Text),
                    TenTN = cmbTienNghi.SelectedValue.ToString()
                };
                if (truyenCT != null)
                {
                    truyenCT(CTTienNghi);
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
                CT_TienNghiDTO ctTienNghi = new CT_TienNghiDTO()
                {
                    MaCT = int.Parse(maCT),
                    SoPhong = cmbSoPhong.SelectedValue.ToString(),
                    SoLuong = int.Parse(txtSoLuong.Text),
                    TenTN = cmbTienNghi.SelectedValue.ToString()
                };
                if (suaCT != null)
                {
                    suaCT(ctTienNghi);
                }
            }
            Window wd = Window.GetWindow(sender as Button);
            wd.Close();
        }
        private bool KiemTra()
        {
            if (string.IsNullOrWhiteSpace(cmbSoPhong.Text))
            {
                new DialogCustoms("Vui lòng chọn số phòng","Thông báo",DialogCustoms.OK).Show();
                return false;
            }
            if (string.IsNullOrWhiteSpace(cmbTienNghi.Text))
            {
                new DialogCustoms("Vui lòng chọn tên tiện nghi", "Thông báo", DialogCustoms.OK).Show();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtSoLuong.Text))
            {
                new DialogCustoms("Vui lòng nhập số lượng", "Thông báo", DialogCustoms.OK).Show();
                return false;
            }
            else
            {
                int so;
                if (int.TryParse(txtSoLuong.Text, out so) == false)
                {
                    new DialogCustoms("Vui lòng nhập đúng định đạng số", "Thông báo", DialogCustoms.OK).Show();
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
