using System;
using System.Collections.Generic;
using System.Text;

namespace AxieLifeAPI.Models.SingleElimination
{
    public class MatchUp
    {
        public string player1;
        public string player2;

        public string winner;

        public MatchUp()
        { }
        public MatchUp(string a, string b)
        {
            player1 = a;
            player2 = b;
            winner = "";
        }
    }
}
