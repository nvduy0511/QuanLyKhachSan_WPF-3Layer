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
using DAL;
using System.Data.Objects.SqlClient;

namespace GUI.View
{
    /// <summary>
    /// Interaction logic for XuatHoaDon.xaml
    /// </summary>
    public partial class XuatHoaDon : Window
    {
        private Phong_Custom phong;
        private int maNV;
        private List<DichVu_DaChon> ls;
        public Phong_Custom Phong { get => phong; set => phong = value; }
        public int MaNV { get => maNV; set => maNV = value; }

        public XuatHoaDon()
        {
            InitializeComponent();
        }
        public XuatHoaDon(int maNV, Phong_Custom ph, ObservableCollection<DichVu_DaChon> lsDV) : this()
        {
            this.MaNV = maNV;
            this.Phong = ph;
            ls = lsDV.ToList();
            try
            {
                decimal? tienPhong = PhongBUS.GetInstance().tinhTienPhong(Phong);
                decimal? tienDV = CTSDDV_BUS.GetInstance().tinhTongTienDichVuTheoMaCTPT(Phong.MaCTPT);
                txbSoPhong.Text = Phong.MaPhong;
                if (Phong.IsDay == true)
                {
                    txbSoNgayOrGio.Text = "Số ngày: ";
                    txbSoNgay.Text = Phong.SoNgayO.ToString();
                }
                else
                {
                    txbSoNgayOrGio.Text = "Số giờ: ";
                    txbSoNgay.Text = Phong.SoGio.ToString();
                }
                txbTenKH.Text = Phong.TenKH;
                txbSoNguoi.Text = Phong.SoNguoi.ToString();
                txbNhanVien.Text = NhanVienBUS.GetInstance().layNhanVienTheoMaNV(MaNV);
                txbNgayLapHD.Text = DateTime.Now.ToString();
                txbTongTien.Text = string.Format("{0:0,0 VND}", ((tienDV == null ? 0 : tienDV) + tienPhong));

                //Thêm hóa đơn vào DB
                HoaDon hd = new HoaDon()
                {
                    MaNV = this.MaNV,
                    MaCTPT = Phong.MaCTPT,
                    NgayLap = DateTime.Now,
                    TongTien = decimal.Parse(((tienDV == null ? 0 : tienDV) + tienPhong).ToString())
                };
                string error = string.Empty;
                if (!HoaDonBUS.GetInstance().themHoaDon(hd, out error))
                {
                    new DialogCustoms("Thêm hóa đơn thất bại!\nLỗi:" + error, "Thông báo", DialogCustoms.OK).ShowDialog();
                }
                txbSoHoaDon.Text = hd.MaHD.ToString();
                //Sửa trạng thái của ctpt
                string errorSuaCTPT = string.Empty;
                if (!CT_PhieuThueBUS.GetInstance().suaTinhTrangThuePhong(Phong.MaCTPT, "Đã thanh toán", out errorSuaCTPT))
                {
                    new DialogCustoms("Lỗi sửa CTPT\nLỗi:" + errorSuaCTPT, "Thông báo", DialogCustoms.OK).ShowDialog();
                }
                // Cập nhật lại tiền phòng và ngày trả thực tế
                string errorCapNhatCTPT = string.Empty;
                if(!CT_PhieuThueBUS.GetInstance().capNhatTienVaNgayTraThucTe(ph.MaCTPT ,tienPhong,DateTime.Now, out errorCapNhatCTPT))
                {
                    new DialogCustoms("Lỗi cập nhật CTPT\nLỗi:" + errorCapNhatCTPT, "Thông báo", DialogCustoms.OK).ShowDialog();
                }
                // Thêm 1 dịch vụ là thuê phòng vào 
                DichVu_DaChon dv = new DichVu_DaChon()
                {
                    SoLuong = Phong.IsDay == true ? Phong.SoNgayO : Phong.SoGio,
                    TenDV = "Thuê phòng",
                    Gia = PhongBUS.GetInstance().layTienPhongTheoSoPhong(Phong),
                    ThanhTien = tienPhong
                };
                ls.Add(dv);
                lvDichVuDaSD.ItemsSource = ls;
            }
            catch (Exception)
            {

                new DialogCustoms("Lỗi load thông tin!", "Thông báo", DialogCustoms.OK).ShowDialog();
            }
        }

        public XuatHoaDon(int mahd) : this()
        {
            try
            {
                HoaDon hd = HoaDonBUS.GetInstance().layHoaDonTheoMaHoaDon(mahd);
                if (hd == null)
                {
                    new DialogCustoms("Hóa đơn không tồn tại!", "Thông báo", DialogCustoms.OK).ShowDialog();
                    return;
                }
                txbNhanVien.Text = hd.NhanVien.HoTen;
                txbSoPhong.Text = hd.CT_PhieuThue.SoPhong;
                DateTime ngayBD = hd.CT_PhieuThue.NgayBD.Value;
                DateTime ngayKT = hd.CT_PhieuThue.NgayKT.Value;
                TimeSpan Time = ngayKT - ngayBD;
                int sogio = (int)Time.TotalHours;
                int songay = (int)Time.TotalDays + 1;
                bool isDay = false;
                if (sogio > 24)
                {
                    isDay = true;
                    txbSoNgayOrGio.Text = "Số ngày: ";
                    txbSoNgay.Text = songay.ToString();
                }
                else
                {
                    txbSoNgayOrGio.Text = "Số giờ: ";
                    txbSoNgay.Text = sogio.ToString();
                }
                txbSoHoaDon.Text = mahd.ToString();
                txbTenKH.Text = KhachHangBUS.GetInstance().layTenKhachHangTheoMaPT(hd.CT_PhieuThue.MaPhieuThue);
                txbSoNguoi.Text = hd.CT_PhieuThue.SoNguoiO.ToString();
                txbNgayLapHD.Text = hd.NgayLap.ToString();
                txbTongTien.Text = string.Format("{0:0,0 VND}", hd.TongTien);
                ls = new List<DichVu_DaChon>(CTSDDV_BUS.GetInstance().getCTSDDVtheoMaCTPT(hd.MaCTPT));
                DichVu_DaChon dv = new DichVu_DaChon()
                {
                    SoLuong = sogio > 24 ? songay : sogio,
                    TenDV = "Thuê phòng",
                    Gia = PhongBUS.GetInstance().layTienPhongTheoSoPhong(hd.CT_PhieuThue.SoPhong, isDay),
                    ThanhTien = hd.CT_PhieuThue.TienPhong
                };
                ls.Add(dv);
                lvDichVuDaSD.ItemsSource = ls;

            }
            catch (Exception ex)
            {
                new DialogCustoms("Lỗi: " + ex.Message, "Thông báo", DialogCustoms.OK).ShowDialog();
            }

        }

        private void click_InHoaDon(object sender, RoutedEventArgs e)
        {
            try
            {
                btnInHoaDon.Visibility = Visibility.Hidden;
                PrintDialog printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == true)
                {
                    printDialog.PrintVisual(print, "In Hóa đơn");
                    new DialogCustoms("In hóa đơn thành công!", "Thông báo", DialogCustoms.OK).ShowDialog();
                }
            }
            catch(Exception ex)
            {
                new DialogCustoms("In hóa đơn thất bại! \n Lỗi: "+ex.Message, "Thông báo", DialogCustoms.OK).ShowDialog();
            }
            finally
            {
                btnInHoaDon.Visibility = Visibility.Visible;
            }
        }

        
    }
}
