using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Diagnostics;
using System;
using GUI.UserControls;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Media;
using System.Threading;
using MaterialDesignThemes.Wpf;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using System.IO;
using DAL;
using BUS;

namespace GUI.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region uc_view
        private uc_Home Home;
        private uc_Phong Phong_UC;
        private uc_PhieuThue ThuePhong_UC;
        private uc_NhanVien NhanVien_UC;
        private uc_QuanLyPhong QuanLyPhong_UC;
        private uc_QuanLyKhachHang QuanLyKhachHang_UC;
        private uc_QuanLyLoaiPhong QuanLyLoaiPhong_UC;
        private uc_QuanLyDichVu QuanLyDichVu_UC;
        private uc_QuanLyTienNghi QuanLyTienNghi_UC;
        private uc_QuanLyChiTietTienNghi QuanLyChiTietTienNghi_UC;
        private uc_QuanLyLoaiDichVu QuanLyLoaiDichVu_UC;
        private uc_HoaDon HoaDon_UC;
        private uc_ThongKe ThongKe_UC;
        #endregion
        #region Khai báo biến
        public List<ItemMenuMainWindow> listMenu { get; set; }
        private string title_Main;
        private int minHeight_ucControlbar;
        private int maNV;
        private int capDoQuyen;

        public string Title_Main
        {
            get => title_Main;
            set
            {
                title_Main = value;
                OnPropertyChanged("Title_Main");
            }
        }
        public int MinHeight_ucControlbar
        {
            get => minHeight_ucControlbar;
            set
            {
                minHeight_ucControlbar = value;
                OnPropertyChanged("MinHeight_ucControlbar");
                if (value == 1)
                {
                    boGoc.Rect = new Rect(0, 0, SystemParameters.MaximizedPrimaryScreenWidth, SystemParameters.MaximizedPrimaryScreenHeight);
                }
                else
                {
                    boGoc.Rect = new Rect(0, 0, 1300, 700);
                }
            }
        }
        public int CapDoQuyen { get => capDoQuyen; set => capDoQuyen = value; }
        public int MaNV { get => maNV; set => maNV = value; }
        #endregion

        TaiKhoan taiKhoan;
        public MainWindow()
        {
            InitializeComponent();

            
        }

        public MainWindow(TaiKhoan tk):this()
        {
            this.taiKhoan = tk;
            this.MaNV = tk.MaNV;
            this.CapDoQuyen = tk.CapDoQuyen;
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            this.MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
        }
        #region method
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string newName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(newName));
            }
        }
        private void initListViewMenu()
        {
            listMenu = new List<ItemMenuMainWindow>();
            //Khoi tao Menu
            if (CapDoQuyen == 1)
            {
                listMenu.Add(new ItemMenuMainWindow() { name = "Trang Chủ", foreColor = "Gray", kind_Icon = "Home" });
                listMenu.Add(new ItemMenuMainWindow() { name = "Phòng", foreColor = "#FFF08033", kind_Icon = "HomeCity" });
                listMenu.Add(new ItemMenuMainWindow() { name = "Đặt Phòng", foreColor = "Green", kind_Icon = "BookAccount" });
                listMenu.Add(new ItemMenuMainWindow() { name = "Hóa đơn", foreColor = "#FFD41515", kind_Icon = "Receipt" });
                listMenu.Add(new ItemMenuMainWindow() { name = "QL nhân Viên", foreColor = "#FFD41515", kind_Icon = "Account" });
                listMenu.Add(new ItemMenuMainWindow() { name = "QL khách hàng", foreColor = "#FFD41515", kind_Icon = "Account" });
                listMenu.Add(new ItemMenuMainWindow() { name = "QL phòng", foreColor = "#FFE6A701", kind_Icon = "StarCircle" });
                listMenu.Add(new ItemMenuMainWindow() { name = "QL loại phòng", foreColor = "#FFE6A701", kind_Icon = "StarCircle" });
                listMenu.Add(new ItemMenuMainWindow() { name = "QL dịch vụ", foreColor = "Blue", kind_Icon = "FaceAgent" });
                listMenu.Add(new ItemMenuMainWindow() { name = "QL loại dịch vụ", foreColor = "Blue", kind_Icon = "FaceAgent" });
                listMenu.Add(new ItemMenuMainWindow() { name = "QL tiện nghi", foreColor = "#FFF08033", kind_Icon = "Fridge" });
                listMenu.Add(new ItemMenuMainWindow() { name = "QL chi tiết tiện nghi", foreColor = "#FFF08033", kind_Icon = "Fridge" });
                listMenu.Add(new ItemMenuMainWindow() { name = "Thống kê", foreColor = "#FF0069C1", kind_Icon = "ChartAreaspline" });
            }
            else if(CapDoQuyen == 2)
            {
                listMenu.Add(new ItemMenuMainWindow() { name = "Trang Chủ", foreColor = "Gray", kind_Icon = "Home" });
                listMenu.Add(new ItemMenuMainWindow() { name = "Phòng", foreColor = "#FFF08033", kind_Icon = "HomeCity" });
                listMenu.Add(new ItemMenuMainWindow() { name = "Đặt Phòng", foreColor = "Green", kind_Icon = "BookAccount" });
                listMenu.Add(new ItemMenuMainWindow() { name = "Hóa đơn", foreColor = "#FFD41515", kind_Icon = "Receipt" });
            }

            lisviewMenu.ItemsSource = listMenu;
            lisviewMenu.SelectedValuePath = "name";
            Title_Main = "Trang Chủ";
        }
        #endregion

        #region event

        private void load_Windows(object sender, RoutedEventArgs e)
        {
            this.DataContext = this;
            Home = new uc_Home();
            contenDisplayMain.Content = Home;
            txbHoTenNV.Text = taiKhoan.NhanVien.HoTen;
            if (string.IsNullOrEmpty(taiKhoan.avatar))
            {
                Uri uri = new Uri("pack://application:,,,/Res/mountains.jpg");
                ImageBrush imageBrush = new ImageBrush(new BitmapImage(uri));
                imgAvatar.Fill = imageBrush;
            }
            else
            {
                string staupPath = Environment.CurrentDirectory + "\\Res";
                string filePath = Path.Combine(staupPath, taiKhoan.avatar);
                if (File.Exists(filePath))
                {
                    ImageBrush imageBrush = new ImageBrush(new BitmapImage(new Uri(filePath)));
                    imgAvatar.Fill = imageBrush;

                }
                else
                {
                    new DialogCustoms("Không tồn tại file ảnh của nhân viên " + taiKhoan.NhanVien.HoTen, "Thông báo", DialogCustoms.OK).ShowDialog();
                }
            }
            initListViewMenu();




        }

        private void lisviewMenu_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (lisviewMenu.SelectedValue != null)
            {
                switch (lisviewMenu.SelectedIndex)
                {
                    case 0:
                        //Đang là Home rồi thì không set nữa
                        if (Title_Main.Equals(lisviewMenu.SelectedValue.ToString()))
                        {
                            break;
                        }
                        contenDisplayMain.Content = Home;
                        break;
                    case 1:
                        if (Phong_UC == null)
                        {
                            Phong_UC = new uc_Phong(MaNV);
                        }
                        contenDisplayMain.Content = Phong_UC;
                        break;
                    case 2:
                        if (ThuePhong_UC == null)
                        {
                            ThuePhong_UC = new uc_PhieuThue(MaNV);
                        }
                        contenDisplayMain.Content = ThuePhong_UC;
                        break;
                    case 3:
                        if (HoaDon_UC == null)
                        {
                            HoaDon_UC = new uc_HoaDon();
                        }
                        contenDisplayMain.Content = HoaDon_UC;
                        break;
                    case 4:
                        if (NhanVien_UC == null)
                        {
                            NhanVien_UC = new uc_NhanVien();
                        }
                        contenDisplayMain.Content = NhanVien_UC;
                        break;
                    case 5:
                        if (QuanLyKhachHang_UC == null)
                        {
                            QuanLyKhachHang_UC = new uc_QuanLyKhachHang();
                        }
                        contenDisplayMain.Content = QuanLyKhachHang_UC;
                        break;
                    case 6:
                        if(QuanLyPhong_UC == null)
                        {
                            QuanLyPhong_UC = new uc_QuanLyPhong();
                        }
                        contenDisplayMain.Content = QuanLyPhong_UC;
                        break;
                    case 7:
                        if(QuanLyLoaiPhong_UC == null)
                        {
                            QuanLyLoaiPhong_UC = new uc_QuanLyLoaiPhong();
                        }
                        contenDisplayMain.Content = QuanLyLoaiPhong_UC;
                        break;
                    case 8:
                        if (QuanLyDichVu_UC == null)
                        {
                            QuanLyDichVu_UC = new uc_QuanLyDichVu();
                        }
                        contenDisplayMain.Content = QuanLyDichVu_UC;
                        break;
                    case 9:
                        if (QuanLyLoaiDichVu_UC == null)
                        {
                            QuanLyLoaiDichVu_UC = new uc_QuanLyLoaiDichVu();
                        }
                        contenDisplayMain.Content = QuanLyLoaiDichVu_UC;
                        break;
                    case 10:
                        if (QuanLyTienNghi_UC == null)
                        {
                            QuanLyTienNghi_UC = new uc_QuanLyTienNghi();
                        }
                        contenDisplayMain.Content = QuanLyTienNghi_UC;
                        break;
                    case 11:
                        if (QuanLyChiTietTienNghi_UC == null)
                        {
                            QuanLyChiTietTienNghi_UC = new uc_QuanLyChiTietTienNghi();
                        }
                        contenDisplayMain.Content = QuanLyChiTietTienNghi_UC;
                        break;
                    case 12:
                        if (ThongKe_UC == null)
                        {
                            ThongKe_UC = new uc_ThongKe();
                        }
                        contenDisplayMain.Content = ThongKe_UC;
                        break;
                }
                Title_Main = lisviewMenu.SelectedValue.ToString();
                //Tự động hóa việc click Button toggleBtnMenu_Close
                btnCloseLVMenu.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
        }
        #endregion

        private void click_ThayDoiAnh(object sender, RoutedEventArgs e)
        {
            //string[] filePathTonTai;
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Pictures files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png)|*.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            openFile.FilterIndex = 1;
            openFile.RestoreDirectory = true;
            if (openFile.ShowDialog()  == true)
            {
                //xử lý đổi tên file truyền vào
                string [] arr = openFile.FileName.Split('\\');
                string[] arrFileName = arr[arr.Length - 1].Split('.');
                string newNameFile = "NV" + maNV  +"-"+ DateTime.Now.Ticks.ToString() + "." + arrFileName[arrFileName.Length - 1];

                try
                {
                    string sourceFile = openFile.FileName;
                    string targetPath = Environment.CurrentDirectory+"\\Res";
                    //Combine file và đường dẫn
                    string destFile = Path.Combine(targetPath, newNameFile);

                    //Copy file từ file nguồn đến file đích
                    File.Copy(sourceFile, destFile, true);

                    //gán ngược lại giao diện
                    Uri uri = new Uri(destFile);
                    ImageBrush imageBrush = new ImageBrush(new BitmapImage(uri));
                    imgAvatar.Fill = imageBrush;
                    //Thêm đường dẫn vào DB
                    string error;
                    if(!TaiKhoanBUS.GetInstance().capNhatAvatar(taiKhoan.username,newNameFile,out error))
                    {
                        new DialogCustoms("Thay đổi ảnh đại diện thất bại !\n Lỗi: "+error, "Thông báo", DialogCustoms.OK).ShowDialog();
                    }
                    else
                    {
                        
                        new DialogCustoms("Thay đổi ảnh đại diện thành công !", "Thông báo", DialogCustoms.OK).ShowDialog();
                    }
 
                }
                catch (Exception ex)
                {
                    new DialogCustoms("Lỗi: "+ ex.Message, "Thông báo", DialogCustoms.OK).ShowDialog();
                }

            }
            

        }

        private void btnDangXuat_Click(object sender, RoutedEventArgs e)
        {
            DialogCustoms dialog = new DialogCustoms("Bạn có muốn đăng xuất ?", "Thông báo", DialogCustoms.YesNo);
            if(dialog.ShowDialog() == true)
            {
                new DangNhap().Show();
                this.Close();
            }
        }
    }


    public class ItemMenuMainWindow
    {
        public string name { get; set; }
        public string foreColor { get; set; }
        public string kind_Icon { get; set; }

        public ItemMenuMainWindow() { }

    }
}
