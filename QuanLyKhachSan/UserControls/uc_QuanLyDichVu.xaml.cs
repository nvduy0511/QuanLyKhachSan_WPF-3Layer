using BUS;
using DAL.DTO;
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
    /// Interaction logic for uc_QuanLyDichVu.xaml
    /// </summary>
    public partial class uc_QuanLyDichVu : UserControl
    {
        ObservableCollection<DichVuDTO> list;

        public uc_QuanLyDichVu()
        {
            InitializeComponent();
            TaiDanhSach();


            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lsvDichVu.ItemsSource);
            view.Filter = DichVuFilter;
        }

        private void TaiDanhSach()
        {
            list = new ObservableCollection<DichVuDTO>(DichVuBUS.GetInstance().getDichVu());
            lsvDichVu.ItemsSource = list;

        }

        private bool DichVuFilter(object obj)
        {
            if (String.IsNullOrEmpty(txtFilter.Text))
                return true;
            else
                return (obj as DichVuDTO).TenDichVu.IndexOf(txtFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        void nhanData(DichVuDTO dv)
        {
            if (DichVuBUS.GetInstance().KiemTraTrungTen(dv))
            {
                if (DichVuBUS.GetInstance().ThemDichVu(dv))
                {
                    new DialogCustoms("Thêm thành công", "Thông báo", DialogCustoms.OK).Show();
                    TaiDanhSach();
                }
            }
            else
            {
                new DialogCustoms("Tên dịch vụ đã tồn tại","Thông báo", DialogCustoms.OK).Show();
            }
        }

        void capNhatData(DichVuDTO dv)
        {
            if (DichVuBUS.GetInstance().capNhatDichVu(dv))
            {
                new DialogCustoms("Cập nhật thành công", "Thông báo", DialogCustoms.OK).Show();
                TaiDanhSach();
            }
        }

        private void txtFilter_TextChanged(object sender, TextChangedEventArgs e)
        {

            CollectionViewSource.GetDefaultView(lsvDichVu.ItemsSource).Refresh();
        }

        private void btnXoa_Click(object sender, RoutedEventArgs e)
        {
            DichVuDTO dv = (sender as Button).DataContext as DichVuDTO;

            var thongbao = new DialogCustoms("Bạn có thật sự muốn xóa " + dv.TenDichVu, "Thông báo", DialogCustoms.YesNo);
            
            if (thongbao.ShowDialog() == true)
            {
                new DialogCustoms("Xoá thành công","Thông báo", DialogCustoms.OK).Show();
                DichVuBUS.GetInstance().xoaDataDichVu(dv);
                TaiDanhSach();
            }
        }

        private void btnThem_Click(object sender, RoutedEventArgs e)
        {
            Them_SuaDichVu ThemDichVu = new Them_SuaDichVu();
            ThemDichVu.truyen = new Them_SuaDichVu.TryenDuLieu(nhanData);
            ThemDichVu.ShowDialog();
        }

        private void btnCapNhat_Click(object sender, RoutedEventArgs e)
        {
            DichVuDTO dv = (sender as Button).DataContext as DichVuDTO;
            Them_SuaDichVu CapNhatDichVu = new Them_SuaDichVu(dv);
            CapNhatDichVu.sua = new Them_SuaDichVu.SuaDuLieu(capNhatData);
            CapNhatDichVu.ShowDialog();
        }
    }
}
