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

namespace GuiClient
{
    /// <summary>
    /// Interaction logic for BannedWindow.xaml
    /// </summary>
    public partial class BannedWindow : Window
    {
        public BannedWindow()
        {
            InitializeComponent();
        }

        private void exit_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.Close();
            MainWindow mainwindow = new MainWindow();
            mainwindow.Show();
        }
    }
}
