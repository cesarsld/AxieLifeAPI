using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AxieLifeAPI.Models.SingleElimination
{
    public class ResolveData
    {
        public int matchIndex;
        public string winner;
        public int scoreWinner;
        public int scoreLoser;
        public List<int> matchupList;
    }
}
