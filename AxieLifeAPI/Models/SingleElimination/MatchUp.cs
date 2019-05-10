using System;
using System.Collections.Generic;
using System.Text;

namespace AxieTournamentApi.Models.SingleElimination
{
    public class MatchUp
    {
        public string player1;
        public int player1Score;
        public string player2;
        public int player2Score;

        public string winner;
        public string loser;

        public List<ChallengeData> matchList;

        public MatchUp()
        { }
        public MatchUp(string a, string b)
        {
            player1 = a;
            player2 = b;
            winner = "";
            loser = "";
            player1Score = 0;
            player2Score = 0;
            matchList = new List<ChallengeData>();
        }
    }
}
