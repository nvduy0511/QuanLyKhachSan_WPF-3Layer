using BUS;
using DAL.Data;
using DAL.DTO;
using GUI.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Objects;
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
    /// Interaction logic for uc_HoaDon.xaml
    /// </summary>
    public partial class uc_HoaDon : UserControl
    {
        ObservableCollection<HoaDonDTO> list;
        public uc_HoaDon()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            TaiDanhSach();
            Console.WriteLine("Load Hoa Don");
        }
        private void TaiDanhSach()
        {
            list = new ObservableCollection<HoaDonDTO>(HoaDonBUS.GetInstance().GetHoaDons());
            lsvHoaDon.ItemsSource = list;
        }

        private void txtFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lsvHoaDon.ItemsSource);
            view.Filter = HoaDonFilter;
            CollectionViewSource.GetDefaultView(lsvHoaDon.ItemsSource).Refresh();
        }
        private bool HoaDonFilter(object obj)
        {
            if (String.IsNullOrEmpty(txtFilter.Text))
            {
                return true;
            }
            else
                return (obj as HoaDonDTO).MaHoaDon == int.Parse(txtFilter.Text);
        }
        private bool HoaDonFilterTheoNgay(object obj)
        {
            if (String.IsNullOrEmpty(dtpChonNgay.Text))
                return true;
            else
                return (obj as HoaDonDTO).NgayLap.ToShortDateString().Equals(dtpChonNgay.Text) ;
        }

        private void dtpChonNgay_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lsvHoaDon.ItemsSource);
            view.Filter = HoaDonFilterTheoNgay;
            CollectionViewSource.GetDefaultView(lsvHoaDon.ItemsSource).Refresh();
        }

        private void chiTiet_Click(object sender, RoutedEventArgs e)
        {
            HoaDonDTO hd = (sender as Button).DataContext as HoaDonDTO;
            new XuatHoaDon(hd.MaHoaDon).ShowDialog();
        }
    }
}
