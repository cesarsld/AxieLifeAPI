using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AxieLifeAPI.Models
{
    public enum TournamentStates
    {
        Created,
        Accepting_Players,
        Ready_For_Seeding,
        Running,
        Ready_For_Updating,
        Paused,
        Over
    }
}
