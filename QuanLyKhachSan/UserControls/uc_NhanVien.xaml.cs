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
using System.Windows.Navigation;
using System.Windows.Shapes;
using GUI.View;
using System.Collections.ObjectModel;
using BUS;
using DAL;
using System.Text.RegularExpressions;

namespace GUI.UserControls
{

    public partial class uc_NhanVien : UserControl
    {
        ObservableCollection<NhanVien> list;
        public uc_NhanVien()
        {
            InitializeComponent();
            list = new ObservableCollection<NhanVien>(NhanVienBUS.GetInstance().getDataNhanVien());
            lvNhanVien.ItemsSource = list;

        }
        #region method
        void nhanData(NhanVien nv)
        {
            list.Add(nv);
            if (NhanVienBUS.GetInstance().addNhanVien(nv))
                new DialogCustoms("Thêm nhân viên thành công!", "Thông báo", DialogCustoms.OK).ShowDialog();

        }

        void SuaThongTinNhanVien(NhanVien nv)
        {
            // sửa để update lên list view
            NhanVien nhanVien_Sua = list.Where(s => s.MaNV.Equals(nv.MaNV)).FirstOrDefault();
            nhanVien_Sua.HoTen = nv.HoTen;
            nhanVien_Sua.GioiTinh = nv.GioiTinh;
            nhanVien_Sua.NTNS = nv.NTNS;
            nhanVien_Sua.Luong = nv.Luong;
            nhanVien_Sua.SDT = nv.SDT;
            nhanVien_Sua.CCCD = nv.CCCD;
            nhanVien_Sua.ChucVu = nv.ChucVu;
            nhanVien_Sua.DiaChi = nv.DiaChi;

            if (NhanVienBUS.GetInstance().updateNhanVien(nv))
            {
                new DialogCustoms("Cập nhật nhân viên thành công!", "Thông báo", DialogCustoms.OK).ShowDialog();
            }

        }
        public string RemoveVietnameseTone(string text)
        {
            string result = text.ToLower();
            result = Regex.Replace(result, "à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ|/g", "a");
            result = Regex.Replace(result, "è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ|/g", "e");
            result = Regex.Replace(result, "ì|í|ị|ỉ|ĩ|/g", "i");
            result = Regex.Replace(result, "ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ|/g", "o");
            result = Regex.Replace(result, "ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ|/g", "u");
            result = Regex.Replace(result, "ỳ|ý|ỵ|ỷ|ỹ|/g", "y");
            result = Regex.Replace(result, "đ", "d");
            return result;
        }
        private bool filterTimKiem(object obj)
        {
            if (String.IsNullOrEmpty(txbTimKiem.Text))
                return true;
            else
            {
                string objTenNV = RemoveVietnameseTone((obj as NhanVien).HoTen);
                string timkiem = RemoveVietnameseTone(txbTimKiem.Text);
                return objTenNV.Contains(timkiem);
            }
        }
        #endregion

        #region event
        private void click_ThemNV(object sender, RoutedEventArgs e)
        {
            Them_SuaNhanVien tnv = new Them_SuaNhanVien();
            tnv.themNhanVien = new Them_SuaNhanVien.CRUD(nhanData);
            tnv.ShowDialog();
        }
        

        private void click_XoaNV(object sender, RoutedEventArgs e)
        {
            NhanVien nv = (sender as Button).DataContext as NhanVien;
            DialogCustoms dlg = new DialogCustoms("Bạn có muốn xóa nhân viên " + nv.HoTen, "Thông báo", DialogCustoms.YesNo);
            if(dlg.ShowDialog() == true )
            {
                if (NhanVienBUS.GetInstance().deleteNhanVien(nv))
                {
                    list.Remove(nv);
                    new DialogCustoms("Xóa nhân viên thành công !", "Thông báo", DialogCustoms.OK).ShowDialog();
                }
                    
            }
        }

        private void click_SuaNV(object sender, RoutedEventArgs e)
        {
            NhanVien nv = (sender as Button).DataContext as NhanVien;
            Them_SuaNhanVien themNhanVien = new Them_SuaNhanVien();
            themNhanVien.truyenNhanVien(nv);
            themNhanVien.suaNhanVien = new Them_SuaNhanVien.CRUD(SuaThongTinNhanVien);
            themNhanVien.ShowDialog();
        }


        #endregion

        private void TimKiem_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionView viewNV = (CollectionView)CollectionViewSource.GetDefaultView(lvNhanVien.ItemsSource);
            viewNV.Filter = filterTimKiem;
        }
        

    }

}
