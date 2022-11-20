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

namespace TicTacToe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MarkType[] results;
        private bool playerTurn;
        private bool gameEnd;

        public MainWindow()
        {
            InitializeComponent();
            NewGame();
        }

        private void NewGame() // starts a new game with a blank slate (grid)
        {
            results = new MarkType[9]; // 3x3 cells

            for (var i = 0; i < results.Length; i++)
            {
                results[i] = MarkType.Free;
            }

            playerTurn = true; // true is player1

            // iterates through every button on the grid
            Container.Children.Cast<Button>().ToList().ForEach(button =>
            {
                button.Content = string.Empty;
                button.Background = Brushes.White;
                button.Foreground = Brushes.Blue;
            });

            gameEnd = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (gameEnd) // checks if the game has ended
            {
                switch (playerTurn) // shows winning persons
                {
                    case true: MessageBox.Show("Player won"); break;
                    case false: MessageBox.Show("AI won"); break;
                }

                NewGame();
                return;
            }

            var button = (Button)sender; // explicit cast to button
            
            int col = Grid.GetColumn(button);
            int row = Grid.GetRow(button);

            var index = col + (row * 3);

            if (results[index] != MarkType.Free) return;

            // setting the content of the buttons once clicked
            results[index] = playerTurn ? MarkType.Cross : MarkType.Nought; // cell value based on player turn
            button.Content = playerTurn ? "X" : "O";

            button.Foreground = playerTurn ? Brushes.Green : Brushes.Red;
            playerTurn = playerTurn ? false : true; // when clicked switches turn

            //CheckForWinner();
        }

        private void CheckForWinner()
        {
            throw new NotImplementedException();
        }
    }
}
