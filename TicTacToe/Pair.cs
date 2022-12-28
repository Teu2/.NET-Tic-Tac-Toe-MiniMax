using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    internal class Pair
    {
        private int row;
        private int col;
        private int score;

        public Pair()
        {
            row = Row;
            col = Col;
            score = MMScore;
        }

        public int Row { get; set; }
        public int Col { get; set; }
        public int MMScore { get; set; }
    }
}
