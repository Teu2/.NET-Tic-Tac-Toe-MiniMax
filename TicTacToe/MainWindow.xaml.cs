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
        private string[,] board = new string[3, 3]; // minimax algo
        private List<int> listIndex;
        private bool playerTurn;
        private bool gameEnd;

        public MainWindow()
        {
            // Initialize the game board to be empty
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    board[row, col] = "";
                }
            }

            listIndex = new List<int>();

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
                button.Foreground = Brushes.Red;

                // reset lines
                topLine.Visibility = Visibility.Hidden;
                midLine.Visibility = Visibility.Hidden;
                botLine.Visibility = Visibility.Hidden;

                leftColLine.Visibility = Visibility.Hidden;
                midColLine.Visibility = Visibility.Hidden;
                rightColLine.Visibility = Visibility.Hidden;

                diagLine1.Visibility = Visibility.Hidden;
                diagLine2.Visibility = Visibility.Hidden;

                listIndex.Clear(); // clear index

            });

            gameEnd = false;
        }

        private void AiMove(Button button) // easy opponent
        {
            bool available = true;

            while (available)
            {
                CheckForWinner();

                Random rnd = new Random();
                int row = rnd.Next(0, 3);
                int col = rnd.Next(0, 3);

                var index = col + (row * 3);

                if (IsAvailable(index))
                {
                    results[index] = playerTurn ? MarkType.Cross : MarkType.Nought; // cell value based on player turn
                    listIndex.Add(index);

                    Button computerButton = (Button)Container.Children[row * 3 + col];
                    computerButton.Foreground = Brushes.Red;
                    computerButton.Content = "X";

                    playerTurn = true; 
                    available = false;
                }
            }
        }

        private bool IsAvailable(int index)
        {
            if (!listIndex.Contains(index)) return true;
            return false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (gameEnd) // checks if the game has ended
            {
                NewGame();
                return;
            }

            var button = (Button)sender; // explicit cast to button
            
            int col = Grid.GetColumn(button);
            int row = Grid.GetRow(button);

            var index = col + (row * 3);

            if (results[index] != MarkType.Free) return;

            
            if (playerTurn)
            {
                results[index] = playerTurn ? MarkType.Cross : MarkType.Nought; // cell value based on player turn
                listIndex.Add(index);
                board[row, col] = "O";
                button.Content = "O";
                button.Foreground = Brushes.Blue;
                playerTurn = false; // when clicked switches turn
                
                AiMove(button);
            }

            CheckForWinner();
        }

        private void CheckForWinner()
        {
            CheckHorizontalWins();
            CheckVerticalWins();
            CheckDiagonalWins();

            if (!results.Any(item => item == MarkType.Free))
            {
                gameEnd = true;
                IterateThroughGrid(); // checks for no winner
            }
        }

        private void AlertWinner()
        {
            switch (playerTurn) // shows winning persons
            {
                case false: MessageBox.Show("Player won"); break;
                case true: MessageBox.Show("AI won"); break;
            }
        }

        private void IterateThroughGrid()
        {
            Container.Children.Cast<Button>().ToList().ForEach(button =>
            {
                button.Background = Brushes.Gray;
            });
        }

        private void CheckDiagonalWins()
        {
            var diag1 = ((results[0] & results[4] & results[8]) == results[0]);
            var diag2 = ((results[2] & results[4] & results[6]) == results[2]);
            
            if (results[0] != MarkType.Free && diag1) // checks diagonal wins
            {
                gameEnd = true;
                diagLine2.Visibility = Visibility.Visible;
                AlertWinner();
            }

            if (results[2] != MarkType.Free && diag2) // checks diagonal wins
            {
                gameEnd = true;
                diagLine1.Visibility = Visibility.Visible;
                AlertWinner();
            }
        }

        private void CheckHorizontalWins()
        {
            var Row1 = ((results[0] & results[1] & results[2]) == results[0]);
            var Row2 = ((results[3] & results[4] & results[5]) == results[3]);
            var Row3 = ((results[6] & results[7] & results[8]) == results[6]);

            if (results[0] != MarkType.Free && Row1) // checks horizontal wins
            {
                gameEnd = true;
                topLine.Visibility = Visibility.Visible;
                AlertWinner();
            }

            if (results[3] != MarkType.Free && Row2) // checks horizontal wins
            {
                gameEnd = true;
                midLine.Visibility = Visibility.Visible;
                AlertWinner();
            }

            if (results[6] != MarkType.Free && Row3) // checks horizontal wins
            {
                gameEnd = true;
                botLine.Visibility = Visibility.Visible;
                AlertWinner();
            }
        }

        private void CheckVerticalWins()
        {
            var col1 = ((results[0] & results[3] & results[6]) == results[0]);
            var col2 = ((results[1] & results[4] & results[7]) == results[1]);
            var col3 = ((results[2] & results[5] & results[8]) == results[2]);

            if (results[0] != MarkType.Free && col1) // checks vertical wins
            {
                gameEnd = true;
                leftColLine.Visibility = Visibility.Visible;
                AlertWinner();
            }

            if (results[1] != MarkType.Free && col2) // checks vertical wins
            {
                gameEnd = true;
                midColLine.Visibility = Visibility.Visible;
                AlertWinner();
            }

            if (results[2] != MarkType.Free && col3) // checks vertical wins
            {
                gameEnd = true;
                rightColLine.Visibility = Visibility.Visible;
                AlertWinner();
            }
        }
    }
}
