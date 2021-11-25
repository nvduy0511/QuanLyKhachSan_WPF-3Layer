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
    /// Interaction logic for uc_QuanLyTienNghi.xaml
    /// </summary>
    public partial class uc_QuanLyTienNghi : UserControl
    {
        ObservableCollection<TienNghi> list;

        public uc_QuanLyTienNghi()
        {
            InitializeComponent();
            TaiDanhSach();

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lsvTienNghi.ItemsSource);
            view.Filter = TienNghiFilter;
        }

        private void TaiDanhSach()
        {
            list = new ObservableCollection<TienNghi>(TienNghiBUS.Instance.getDataTienNghi());
            lsvTienNghi.ItemsSource = list;
        }

        private bool TienNghiFilter(object obj)
        {
            if (String.IsNullOrEmpty(txtFilter.Text))
                return true;
            else
                return (obj as TienNghi).TenTN.IndexOf(txtFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private void btnSuaTienNghi_Click(object sender, RoutedEventArgs e)
        {
            TienNghi tienNghi = (sender as Button).DataContext as TienNghi;
            if(tienNghi!= null)
            {
                Them_SuaTienNghi CapNhatTienNghi = new Them_SuaTienNghi(tienNghi);
                CapNhatTienNghi.suaTN = new Them_SuaTienNghi.suaData(capNhatData);
                CapNhatTienNghi.ShowDialog();
            }
            
        }

        private void btnXoaTienNghi_Click(object sender, RoutedEventArgs e)
        {
            TienNghi tn = (sender as Button).DataContext as TienNghi;
            var thongbao = new DialogCustoms("Bạn có thật sự muốn xóa tiện nghi " + tn.TenTN, "Thông báo", DialogCustoms.YesNo);

            if (thongbao.ShowDialog() == true)
            {
                new DialogCustoms("Xoá thành công", "Thông báo", DialogCustoms.OK).Show();
                TienNghiBUS.Instance.xoaTienNghi(tn);
                TaiDanhSach();
            }
        }

        private void btnThemTienNghi_Click(object sender, RoutedEventArgs e)
        {
            Them_SuaTienNghi ThemTienNghi = new Them_SuaTienNghi();
            ThemTienNghi.truyenTN = new Them_SuaTienNghi.truyenData(nhanData);
            ThemTienNghi.ShowDialog();
        }

        void nhanData(TienNghi tn)
        {
            if (TienNghiBUS.Instance.KiemTraTenTienNghi(tn))
            {
                if (TienNghiBUS.Instance.addTienNghi(tn))
                {
                    new DialogCustoms("Thêm thành công", "Thông báo", DialogCustoms.OK).ShowDialog();
                    TaiDanhSach();
                }
            }
            else
            {
                new DialogCustoms("Tên tiện nghi đã tồn tại", "Thông báo", DialogCustoms.OK).ShowDialog();
            }

        }
        void capNhatData(TienNghi tn)
        {
            if (TienNghiBUS.Instance.KiemTraTenTienNghi(tn))
            {
                if (TienNghiBUS.Instance.capNhatTienNghi(tn))
                {
                    new DialogCustoms("Cập nhật thành công","Thông báo", DialogCustoms.OK).ShowDialog();
                    TaiDanhSach();
                }
            }
            else
            {
                new DialogCustoms("Tên tiện nghi đã tồn tại", "Thông báo", DialogCustoms.OK).ShowDialog();
            }

        }

        private void txtFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(lsvTienNghi.ItemsSource).Refresh();
        }
    }
}
