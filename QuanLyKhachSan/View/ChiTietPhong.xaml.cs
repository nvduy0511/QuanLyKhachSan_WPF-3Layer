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
using DAL.DTO;
using BUS;
namespace GUI.View
{
    /// <summary>
    /// Interaction logic for ChiTietPhong.xaml
    /// </summary>
    public partial class ChiTietPhong : Window
    {
        
        public delegate void truyenDataPhong(Phong_Custom phong);
        public truyenDataPhong truyenData;

        ObservableCollection<DichVu_DaChon> obDichVu;
        private Phong_Custom phong_CTPhong;
        private int? maCTPhieuThue;
        private bool kiemTraSuaDoiTinhTrangDonDep;
        private bool kiemTraNhanPhong;
        private int maNV;
        public int MaNV { get => maNV; set => maNV = value; }

        public ChiTietPhong()
        {
            InitializeComponent();
            truyenData = new truyenDataPhong(setDataPhongCustom);
            kiemTraSuaDoiTinhTrangDonDep = false;
            kiemTraNhanPhong = false;
        }

        public ChiTietPhong(int maNV):this()
        {
            this.MaNV = maNV;
        }
        #region method
        void setDataPhongCustom(Phong_Custom phong)
        {
            //Nhận dữ liệu từ form cha và gán giá trị lên form con
            phong_CTPhong = phong;
            txblTieuDe.Text = phong.MaPhong;
            txblTenKH.Text = phong.TenKH;
            if (phong.IsDay == true)
            {
                icDayorHour.Kind = MaterialDesignThemes.Wpf.PackIconKind.CalendarToday;
                txblSoNgay.Text = phong.SoNgayO.ToString()+ "  ngày";
            }
            else
            {
                icDayorHour.Kind = MaterialDesignThemes.Wpf.PackIconKind.AlarmCheck;
                txblSoNgay.Text = phong.SoGio.ToString() + "  giờ";
            }
            txblSoNguoi.Text = phong.SoNguoi.ToString();
            txblNgayDen.Text = phong.NgayDen.ToString();
            cbTinhTrang.Text = phong.TinhTrang;
            cbDonDep.Text = phong.DonDep;
            kiemTraSuaDoiTinhTrangDonDep = false;
            //Lấy ra mã CT phiếu thuê
            maCTPhieuThue = phong.MaCTPT;
            //Lấy chi tiết sử dụng dịch vụ của phòng đó nếu có
            if(maCTPhieuThue != null)
            {
                obDichVu = new ObservableCollection<DichVu_DaChon>(CTSDDV_BUS.GetInstance().getCTSDDVtheoMaCTPT(maCTPhieuThue));
            }
            else
            {
                obDichVu = new ObservableCollection<DichVu_DaChon>();
            }

            lvSuDungDV.ItemsSource = obDichVu;

        }
        void nhanData(ObservableCollection<DichVu_DaChon> obDVCT)
        {
            foreach (var item in obDVCT)
            {
                obDichVu.Add(item);
            }
        }
        #endregion

        #region event
        private void click_Thoat(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        

        private void click_NhanPhong(object sender, RoutedEventArgs e)
        {
            kiemTraNhanPhong = true;
            cbTinhTrang.Text = "Phòng đang thuê";
        }

        private void click_ThanhToan(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Visibility = Visibility.Hidden;
            XuatHoaDon hoaDon = new XuatHoaDon(MaNV, phong_CTPhong, obDichVu);
            hoaDon.ShowDialog();
            this.Close();
        }

        private void click_ThemDV(object sender, RoutedEventArgs e)
        {
            CTP_ThemDV cTP_ThemDV = new CTP_ThemDV(maCTPhieuThue);
            cTP_ThemDV.truyenData = new CTP_ThemDV.Delegate_CTPDV(nhanData);
            cTP_ThemDV.ShowDialog();
        }
        private void cbDonDep_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
                kiemTraSuaDoiTinhTrangDonDep = true;
        }
        private void click_Luu(object sender, RoutedEventArgs e)
        {
            if (kiemTraSuaDoiTinhTrangDonDep)
            {
                
                string error = string.Empty;
                if( !PhongBUS.GetInstance().suaTinhTrangDonDep(phong_CTPhong.MaPhong, cbDonDep.Text, out  error))
                {
                    new DialogCustoms("Lưu thất bại !\n Lỗi:"+error, "Thông báo", DialogCustoms.OK).ShowDialog();
                    return;
                }
                else
                {
                    this.DialogResult = true;
                }
                this.Close();
            }
            if (kiemTraNhanPhong)
            {
                string error = string.Empty;
                if (!CT_PhieuThueBUS.GetInstance().suaTinhTrangThuePhong(phong_CTPhong.MaCTPT,"Phòng đang thuê", out error))
                {
                    new DialogCustoms("Lưu thất bại !\n Lỗi:" + error, "Thông báo", DialogCustoms.OK).ShowDialog();
                    return;
                }
                else
                {
                    this.DialogResult = true;
                }
                this.Close();
            }
            if( !kiemTraSuaDoiTinhTrangDonDep && !kiemTraNhanPhong)
            {
                this.Close();
            }
        }



        #endregion

        
    }
}
