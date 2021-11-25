using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using DAL;

namespace GUI.View
{
    /// <summary>
    /// Interaction logic for DangNhap.xaml
    /// </summary>
    public partial class DangNhap : Window
    {
        public DangNhap()
        {
            InitializeComponent();
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = false;
        }


        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void click_DangNhap(object sender, RoutedEventArgs e)
        {
            string username = txbTenDangNhap.Text;
            string pass = txbMatKhau.Password;
            TaiKhoan taiKhoan = TaiKhoanBUS.GetInstance().kiemTraTKTonTaiKhong(username, pass);
            if (taiKhoan != null)
            {
                MainWindow main = new MainWindow(taiKhoan);
                main.Show();
                this.Close();
            }
            else
            {
                new DialogCustoms("Không tồn tại tài khoản mật khẩu  !", "Thông báo" , DialogCustoms.OK).ShowDialog();
            }
            
        }
    }
}
