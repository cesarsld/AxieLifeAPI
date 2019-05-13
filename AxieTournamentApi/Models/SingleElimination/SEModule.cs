using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
namespace AxieTournamentApi.Models.SingleElimination
{
    public class SEModule
    {
        public static async Task<bool> JoinTourney(string id, string userAddress)
        {
            var tourney = await GetTourney(id);
            var user = await User.UserModule.GetUser(userAddress);
            if (tourney != null)
            {
                if (user != null)
                {
                    var didJoin = await tourney.AddPlayer(user);
                    if (didJoin)
                        return true;
                    else
                        return false;
                }
                else return false;
            }
            else return false;
        }

        public static async Task<SingleEliminationTournament> GetTourney(string id)
        {
            var tourneyCollec = DatabaseConnection.GetDb().GetCollection<SingleEliminationTournament>("SingleEliminationTournaments");
            var tourney = (await tourneyCollec.FindAsync(a => a.id == id.ToLower())).FirstOrDefault();
            return tourney;
        }

        public static async Task<string> CreateTourney(TourneyCreationData data)
        {
            var newTourney = new SingleEliminationTournament(data.time, data.creatorAddress.ToLower(), data.bo, data.max, data.creatorName);
            var challongeId = await Challonge.ChallongeModule.CreateTournament(newTourney.creatorName, newTourney.id);
            newTourney.challongeId = challongeId;
            await newTourney.SaveDataToDb();
            return newTourney.id;        
        }

        public static async Task<bool> ResolveMatchup(string tourneyId, ResolveData resolveData)
        {
            var tourney = await GetTourney(tourneyId);
            if (await tourney.ResolveMatchUp(resolveData.matchIndex, resolveData.winner, resolveData.scoreWinner, resolveData.scoreLoser, resolveData.matchupList))
                return true;
            else return false;
        }

        public static async Task<bool> StartTourney(string id)
        {
            var tourney = await GetTourney(id);
            if (tourney != null)
            {
                tourney.Start();
                await tourney.SaveDataToDb();
                return true;
            }
            else return false;
        }
    }
}
