using System;
using System.Collections.Generic;
using System.Text;
using AxieTournamentApi.Models.User;

namespace AxieTournamentApi.Models.SingleElimination
{
    public class ParticipantData
    {
        public string ethAddress;
        public ulong discordId;
        public bool stillCompeting;
        public int challongeId;

        public bool IsDiscordIdValid() => discordId != 0;

        public ParticipantData(UserData user)
        {
            ethAddress = user.id;
            discordId = user.discordId;
            stillCompeting = true;
            challongeId = 0;
        }
    }
}
