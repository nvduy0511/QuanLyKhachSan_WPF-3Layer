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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GUI.UserControls
{
    /// <summary>
    /// Interaction logic for uc_Controlbar.xaml
    /// </summary>
    public partial class uc_Controlbar : UserControl
    {
        public uc_Controlbar()
        {
            InitializeComponent();
        }
        #region event
        private void Button_Close(object sender, RoutedEventArgs e)
        {
            Button btn_close = sender as Button;
            Window mainwindows = Window.GetWindow(btn_close);
            mainwindows.Close();
        }

        private void Button_Maximize(object sender, RoutedEventArgs e)
        {
            Button btn_close = sender as Button;
            Window mainwindows = Window.GetWindow(btn_close);
            if (mainwindows != null)
            {
                if (mainwindows.WindowState != WindowState.Maximized)
                {
                    mainwindows.WindowState = WindowState.Maximized;
                    btn_maximize.ToolTip = "Normal";
                    controlbar.MinHeight = 1;
                }
                else
                {
                    mainwindows.WindowState = WindowState.Normal;
                    btn_maximize.ToolTip = "Maximize";
                    controlbar.MinHeight = 0;
                }
            }
        }
        private void Button_Minimize(object sender, RoutedEventArgs e)
        {
            Button btn_close = sender as Button;
            Window mainwindows = Window.GetWindow(btn_close);
            mainwindows.WindowState = WindowState.Minimized;
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Grid grid = sender as Grid;
            Window mainwindows = Window.GetWindow(grid);
            mainwindows.DragMove();
        }
        #endregion
    }
}
