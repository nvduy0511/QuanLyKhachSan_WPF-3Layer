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
using DAL.DTO;
using BUS;
using System.Globalization;
using System.Collections.ObjectModel;

namespace GUI.View
{
    public partial class DatPhong : Window
    {
        public delegate void dlg();

        public dlg luuPhieuDatPhong;
        ObservableCollection<PhongTrong> lsPhongTrongs;
        ObservableCollection<CT_PhieuThue> lsPDaChons;
        List<PhongTrong> lsPhongCaches;
        private int maNV;

        public int MaNV { get => maNV; set => maNV = value; }

        public DatPhong()
        {
            InitializeComponent();
        }

        public DatPhong(int maNV) : this()
        {
            this.MaNV = maNV;
        }

        #region method
        private void getPhongTrongTheoNgayGio()
        {
            DateTime ngayBD = DateTime.Parse(dtpNgayBD.Text + " " + tpGioBD.Text);
            DateTime ngayKT = DateTime.Parse(dtpNgayKT.Text + " " + tpGioKT.Text);
            List<PhongTrong> lsTemp = PhongBUS.GetInstance().getPhongTrong(ngayBD, ngayKT);
            var ls = (from l in lsTemp
                          where !(from pdc in lsPDaChons select pdc.SoPhong).Contains(l.SoPhong)
                          select l).ToList();
            lsPhongTrongs = new ObservableCollection<PhongTrong>(ls);
            lvPhongTrong.ItemsSource = lsPhongTrongs;

        }

        private bool kiemTraDayDuThongTin()
        {
            if (string.IsNullOrWhiteSpace(txbHoTen.Text))
            {
                txbHoTen.Focus();
                new DialogCustoms("Nhập đầy đủ họ tên !", "Thông báo", DialogCustoms.OK).ShowDialog();
                return false;
            }
            //Kiểm tra textbox CCCD rỗng hoặc nhập kí tự chữ không
            if (string.IsNullOrWhiteSpace(txbCCCD.Text))
            {
                txbCCCD.Focus();
                new DialogCustoms("Nhập đầy đủ căn cước công dân !", "Thông báo", DialogCustoms.OK).ShowDialog();
                return false;
            }
            else
            {
                if( !long.TryParse(txbCCCD.Text, out long temp))
                {
                    txbCCCD.Focus();
                    new DialogCustoms("Nhập căn cước công dân đúng định dạng số !", "Thông báo", DialogCustoms.OK).ShowDialog();
                    return false;
                }
                if(txbCCCD.Text.Length > 12)
                {
                    txbCCCD.Focus();
                    new DialogCustoms("Nhập căn cước công dân không quá 12 ký tự !", "Thông báo", DialogCustoms.OK).ShowDialog();
                    return false;
                }
            }
            //Kiểm tra textbox SDT rỗng hoặc có nhập chữ không
            if (string.IsNullOrWhiteSpace(txbSDT.Text))
            {
                txbSDT.Focus();
                new DialogCustoms("Nhập đầy đủ số điện thoại !", "Thông báo", DialogCustoms.OK).ShowDialog();
                return false;
            }
            else
            {
                if (!long.TryParse(txbSDT.Text, out long temp))
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
            //Kiểm tra ô nhập địa chỉ
            if (string.IsNullOrWhiteSpace(txbDiaChi.Text))
            {
                txbDiaChi.Focus();
                new DialogCustoms("Nhập đầy đủ địa chỉ !", "Thông báo", DialogCustoms.OK).ShowDialog();
                return false;
            }
            //kiểm tra ô quốc tịch
            if (string.IsNullOrWhiteSpace(txbQuocTich.Text))
            {
                txbQuocTich.Focus();
                new DialogCustoms("Nhập đầy đủ quốc tịch !", "Thông báo", DialogCustoms.OK).ShowDialog();
                return false;
            }
            //Kiểm tra ô giới tính
            if (string.IsNullOrWhiteSpace(cbGioiTinh.Text))
            {
                new DialogCustoms("Nhập đầy đủ giới tính !", "Thông báo", DialogCustoms.OK).ShowDialog();
                return false;
            }
            //Kiểm tra xem đã có phòng nào được chọn chưa
            if(lsPDaChons.Count==0)
            {
                new DialogCustoms("Vui lòng chọn phòng trước khi lưu !", "Thông báo", DialogCustoms.OK).ShowDialog();
                return false;
            }    
            return true;
        }
        #endregion

        #region event
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Khởi tạo giá trị cho ngày, giờ Bắt đầu và Kết thúc là ngày giờ hiện tại
            dtpNgayBD.Text = new DateTime(2021, 10, 8).ToShortDateString();
            dtpNgayKT.Text = new DateTime(2021, 10, 8).ToShortDateString();
            tpGioBD.Text = DateTime.Now.ToShortTimeString();
            tpGioKT.Text = DateTime.Now.ToShortTimeString();

            //Khởi tạo sự kiện thay đổi ngày hoặc giờ
            dtpNgayBD.SelectedDateChanged += DT_SelectedDateChanged;
            dtpNgayKT.SelectedDateChanged += DT_SelectedDateChanged;
            tpGioBD.SelectedTimeChanged += tpGio_SelectedTimeChanged;
            tpGioKT.SelectedTimeChanged += tpGio_SelectedTimeChanged;

            //Lấy danh sách phòng trống có thể dặt từ DB lên dựa vào ngày giờ Bắt đầu, Kết thúc
            lsPDaChons = new ObservableCollection<CT_PhieuThue>();
            lsPhongCaches = new List<PhongTrong>();
            lvPhongDaChon.ItemsSource = lsPDaChons;
            getPhongTrongTheoNgayGio();

        }
        
        private void tpGio_SelectedTimeChanged(object sender, RoutedPropertyChangedEventArgs<DateTime?> e)
        {
            DateTime dtNBD;
            DateTime dtNKT;
            DateTime timeNBD;
            DateTime timeNKT;
            if (!DateTime.TryParse(dtpNgayBD.Text, out dtNBD))
            {
                new DialogCustoms("Nhập đúng định dạng ngày bắt đầu !", "Thông báo", DialogCustoms.OK).ShowDialog();
                return;
            }
            if (!DateTime.TryParse(dtpNgayKT.Text, out dtNKT))
            {
                new DialogCustoms("Nhập đúng định dạng ngày kết thúc !", "Thông báo", DialogCustoms.OK).ShowDialog();
                return;
            }
            //Nếu 2 ngày bằng nhau thì kiểm tra giờ xem giờ bắt đầu có lơn hơn giờ kết thúc không
            if(dtNBD == dtNKT)
            {
                if (!DateTime.TryParse(dtpNgayBD.Text + " " + tpGioBD.Text, out timeNBD))
                {
                    new DialogCustoms("Nhập đúng định dạng giờ bắt đầu !", "Thông báo", DialogCustoms.OK).ShowDialog();
                    return;
                }
                if (!DateTime.TryParse(dtpNgayKT.Text + " " + tpGioKT.Text, out timeNKT))
                {
                    new DialogCustoms("Nhập đúng định dạng giờ kết thúc !", "Thông báo", DialogCustoms.OK).ShowDialog();
                    return;
                }
                if(timeNBD > timeNKT)
                {
                    new DialogCustoms("Giờ bắt đầu không thế lớn hơn giờ kết thúc !", "Thông báo", DialogCustoms.OK).ShowDialog();
                    tpGioBD.Text = tpGioKT.Text;
                    return;
                }

            }

            getPhongTrongTheoNgayGio();       
        }

        private void DT_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            string ngayBD = dtpNgayBD.Text;
            string ngayKT = dtpNgayKT.Text;
            DateTime dtNBD;
            DateTime dtNKT;
            if ( !DateTime.TryParse(ngayBD,out dtNBD))
            {
                new DialogCustoms("Nhập đúng định dạng ngày bắt đầu !", "Thông báo", DialogCustoms.OK).ShowDialog();
                return;
            }

            if( !DateTime.TryParse(ngayKT,out dtNKT))
            {
                new DialogCustoms("Nhập đúng định dạng ngày kết thúc !", "Thông báo", DialogCustoms.OK).ShowDialog();
                return;
            }
            //nếu ngày bắt đầu lớn hơn ngày kết thúc thì phải báo lỗi ngay
            if(dtNBD > dtNKT)
            {
                new DialogCustoms("Ngày bắt đầu không thể lớn hơn ngày kết thúc !", "Thông báo", DialogCustoms.OK).ShowDialog();
                dtpNgayBD.Text = ngayKT;
                dtpNgayKT.Text = ngayKT;
                return;
            }
            getPhongTrongTheoNgayGio();
        }

        private void click_Huy(object sender, RoutedEventArgs e)
        {
            Button huy = sender as Button;
            this.Close();
        }

        private void click_Luu(object sender, RoutedEventArgs e)
        {
            //Khai báo biến cần thiết
            int maKhachHangThemMoi;
            string errorKhachHang;
            string errorPhieuThue;
            string errorCTPT;
            int dem = 0;
            bool checkThemKHThanhCong = false;
            bool themKhachHangMoi = false;

            if (kiemTraDayDuThongTin())
            {
                KhachHang kh = new KhachHang()
                {
                    CCCD = txbCCCD.Text,
                    TenKH = txbHoTen.Text,
                    DiaChi = txbDiaChi.Text,
                    GioiTinh = cbGioiTinh.Text,
                    QuocTich = txbQuocTich.Text,
                    SDT = txbSDT.Text
                };
                //B1: THêm khách hàng kiểm tra xem khách hàng đã tồn tại trong CSDL chưa dựa vào CCCD nếu có rồi thì không thêm nữa, còn chưa có thì thêm vào CSDL
                maKhachHangThemMoi = KhachHangBUS.GetInstance().kiemTraTonTaiKhachHang(kh.CCCD);
                if ( maKhachHangThemMoi == -1)
                {
                    checkThemKHThanhCong = KhachHangBUS.GetInstance().addKhachHang(kh, out errorKhachHang);
                    if (!checkThemKHThanhCong )
                    {
                        new DialogCustoms("Lỗi: Thêm Khách Hàng " + errorKhachHang, "Thông báo", DialogCustoms.OK).ShowDialog();
                        return;
                    }
                    maKhachHangThemMoi = kh.MaKH;
                    themKhachHangMoi = true;
                }
                else
                {
                    checkThemKHThanhCong = true;
                }
                //B2: Thêm phiếu thuê nếu thêm khách hàng thành công hoặc lấy ra được mã khách hàng đã tồn tại
                if (checkThemKHThanhCong)
                {
                    PhieuThue pt = new PhieuThue()
                    {
                        NgayLapPhieu = DateTime.Now,
                        MaKH = maKhachHangThemMoi,
                        MaNV = this.MaNV
                    };

                    if (PhieuThueBUS.GetInstance().addPhieuThue(pt, out errorPhieuThue))
                    {
                        //B3: Thêm CT phiếu thuê
                        foreach (CT_PhieuThue item in lsPDaChons)
                        {

                            item.MaPhieuThue = pt.MaPhieuThue;
                            item.TinhTrangThue = "Phòng đã đặt";
                            if (CT_PhieuThueBUS.GetInstance().addCTPhieuThue(item, out errorCTPT))
                            {
                                dem++;
                            }
                            else
                            {
                                new DialogCustoms("Lỗi: Thêm CTPT " + errorCTPT, "Thông báo", DialogCustoms.OK).ShowDialog();
                                break;
                            }
                        }
                    }
                    else
                    {
                        new DialogCustoms("Lỗi: Thêm Phiếu Thuê  " + errorPhieuThue, "Thông báo", DialogCustoms.OK).ShowDialog();
                    }

                }
                


                if (dem == lsPDaChons.Count && dem != 0)
                {
                    if (themKhachHangMoi)
                    {
                        new DialogCustoms("Thêm khách hàng mới và đặt phòng thành công !", "Thông báo", DialogCustoms.OK).ShowDialog();
                    }
                    else
                    {
                        new DialogCustoms("Khách hàng đã tồn tại đặt phòng thành công !", "Thông báo", DialogCustoms.OK).ShowDialog();
                    }
                    
                    if (luuPhieuDatPhong != null)
                    {
                        luuPhieuDatPhong();
                    }
                    this.Close();
                }
                else
                {
                    new DialogCustoms("Đặt phòng thất bại  !", "Thông báo", DialogCustoms.OK).ShowDialog();
                }
            }
            
        }

        private void click_ThemPhong(object sender, RoutedEventArgs e)
        {
            PhongTrong ephongTrong = (sender as Button).DataContext as PhongTrong;
            lsPhongCaches.Add(ephongTrong);
            lsPhongTrongs.Remove(ephongTrong);
            CT_PhieuThue phongDaChon = new CT_PhieuThue()
            {
                SoPhong = ephongTrong.SoPhong,
                SoNguoiO =1,
                NgayBD = DateTime.Parse(dtpNgayBD.Text + " " + tpGioBD.Text),
                NgayKT = DateTime.Parse(dtpNgayKT.Text + " " + tpGioKT.Text)
            };
            lsPDaChons.Add(phongDaChon);
        }

        private void click_Delete(object sender, RoutedEventArgs e)
        {
            CT_PhieuThue phongDaChon = (sender as Button).DataContext as CT_PhieuThue;
            foreach (PhongTrong pt in lsPhongCaches)
            {
                if (pt.SoPhong.Equals(phongDaChon.SoPhong))
                {
                    lsPhongTrongs.Add(pt);
                    lsPhongCaches.Remove(pt);
                    break;
                }
            }
            lsPDaChons.Remove(phongDaChon);
        }
        private void txbSoLuong_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox txb = sender as TextBox;
            CT_PhieuThue ctpt = (sender as TextBox).DataContext as CT_PhieuThue;
            int soNguoi = 1;
            if(!int.TryParse(txb.Text, out soNguoi))
            {
                new DialogCustoms("Lỗi: Nhập số người kiểu số nguyên!", "Thông báo", DialogCustoms.OK).ShowDialog();
                return;
            }
            ctpt.SoNguoiO = soNguoi;
        }


        #endregion

        private void txbSoLuong_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                TextBox txb = sender as TextBox;
                CT_PhieuThue ctpt = (sender as TextBox).DataContext as CT_PhieuThue;
                int soNguoi = 1;
                if (!int.TryParse(txb.Text, out soNguoi))
                {
                    new DialogCustoms("Lỗi: Nhập số người kiểu số nguyên!", "Thông báo", DialogCustoms.OK).ShowDialog();
                    return;
                }
                ctpt.SoNguoiO = soNguoi;
            }
            
        }
    }
}
