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
    /// Interaction logic for Them_SuaDichVu.xaml
    /// </summary>
    public partial class Them_SuaDichVu : Window
    {
        public delegate void TryenDuLieu(DichVuDTO dv);
        public delegate void SuaDuLieu(DichVuDTO dv);

        public TryenDuLieu truyen;
        public SuaDuLieu sua;
        private List<LoaiDV> loaiDV;
        private string maDV;
        public Them_SuaDichVu()
        {
            InitializeComponent();
            loaiDV = new List<LoaiDV>(LoaiDichVuBUS.Instance.getDataLoaiDV());
            cmbMaLoai.ItemsSource = loaiDV;
            cmbMaLoai.DisplayMemberPath = "TenLoaiDV";
            cmbMaLoai.SelectedValuePath = "MaLoaiDV";
        }
        public Them_SuaDichVu(DichVuDTO dv) : this()
        {
            txtTenDichVu.IsReadOnly = true;
            cmbMaLoai.DisplayMemberPath = "TenLoaiDV";
            cmbMaLoai.SelectedValuePath = "MaLoaiDV";
            
            txtTenDichVu.Text = dv.TenDichVu;
            cmbMaLoai.Text = dv.LoaiDichVu;
            txtGia.Text = dv.Gia.ToString();
            txbTitle.Text = "Sửa thông tin dịch vụ " + dv.MaDichVu;
            maDV = dv.MaDichVu.ToString();
        }

        private void btnHuy_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnCapNhat_Click(object sender, RoutedEventArgs e)
        {
            if (!KiemTra())
            {
                return;
            }
            else
            {
                DichVuDTO dichVu = new DichVuDTO()
                {
                    MaDichVu = int.Parse(maDV.ToString()),
                    TenDichVu = txtTenDichVu.Text,
                    Gia = int.Parse(txtGia.Text),
                    LoaiDichVu = cmbMaLoai.SelectedValue.ToString()
                };
                if (sua != null)
                {
                    sua(dichVu);
                }
            }
            Window wd = Window.GetWindow(sender as Button);
            wd.Close();
        }

        private void btnThem_Click(object sender, RoutedEventArgs e)
        {

            if (!KiemTra())
            {
                return;
            }
            else
            {
                DichVuDTO dichVu = new DichVuDTO()
                {
                    TenDichVu = txtTenDichVu.Text,
                    Gia = int.Parse(txtGia.Text),
                    LoaiDichVu = cmbMaLoai.SelectedValue.ToString()
                };
                if (truyen != null)
                {
                    truyen(dichVu);
                }
            }
            Window wd = Window.GetWindow(sender as Button);
            wd.Close();
        }

        private bool KiemTra()
        {
            if (string.IsNullOrWhiteSpace(txtTenDichVu.Text))
            {
                new DialogCustoms("Vui lòng nhập tên dịch vụ","Thông báo",DialogCustoms.OK).Show();
                return false;
            }
            if (string.IsNullOrWhiteSpace(cmbMaLoai.Text))
            {
                new DialogCustoms("Vui lòng chọn mã loại dịch vụ", "Thông báo", DialogCustoms.OK).Show();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtGia.Text))
            {
                new DialogCustoms("Vui lòng nhập giá", "Thông báo", DialogCustoms.OK).Show();
                return false;
            }
            else
            {
                int so;
                if (int.TryParse(txtTenDichVu.Text, out so) == true)
                {
                    new DialogCustoms("Vui lòng nhập đúng định đạng tên dịch vụ", "Thông báo", DialogCustoms.OK).Show();
                    return false;
                }
                if (int.TryParse(txtGia.Text, out so) == false)
                {
                    new DialogCustoms("Vui lòng nhập đúng định đạng giá", "Thông báo", DialogCustoms.OK).Show();
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
