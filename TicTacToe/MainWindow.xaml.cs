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
        private string[,] board = new string[3, 3]; // 2D array needed to apply MiniMax Algorithm
        private List<int> listIndex;
        Dictionary<string, int> scores;
        private bool playerTurn;
        private bool gameEnd;
        private bool MMGameEnd;
        Pair bestMove = new();

        public MainWindow()
        {
            listIndex = new List<int>();
            scores = new();
            scores.Add("X", 1);
            scores.Add("O", -1);
            scores.Add("Tie", 0);
            scores.Add("", 0);

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

            playerTurn = false; // true is player1

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

                bestMove = new Pair();
                
                // Initialize the game board to be empty
                for (int row = 0; row < 3; row++)
                {
                    for (int col = 0; col < 3; col++)
                    {
                        board[row, col] = "";
                    }
                }

            });

            MMGameEnd = false;
            gameEnd = false;
            AiBestMove(); 
        }

        private void AiMove() // not really an AI
        {
            bool available = true;

            CheckForWinner();
            if (gameEnd == true) return;

            while (available)
            {
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

        private void AiBestMove() // fixed
        {
            var bestScore = int.MinValue;
            Pair pair = new(); // Best Move

            CheckForWinner();
            if (gameEnd == true) return;

            for (int r = 0; r < 3; r++)
            {
                for (int c = 0; c < 3; c++)
                {
                    if (board[r, c] == "")
                    {
                        board[r, c] = "X";
                        var score = MiniMax(false);
                        board[r, c] = "";

                        if (score > bestScore)
                        {
                            bestScore = score;
                            pair.Row = r;
                            pair.Col = c;
                        }
                    }
                }
            }

            var index = pair.Col + (pair.Row * 3);

            if (IsAvailable(index))
            {
                board[pair.Row, pair.Col] = "X";
                results[index] = playerTurn ? MarkType.Cross : MarkType.Nought; // cell value based on player turn
                listIndex.Add(index);

                Button computerButton = (Button)Container.Children[pair.Row * 3 + pair.Col];
                computerButton.Foreground = Brushes.Red;
                computerButton.Content = "X";

                playerTurn = true;
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

                if (listIndex.Count != 9)
                {
                    //AiMove();
                    AiBestMove();
                    //FindBestMove();
                }
            }
            
            CheckForWinner();
        }

        private void CheckForWinner()
        {
            CheckHorizontalWins();
            CheckVerticalWins();
            CheckDiagonalWins();

            if (gameEnd == true) return;

            if (!results.Any(item => item == MarkType.Free))
            {
                gameEnd = true;
                MessageBox.Show("Draw");
                NewGame();
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

        // MiniMax functions =========================================================================================================

        private int MiniMax(bool isMaximisingPlayer)
        {
            int score;
            string result = MMCheckBoard();

            if (result != null)
            {
                return scores[result];
            }

            if (isMaximisingPlayer)
            {
                int bestScore = int.MinValue;
                for (int r = 0; r < 3; r++)
                {
                    for (int c = 0; c < 3; c++)
                    {
                        if(MMIsAvailable(r, c))
                        { 
                            board[r, c] = "X";
                            score = MiniMax(!isMaximisingPlayer);
                            board[r, c] = "";
                            bestScore = Math.Max(score, bestScore);
                        }
                    }
                }
                return bestScore;
            }
            else
            {
                int bestScore = int.MaxValue;
                for (int r = 0; r < 3; r++)
                {
                    for (int c = 0; c < 3; c++)
                    {
                        if (MMIsAvailable(r, c))
                        {
                            board[r, c] = "O";
                            score = MiniMax(!isMaximisingPlayer);
                            board[r, c] = "";
                            bestScore = Math.Min(score, bestScore);
                        }
                    }
                }
                return bestScore;
            }
        }
       
        private bool MMIsAvailable(int r, int c)
        {
            if (board[r, c] == "") return true;
            return false;
        }

        private string MMCheckBoard()
        {
            string winner = null;
            // check rows
            for (int row = 0; row < 3; row++)
            {
                if (board[row, 0] == board[row, 1] && board[row, 1] == board[row, 2] && (board[row, 0] == "X" || board[row, 0] == "O"))
                {
                    winner = board[row, 0];
                }
            }

            // check columns
            for (int col = 0; col < 3; col++)
            {
                if (board[0, col] == board[1, col] && board[1, col] == board[2, col] && (board[col, 0] == "X" || board[col, 0] == "O"))
                {
                    winner = board[0, col];
                }
            }

            // check diagonals
            if (board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2] && (board[0, 0] == "X" || board[0, 0] == "O"))
            {
                winner = board[0, 0];
            }

            if (board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0] && (board[0, 2] == "X" || board[0, 2] == "O"))
            {
                winner = board[0, 2];
            }

            if (winner == null && MMCheckAvailableSpots() == 0) return "Tie";
            return winner;
        }

        private int MMCheckAvailableSpots()
        {
            int val = 0;

            for (int r = 0; r < 3; r++)
            {
                for (int c = 0; c < 3; c++)
                {
                    if (board[r, c] == "") val++;
                }
            }

            return val;
        }
    }

}