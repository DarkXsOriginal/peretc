using peretc.AccountParser;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Permissions;
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
            GiveToTheList();
        }

        private void ListBoards_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        public void GiveToTheList()
        {
            
            BoardData[] boards=new PinterestAccountScraper("marcykatya").GetBoardList().Result;
            foreach (BoardData board in boards)
            {
                string boadstr = $"{board.Name}";
                lsb.Items.Add(board);
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
