using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AxieLifeAPI.Models.User
{
    public class BaseUserData
    {
        public string id;
        public ulong discordId;
    }

    public class UserData
    {
        public string id;
        public ulong discordId;

        public List<CompletedTournaments> ongoingTournaments;
        public List<CompletedTournaments> completedTournaments;

        public List<CompletedTournaments> ongoingCreatedTournaments;
        public List<CompletedTournaments> completedCreatedTournaments;

        public UserData(BaseUserData data)
        {
            id = data.id;
            discordId = data.discordId;

            completedTournaments = new List<CompletedTournaments>();
            ongoingTournaments = new List<CompletedTournaments>();
        }

        public bool IsDiscordIdValid() => discordId != 0;
    }

    public class CompletedTournaments
    {
        public string id;
        public int ranking;
    }
}
