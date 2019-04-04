using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace AxieLifeAPI.Models.SingleElimination
{
    public class SingleEliminationTournament
    {
        private static readonly Random _rand = new Random();
        private const string VIRTUAL_PLAYER = "BYE";

        public string id;
        public string creatorAddress;
        public bool canJoin;
        public List<ParticipantData> participantList;
        public int maxPlayers;
        public int secondsBetweenRounds;
        public int totalRoundNumber;
        public int boFormat;
        public int currentRoundTime;
        public List<MatchUp> matchUpList;

        public SingleEliminationTournament()
        {
        }
        public SingleEliminationTournament(int seconds, string creator, int bo, int max)
        {
            id = "";
            for (int i = 0; i < 40; i++)
                id += _rand.Next(16).ToString("X");

            creatorAddress = creator;
            participantList = new List<ParticipantData>();
            canJoin = true;
            secondsBetweenRounds = seconds;
            boFormat = bo;
            maxPlayers = max;
            totalRoundNumber = 0;
            currentRoundTime = 0;
            matchUpList = new List<MatchUp>();

        }
        public async Task SetupInitialSeeding()
        {
            matchUpList = new List<MatchUp>();
            var tempList = new List<string>();
            foreach (var participant in participantList)
                tempList.Add(participant.ethAddress);
            var totalParticipants = participantList.Count;
            int playerCapacity = 1;
            int rounds = 0;
            //loop to find how many participants and rounds needed
            while (totalParticipants > playerCapacity)
            {
                playerCapacity *= 2;
                rounds++;
            }
            totalRoundNumber = rounds;
            int virtualPlayersNeeded = playerCapacity - totalParticipants;
            for (int i = 0; i < virtualPlayersNeeded; i++)
            {
                var index = _rand.Next(totalParticipants);
                matchUpList.Add(new MatchUp(tempList[index], VIRTUAL_PLAYER));
                totalParticipants--;
                tempList.RemoveAt(index);
            }
            //fill bracket with remaining bracket
            while (totalParticipants != 0)
            {
                var index = _rand.Next(tempList.Count);
                var player = tempList[index];
                tempList.RemoveAt(index);
                var index2 = _rand.Next(tempList.Count);
                var player2 = tempList[index2];
                tempList.RemoveAt(index2);
                matchUpList.Add(new MatchUp(player, player2));
            }
            await SaveDataToDb();
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

        public async Task UpdateCompetition()
        {
            await Utils.UpdateChallengesDB();
            var newList = new List<MatchUp>();
            bool newPair = true;
            foreach (var matchup in matchUpList)
            {
                //creates new matchup every 2 matches read
                if (newPair)
                    newList.Add(new MatchUp());
                if (matchup.player2 != VIRTUAL_PLAYER)
                {
                    List<ChallengeData> bestOfMatches = await GetMatchesFromRange(matchup.player1, matchup.player2, currentRoundTime, currentRoundTime + secondsBetweenRounds);
                    if (bestOfMatches.Count >= boFormat)
                    {
                        int player1Wins = 0;
                        int player2Wins = 0;
                        for (int i = 0; i < boFormat; i++)
                        {
                            if (matchup.player1.ToLower() == bestOfMatches[i].winner.ToLower())
                                player1Wins++;
                            else
                                player2Wins++;
                        }
                        if (player1Wins > player2Wins)
                        {
                            participantList.Find(x => x.ethAddress.ToLower() == matchup.player2.ToLower()).stillCompeting = false;
                            matchup.winner = matchup.player1;
                        }
                        else
                        {
                            participantList.Find(x => x.ethAddress.ToLower() == matchup.player1.ToLower()).stillCompeting = false;
                            matchup.winner = matchup.player2;
                        }
                    }
                    if (newPair)
                        newList.Last().player1 = matchup.winner;
                    else
                        newList.Last().player2 = matchup.winner;
                }
                //make player with bye move to next round
                else
                {
                    if (newPair)
                        newList.Last().player1 = matchup.player1;
                    else
                        newList.Last().player2 = matchup.player1;
                }
                newPair = !newPair;
            }
            matchUpList = newList;
            await SaveDataToDb();

        }

        public async Task<List<ChallengeData>> GetMatchesFromRange(string address, string address2, int a = 0, int b = 2147483647)
        {
            var collec = DatabaseConnection.GetDb().GetCollection<ChallengeData>("ChallengeCollec");

            var list = (await collec.FindAsync(data => (data.winner.ToLower() == address.ToLower() && data.loser.ToLower() == address2.ToLower())
                                                    || (data.winner.ToLower() == address2.ToLower() && data.loser.ToLower() == address.ToLower()))).ToList();
            list = list.Where(w => w.unixTime > a && w.unixTime < b).ToList();
            return list;

        }
    }
}
