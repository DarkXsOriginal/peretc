using System;
using System.Collections;
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

namespace Interface
{
    /// <summary>
    /// Логика взаимодействия для DownloadingWindow.xaml
    /// </summary>
    public partial class DownloadingWindow : Page
    {
        
        public DownloadingWindow()
        {
            InitializeComponent();
        }

        private void ListBoards_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GiveToTheList();
        }
            public void GiveToTheList()
        {
            ArrayList boards = new ArrayList();
            boards.Add("1Dock");
            boards.Add("2Dock");
            boards.Add("3Dock");
            ListBoards.Items.Add(boards);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
