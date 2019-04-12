using System;
using System.Collections.Generic;
using System.Text;
using AxieLifeAPI.Models.User;

namespace AxieLifeAPI.Models.SingleElimination
{
    public class ParticipantData
    {
        public string ethAddress;
        public ulong discordId;
        public bool stillCompeting;

        public bool IsDiscordIdValid() => discordId != 0;

        public ParticipantData(UserData user)
        {
            ethAddress = user.id;
            discordId = user.discordId;
            stillCompeting = true;
        }
    }
}
