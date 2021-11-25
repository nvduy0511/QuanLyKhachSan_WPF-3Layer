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
using System.Windows.Shapes;
using BUS;
using DAL.DTO;
using DAL;
using System.Text.RegularExpressions;

namespace GUI.View
{
    /// <summary>
    /// Interaction logic for CTP_ThemDV.xaml
    /// </summary>
    public partial class CTP_ThemDV : Window
    {
        public delegate void Delegate_CTPDV (ObservableCollection<DichVu_DaChon> obDVCT);
        public Delegate_CTPDV truyenData;

        ObservableCollection<DichVu_Custom> lsdichVu_Customs;
        ObservableCollection<DichVu_DaChon> lsDichVu_DaChon;
        List<string> lsLoaiDV;
        List<DichVu_Custom> lsCache;

        private int? maCTPhieuThue;
        public CTP_ThemDV()
        {
            InitializeComponent();
        }
        public CTP_ThemDV(int? mactpt): this()
        {
            maCTPhieuThue = mactpt;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lsdichVu_Customs = new ObservableCollection<DichVu_Custom>( DichVuBUS.GetInstance().getDichVu_Custom() );
            lsDichVu_DaChon = new ObservableCollection<DichVu_DaChon>();
            lsCache = new List<DichVu_Custom>();
            lsLoaiDV = new List<string>();
            lsLoaiDV = DichVuBUS.GetInstance().getLoaiDichVu();
            lsLoaiDV.Add("Tất cả");
            lvDanhSachDV.ItemsSource = lsdichVu_Customs;
            lvDichVuDaChon.ItemsSource = lsDichVu_DaChon;
            cbTimKiemLoaiDV.ItemsSource = lsLoaiDV;
        }

        private void click_Them(object sender, RoutedEventArgs e)
        {
            DichVu_Custom dvct = (sender as Button).DataContext as DichVu_Custom;
            lsDichVu_DaChon.Add(new DichVu_DaChon() {ThanhTien = dvct.Gia,  TenDV = dvct.TenDV, SoLuong = 1, MaDV = dvct.MaDV , Gia = dvct.Gia});
            lsCache.Add(dvct);
            lsdichVu_Customs.Remove(dvct);

        }

        private void click_Xoa(object sender, RoutedEventArgs e)
        {
            DichVu_DaChon dvdachon = (sender as Button).DataContext as DichVu_DaChon;
            DichVu_Custom dichVu_Custom = (lsCache.Where(p => p.MaDV.Equals(dvdachon.MaDV) ) ).FirstOrDefault() ;
            lsdichVu_Customs.Add(dichVu_Custom);
            lsDichVu_DaChon.Remove(dvdachon);

        }

        private void click_Thoat(object sender, RoutedEventArgs e)
        {
            Window wd = Window.GetWindow(sender as Button);
            wd.Close();
        }

        private void click_Luu(object sender, RoutedEventArgs e)
        {
            string error = string.Empty;
            int dem = 0;
            foreach (var item in lsDichVu_DaChon)
            {
                CT_SDDichVu ct = new CT_SDDichVu() 
                { 
                    MaCTPT = maCTPhieuThue,
                    MaDV = item.MaDV,
                    SL = item.SoLuong == null ? 0: int.Parse(item.SoLuong.ToString()),
                    ThanhTien = item.ThanhTien == null ? 0: decimal.Parse(item.ThanhTien.ToString())
                };
                
                if( CTSDDV_BUS.GetInstance().addDataCTSDDC(ct, out error) )
                {
                    dem++;
                }
                else 
                {
                    new DialogCustoms("Lỗi: "+error, "Thông báo", DialogCustoms.OK).ShowDialog();
                }
            }
            if(dem == lsDichVu_DaChon.Count)
            {
                new DialogCustoms("Thêm dịch vụ sử dụng thành công !", "Thông báo", DialogCustoms.OK).ShowDialog();
                if (truyenData != null)
                {
                    truyenData(lsDichVu_DaChon);
                }
            }

            this.Close();
        }

        private void txbSoLuong_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox txb = sender as TextBox;
            DichVu_DaChon dvdc = (sender as TextBox).DataContext as DichVu_DaChon;
            int soLuong = 1;
            if (!int.TryParse(txb.Text, out soLuong))
            {
                new DialogCustoms("Lỗi: Nhập số lượng kiểu số nguyên!", "Thông báo", DialogCustoms.OK).ShowDialog();
                return;
            }
            dvdc.SoLuong = soLuong;
            dvdc.ThanhTien = dvdc.Gia * soLuong;
        }
        private void txbSoLuong_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                TextBox txb = sender as TextBox;
                DichVu_DaChon dvdc = (sender as TextBox).DataContext as DichVu_DaChon;
                int soLuong = 1;
                if (!int.TryParse(txb.Text, out soLuong))
                {
                    new DialogCustoms("Lỗi: Nhập số lượng kiểu số nguyên!", "Thông báo", DialogCustoms.OK).ShowDialog();
                    return;
                }
                dvdc.SoLuong = soLuong;
                dvdc.ThanhTien = dvdc.Gia * soLuong;
            }
        }
        private void txbTimKiem_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionView viewDV = (CollectionView)CollectionViewSource.GetDefaultView(lvDanhSachDV.ItemsSource);
            viewDV.Filter = filterTimKiem;
        }

        private void cbTimKiemLoaiDV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CollectionView viewDV = (CollectionView)CollectionViewSource.GetDefaultView(lvDanhSachDV.ItemsSource);
            viewDV.Filter = filterTimKiemLoaiDV;
        }
        #region method
        private bool filterTimKiem(object obj)
        {
            if (String.IsNullOrEmpty(txbTimKiem.Text))
                return true;
            else
            {
                string objTenDV = RemoveVietnameseTone((obj as DichVu_Custom).TenDV);
                string timkiem = RemoveVietnameseTone(txbTimKiem.Text);
                return objTenDV.Contains(timkiem);
            }
        }
        private bool filterTimKiemLoaiDV(object obj)
        {
            if (cbTimKiemLoaiDV.SelectedItem.ToString().Equals("Tất cả"))
                return true;
            else
            {
                string objTenDV = RemoveVietnameseTone((obj as DichVu_Custom).LoaiDV);
                string timkiem = RemoveVietnameseTone(cbTimKiemLoaiDV.SelectedItem.ToString());
                return objTenDV.Contains(timkiem);
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

        #endregion

        
    }
}
