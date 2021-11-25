using DAL;
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

namespace GUI.View
{
    /// <summary>
    /// Interaction logic for Them_SuaTienNghi.xaml
    /// </summary>
    public partial class Them_SuaTienNghi : Window
    {
        public delegate void truyenData(TienNghi tienNghi);
        public delegate void suaData(TienNghi tienNghi);


        public truyenData truyenTN;
        public suaData suaTN;
        private int maTN;
        public Them_SuaTienNghi()
        {
            InitializeComponent();
        }
        public Them_SuaTienNghi(TienNghi tn) : this()
        {
            txtTenTN.Text = tn.TenTN;
            txbTitle.Text = "Sửa thông tin " + tn.MaTN;
            maTN = tn.MaTN;
        }

        private void btnThem_Click(object sender, RoutedEventArgs e)
        {
            if (!KiemTra())
            {
                return;
            }
            else
            {
                TienNghi tienNghi = new TienNghi()
                {
                    MaTN = maTN,
                    TenTN = txtTenTN.Text,
                };
                if (truyenTN != null)
                {
                    truyenTN(tienNghi);
                }
            }
            Window wd = Window.GetWindow(sender as Button);
            wd.Close();
        }

        private void btnCapNhat_Click(object sender, RoutedEventArgs e)
        {
            if (!KiemTra())
            {
                return;
            }
            else
            {
                TienNghi tienNghi = new TienNghi()
                {
                    MaTN = maTN,
                    TenTN = txtTenTN.Text,
                };
                if (suaTN != null)
                {
                    suaTN(tienNghi);
                }
            }
            Window wd = Window.GetWindow(sender as Button);
            wd.Close();
        }

        private void btnHuy_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private bool KiemTra()
        {
            if (string.IsNullOrWhiteSpace(txtTenTN.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy dủ thông tin");
                return false;
            }
            else
            {
                int so;
                if (int.TryParse(txtTenTN.Text, out so))
                {
                    MessageBox.Show("Vui lòng nhập đúng dữ liệu");
                    return false;
                }
            }
            return true;
        }
    }
}
