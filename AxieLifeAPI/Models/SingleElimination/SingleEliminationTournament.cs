using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using AxieLifeAPI.Models.Notification;
using AxieLifeAPI.Models.User;

namespace AxieLifeAPI.Models.SingleElimination
{
    public class SingleEliminationTournament
    {
        private static readonly Random _rand = new Random();
        private const string VIRTUAL_PLAYER = "BYE";

        public string id;
        public string creatorAddress;

        public TournamentStates tourneyState;

        public int maxPlayers;
        public int secondsBetweenRounds;
        public int totalRoundNumber;
        public int boFormat;
        public int currentRoundTime;

        public List<ParticipantData> participantList;
        public List<MatchUp> matchUpList;
        public string winner;

        public List<List<MatchUp>> matchUpHistoryList;

        public SingleEliminationTournament(int seconds, string creator, int bo = 3, int max  = -1)
        {
            id = "";
            for (int i = 0; i < 40; i++)
                id += _rand.Next(16).ToString("X");

            creatorAddress = creator;
            participantList = new List<ParticipantData>();
            tourneyState = TournamentStates.Accepting_Players;

            secondsBetweenRounds = seconds;
            boFormat = bo;
            maxPlayers = max;
            totalRoundNumber = 0;
            currentRoundTime = 0;
            matchUpList = new List<MatchUp>();
            matchUpHistoryList = new List<List<MatchUp>>();
            winner = "";
        }

        public async Task<bool> AddPlayer(UserData user)
        {
            if (participantList.Exists(u => u.ethAddress == user.id))
                return false;
            if (maxPlayers == -1 || participantList.Count < maxPlayers)
            {
                participantList.Add(new ParticipantData(user));
                if (participantList.Count == maxPlayers)
                    tourneyState = TournamentStates.Ready_For_Seeding;
                await SaveDataToDb();
                return true;

            }
            else return false;
        }

        public async Task SaveDataToDb()
        {
            var collec = DatabaseConnection.GetDb().GetCollection<SingleEliminationTournament>("SingleEliminationTournaments");
            var doc = (await collec.FindAsync(d => d.id == id)).FirstOrDefault();
            if (doc != null)
            {
                await collec.FindOneAndReplaceAsync(d => d.id == id, this);
            }
            else
                await collec.InsertOneAsync(this);
        }
    }
}
