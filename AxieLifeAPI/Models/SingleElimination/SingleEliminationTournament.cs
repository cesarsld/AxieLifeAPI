using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using AxieTournamentApi.Models.Notification;
using AxieTournamentApi.Models.User;

namespace AxieTournamentApi.Models.SingleElimination
{
    public class SingleEliminationTournament
    {
        private static readonly Random _rand = new Random();
        private const string VIRTUAL_PLAYER = "BYE";
        private const string UNRESOLVED = "";

        public string id;
        public int challongeId;
        public string creatorAddress;
        public string creatorName;

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

        public List<int> indexOfUnresolvedMatches;

        public SingleEliminationTournament(int seconds, string creator, int bo, int max, string name)
        {
            id = "";
            for (int i = 0; i < 15; i++)
                id += _rand.Next(16).ToString("X");
            id = id.ToLower();
            creatorAddress = creator;
            creatorName = name;
            participantList = new List<ParticipantData>();
            tourneyState = TournamentStates.Accepting_Players;

            secondsBetweenRounds = seconds;
            boFormat = bo;
            maxPlayers = max;
            totalRoundNumber = 0;
            currentRoundTime = 0;
            matchUpList = new List<MatchUp>();
            matchUpHistoryList = new List<List<MatchUp>>();
            indexOfUnresolvedMatches = new List<int>();
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

        public async Task<bool> ResolveMatchUp(int matchIndex, string _winner, int scoreWinner = 0, int scoreLoser = 0, List<int> matchList = null)
        {
            var matchup = matchUpList[matchIndex];
            matchup.winner = _winner;
            if (matchup.player1 == matchup.winner)
            {
                matchup.player1Score = scoreWinner;
                matchup.player2Score = scoreLoser;
                matchup.loser = matchup.player2;
            }
            else if (matchup.player2 == matchup.winner)
            {
                matchup.player1Score = scoreLoser;
                matchup.player2Score = scoreWinner;
                matchup.loser = matchup.player1;
            }
            else
                return false;

            if (matchList != null)
                matchup.matchList = await GetMatchesFromIndexes(matchList);
            indexOfUnresolvedMatches.Remove(matchIndex);
            if (indexOfUnresolvedMatches.Count == 0)
                tourneyState = TournamentStates.Running;
            await SaveDataToDb();
            return true;
        }

        private async Task<List<ChallengeData>> GetMatchesFromIndexes(List<int> range)
        {
            var collec = DatabaseConnection.GetDb("AxieData").GetCollection<ChallengeData>("ChallengeCollec");
            var list = new List<ChallengeData>();
            foreach (var index in range)
            {
                var result = (await collec.FindAsync(data => data._id == index)).FirstOrDefault();
                list.Add(result);
            }
            return list;
        }

        public void Start() => tourneyState = TournamentStates.Ready_For_Seeding;
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
