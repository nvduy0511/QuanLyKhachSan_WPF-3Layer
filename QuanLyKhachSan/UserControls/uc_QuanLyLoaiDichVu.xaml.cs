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
    /// Interaction logic for uc_QuanLyLoaiDichVu.xaml
    /// </summary>
    public partial class uc_QuanLyLoaiDichVu : UserControl
    {
        ObservableCollection<LoaiDV> list;

        public uc_QuanLyLoaiDichVu()
        {
            InitializeComponent();
            TaiDanhSach();


            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lsvLoaiDV.ItemsSource);
            view.Filter = LoaiDVFilter;
        }

        #region Event
        private void btnThemLoaiDV_Click(object sender, RoutedEventArgs e)
        {
            Them_SuaLoaiDichVu themLoaiDichVu = new Them_SuaLoaiDichVu();
            themLoaiDichVu.truyenLoaiDV = new Them_SuaLoaiDichVu.truyenData(nhanData);
            themLoaiDichVu.ShowDialog();
        }

        private void btnSuaLoaiDV_Click(object sender, RoutedEventArgs e)
        {
            LoaiDV loaiDV = (sender as Button).DataContext as LoaiDV;
            Them_SuaLoaiDichVu capNhatLoaiDichVu = new Them_SuaLoaiDichVu(loaiDV);
            capNhatLoaiDichVu.suaLoaiDV = new Them_SuaLoaiDichVu.suaData(capNhatData);
            capNhatLoaiDichVu.ShowDialog();
        }

        private void btnXoaLoaiDV_Click(object sender, RoutedEventArgs e)
        {
            LoaiDV loaiDV = (sender as Button).DataContext as LoaiDV;
            var ThongBao = new DialogCustoms("Bạn có thật sự muốn xóa loại dịch vụ " + loaiDV.TenLoaiDV,"Thông báo",DialogCustoms.YesNo);
            if (ThongBao.ShowDialog() == true)
            {
                new DialogCustoms("Xóa thành công", "Thông báo", DialogCustoms.OK).Show();
                LoaiDichVuBUS.Instance.xoaLoaiDV(loaiDV);
                TaiDanhSach();
            }
        }
        #endregion

        #region Method
        private bool LoaiDVFilter(object obj)
        {
            if (String.IsNullOrEmpty(txtFilter.Text))
                return true;
            else
                return (obj as LoaiDV).TenLoaiDV.ToLower().Contains(txtFilter.Text.ToLower());
        }

        private void TaiDanhSach()
        {
            list = new ObservableCollection<LoaiDV>(LoaiDichVuBUS.Instance.getDataLoaiDV());
            lsvLoaiDV.ItemsSource = list;
        }

        void nhanData(LoaiDV loaiDV)
        {

            if (LoaiDichVuBUS.Instance.KiemTraTenLoaiDV(loaiDV))
            {
                if (LoaiDichVuBUS.Instance.addLoaiDV(loaiDV))
                {
                    new DialogCustoms("Thêm thành công", "Thông báo", DialogCustoms.OK).Show();
                    TaiDanhSach();
                }
            }
            else
            {
                new DialogCustoms("Tên loại dịch vụ đã tồn tại", "Thông báo", DialogCustoms.OK).Show();
            }
        }
        void capNhatData(LoaiDV loaiDV)
        {
            if (LoaiDichVuBUS.Instance.KiemTraTenLoaiDV(loaiDV))
            {
                if (LoaiDichVuBUS.Instance.capNhatDataLoaiDV(loaiDV))
                {
                    new DialogCustoms("Cập nhật thành công", "Thông báo", DialogCustoms.OK).Show();
                    TaiDanhSach();
                }
            }
            else
            {
                new DialogCustoms("Tên loại dịch vụ đã tồn tại","Thông báo", DialogCustoms.OK).Show();
            }
        }

        private void txtFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(lsvLoaiDV.ItemsSource).Refresh();
        }
        #endregion
    }
}
