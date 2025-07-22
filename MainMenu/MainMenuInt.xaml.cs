using Interface;
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

namespace MainMenu
{
    /// <summary>
    /// Логика взаимодействия для MainMenuInt.xaml
    /// </summary>
    public partial class MainMenuInt : Page
    {
        public MainMenuInt()
        {
            InitializeComponent();
        }

        private void BtnDownloading_Click(object sender, RoutedEventArgs e)
        {
            Window window = new Window();
            window.Content = new DownloadingWindow();
            window.Show();
        }
    }
}
