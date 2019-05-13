using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AxieTournamentApi.IO;

namespace AxieTournamentApi.Models.Challonge
{
    public class ChallengePostRequest
    {
        public string api_key;
        public TournamentJson tournament;

        public ChallengePostRequest()
        {
            api_key = IOGetter.GetChallongeKey();
            //tournament = new TournamentJson();
        }
    }
}
