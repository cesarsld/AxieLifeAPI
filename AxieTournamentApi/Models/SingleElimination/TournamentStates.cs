using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AxieTournamentApi.Models
{
    public enum TournamentStates
    {
        Created,
        Accepting_Players,
        Ready_For_Seeding,
        Running,
        Paused,
        Over
    }
}
