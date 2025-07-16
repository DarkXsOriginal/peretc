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
            string[,] boards = new string[,]
            {
                {"1","2DTyn"},
                {"2","2DTyn3"},
                {"3","3DTyn"},
            };
            for (int i = 0; i < boards.GetLength(0); i++)
            {
                string[] board = new string[boards.GetLength(1)];
                for (int j = 0; j < board.GetLength(0); j++)
                {
                    board[j] = boards[i, j];
                }

               ListBoards.Items.Add(board);
            }
            
        }
    }
}
