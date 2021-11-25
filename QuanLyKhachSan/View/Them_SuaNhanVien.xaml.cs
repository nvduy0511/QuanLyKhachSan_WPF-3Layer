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
using DAL;


namespace GUI.View
{
    
    public partial class Them_SuaNhanVien : Window
    {
        public delegate void CRUD(NhanVien nv);

        public CRUD themNhanVien;
        public CRUD suaNhanVien;
        public CRUD truyenNhanVien;
        private string maNV;

        public Them_SuaNhanVien()
        {
            InitializeComponent();
            truyenNhanVien = new CRUD(nhanData);
        }
        public void nhanData(NhanVien nv)
        {
            txbTitle.Text = "Sửa nhân viên";
            txbHoTenNV.Text = nv.HoTen;
            txbCCCD.Text = nv.CCCD;
            txbChucVu.Text = nv.ChucVu;
            txbDiaChi.Text = nv.DiaChi;
            txbLuong.Text = nv.Luong.ToString();
            txbSDT.Text = nv.SDT;
            cbGioiTinh.Text = nv.GioiTinh;
            dtNTNS.Text = nv.NTNS.ToString();
            maNV = nv.MaNV.ToString();
        }
        private bool kiemTraDayDuThongTin()
        {
            if(string.IsNullOrWhiteSpace(txbHoTenNV.Text))
            {
                txbHoTenNV.Focus();
                new DialogCustoms("Nhập đầy đủ họ tên !", "Thông báo", DialogCustoms.OK).ShowDialog();
                return false;
            }
            // Ngày tháng năm sinh
            if (string.IsNullOrWhiteSpace(dtNTNS.Text))
            {
                dtNTNS.Focus();
                new DialogCustoms("Nhập đúng định dạng ngày tháng năm sinh !", "Thông báo", DialogCustoms.OK).ShowDialog();
                return false;
            }
            else
            {
                if(!DateTime.TryParse(dtNTNS.Text, out DateTime dt))
                {
                    dtNTNS.Focus();
                    new DialogCustoms("Nhập đúng định dạng ngày tháng năm sinh !", "Thông báo", DialogCustoms.OK).ShowDialog();
                    return false;
                }
            }
            //Căn cước công dân
            if (string.IsNullOrWhiteSpace(txbCCCD.Text))
            {
                txbCCCD.Focus();
                new DialogCustoms("Nhập đầy đủ căn cước công dân !", "Thông báo", DialogCustoms.OK).ShowDialog();
                return false;
            }
            else
            {
                if(!long.TryParse(txbCCCD.Text, out long cccd))
                {
                    txbCCCD.Focus();
                    new DialogCustoms("Nhập căn cước công dân đúng định dạng số !", "Thông báo", DialogCustoms.OK).ShowDialog();
                    return false;
                }
                if (txbCCCD.Text.Length > 12)
                {
                    txbCCCD.Focus();
                    new DialogCustoms("Nhập căn cước công dân không quá 12 ký tự !", "Thông báo", DialogCustoms.OK).ShowDialog();
                    return false;
                }
            }
            // Lương
            if (string.IsNullOrWhiteSpace(txbLuong.Text))
            {
                txbLuong.Focus();
                new DialogCustoms("Nhập đầy đủ lương !", "Thông báo", DialogCustoms.OK).ShowDialog();
                return false;
            }
            else
            {
                if(!decimal.TryParse(txbLuong.Text, out decimal luong))
                {
                    txbLuong.Focus();
                    new DialogCustoms("Nhập lương đúng định dạng số !", "Thông báo", DialogCustoms.OK).ShowDialog();
                    return false;
                }
            }
            // Số điện thoại
            if (string.IsNullOrWhiteSpace(txbSDT.Text))
            {
                txbSDT.Focus();
                new DialogCustoms("Nhập đầy đủ số điện thoại !", "Thông báo", DialogCustoms.OK).ShowDialog();
                return false;
            }
            else
            {
                if(!long.TryParse(txbSDT.Text, out long sdt))
                {
                    txbSDT.Focus();
                    new DialogCustoms("Nhập số điện thoại đúng định dạng số !", "Thông báo", DialogCustoms.OK).ShowDialog();
                    return false;
                }
                if (txbSDT.Text.Length > 10)
                {
                    txbSDT.Focus();
                    new DialogCustoms("Nhập số điện thoại không quá 10 ký tự !", "Thông báo", DialogCustoms.OK).ShowDialog();
                    return false;
                }
            }
            // Giới tính
            if (string.IsNullOrWhiteSpace(cbGioiTinh.Text))
            {
                cbGioiTinh.Focus();
                new DialogCustoms("Nhập đầy đủ giới tính !", "Thông báo", DialogCustoms.OK).ShowDialog();
                return false;
            }
            //Địa chỉ
            if (string.IsNullOrWhiteSpace(txbDiaChi.Text))
            {
                txbDiaChi.Focus();
                new DialogCustoms("Nhập đầy đủ địa chỉ !", "Thông báo", DialogCustoms.OK).ShowDialog();
                return false;
            }
            // Chức vụ
            if (string.IsNullOrWhiteSpace(txbChucVu.Text))
            {
                txbChucVu.Focus();
                new DialogCustoms("Nhập đầy đủ chức vụ !", "Thông báo", DialogCustoms.OK).ShowDialog();
                return false;
            }
            //nếu đúng hết thì trả về false
            return true;

        }
        #region event
        private void click_Huy(object sender, RoutedEventArgs e)
        {
            Window wd = Window.GetWindow(sender as Button);
            wd.Close();
            
        }

        private void click_ThemNV(object sender, RoutedEventArgs e)
        {

            if (kiemTraDayDuThongTin())
            {
                NhanVien nhanVien = new NhanVien()
                {
                    HoTen = txbHoTenNV.Text,
                    CCCD = txbCCCD.Text,
                    ChucVu = txbChucVu.Text,
                    DiaChi = txbDiaChi.Text,
                    GioiTinh = cbGioiTinh.Text,
                    Luong = (decimal.Parse(txbLuong.Text)),
                    NTNS = DateTime.Parse(dtNTNS.SelectedDate.ToString()),
                    SDT = txbSDT.Text
                };
                if (themNhanVien != null)
                {
                    themNhanVien(nhanVien);
                }
                this.Close();
            }
            
        }

        private void click_Sua(object sender, RoutedEventArgs e)
        {
            
            if (kiemTraDayDuThongTin())
            {
                NhanVien nhanVien = new NhanVien()
                {
                    MaNV = int.Parse(maNV),
                    HoTen = txbHoTenNV.Text,
                    CCCD = txbCCCD.Text,
                    ChucVu = txbChucVu.Text,
                    DiaChi = txbDiaChi.Text,
                    GioiTinh = cbGioiTinh.Text,
                    Luong = (decimal.Parse(txbLuong.Text)),
                    NTNS = DateTime.Parse(dtNTNS.SelectedDate.ToString()),
                    SDT = txbSDT.Text
                };

                if (suaNhanVien != null)
                {
                    suaNhanVien(nhanVien);
                }
                this.Close();
            }
        }
        #endregion
    }
}
