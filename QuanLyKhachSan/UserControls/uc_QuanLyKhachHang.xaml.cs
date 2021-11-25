using BUS;
using DAL;
using GUI.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace GUI.UserControls
{
    /// <summary>
    /// Interaction logic for uc_QuanLyKhachHang.xaml
    /// </summary>
    public partial class uc_QuanLyKhachHang : UserControl
    {
        ObservableCollection<KhachHang> list;

        public uc_QuanLyKhachHang()
        {
            InitializeComponent();
            TaiDanhSach();

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lsvKhachHang.ItemsSource);
            view.Filter = KhachHangFilter;
        }

        private void TaiDanhSach()
        {
            list = new ObservableCollection<KhachHang>(KhachHangBUS.GetInstance().GetKhachHangs());
            lsvKhachHang.ItemsSource = list;
        }

        private bool KhachHangFilter(object obj)
        {
            if (String.IsNullOrEmpty(txtFilter.Text))
                return true;
            else
                return (obj as KhachHang).TenKH.IndexOf(txtFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private void btnThemKhachHang_Click(object sender, RoutedEventArgs e)
        {
            Them_SuaKhachHang nhapThongTinKhach = new Them_SuaKhachHang();
            nhapThongTinKhach.truyenKhachHang = new Them_SuaKhachHang.truyenData(nhanData);
            nhapThongTinKhach.ShowDialog();
        }

        private void btnCapNhat_Click(object sender, RoutedEventArgs e)
        {
            KhachHang khachHang = (sender as Button).DataContext as KhachHang;
            Them_SuaKhachHang CapNhatThongTinKhach = new Them_SuaKhachHang(khachHang);
            CapNhatThongTinKhach.suaKhachHang = new Them_SuaKhachHang.suaData(capNhatData);
            CapNhatThongTinKhach.ShowDialog();
        }

        private void btnXoa_Click(object sender, RoutedEventArgs e)
        {
            KhachHang khachHang = (sender as Button).DataContext as KhachHang;
            var ThongBao = new DialogCustoms("Bạn có thật sự muỗn xóa " + khachHang.TenKH, "Thông báo", DialogCustoms.YesNo);

            if (ThongBao.ShowDialog() == true)
            {
                if (KhachHangBUS.GetInstance().xoaDataKhachHang(khachHang))
                {
                    new DialogCustoms("Xóa thành công", "Thông báo", DialogCustoms.OK).Show();
                    TaiDanhSach();
                }
            }
        }

        void nhanData(KhachHang khachHang)
        {
            string error = string.Empty;
            if (KhachHangBUS.GetInstance().kiemTraTonTaiKhachHang(khachHang.CCCD.ToString()) == -1)
            {
                if(KhachHangBUS.GetInstance().addKhachHang(khachHang, out error))
                {
                    new DialogCustoms("Thêm thành công", "Thông báo", DialogCustoms.OK).Show();
                    TaiDanhSach();
                }
            }
            else
            {
                new DialogCustoms("Khách hàng đã có","Thông báo", DialogCustoms.OK).Show();
                return;
            }

           
        }

        void capNhatData(KhachHang khachHang)
        {
            if (KhachHangBUS.GetInstance().capNhatDataKhachHang(khachHang))
            {
                new DialogCustoms("Cập nhật thành công", "Thông báo", DialogCustoms.OK).Show();
                TaiDanhSach();
            }
        }

        private void txtFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(lsvKhachHang.ItemsSource).Refresh();
        }
    }
}
