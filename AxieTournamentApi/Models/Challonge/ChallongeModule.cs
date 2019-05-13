using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json.Linq;
namespace AxieTournamentApi.Models.Challonge
{
    public class ChallongeModule
    {

        public static async Task<int> CreateTournament(string creator, string tournamentId)
        {
            var data = new TournamentJson(creator, tournamentId);
            var url = data.GetPostUrl();
            using (var cl = new HttpClient())
            {
                var response = await cl.PostAsync(url, null);
                var content = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(content);
                return (int)json["tournament"]["id"];
            }
        }

        public static async Task<int> AddPlayer(int challongeId, string playerAddress, string playerName)
        {
            var url = GetPlayerBaseUrl(challongeId);
            url += $"&participant[name]={playerName}";
            url += $"&participant[misc]={playerAddress}";
            url = url.Replace(" ", "%20");
            using (var cl = new HttpClient())
            {
                var response = await cl.PostAsync(url, null);
                var content = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(content);
                return (int)json["participant"]["id"];
            }
        }


        private static string GetTournamentBaseUrl()
        {
            var url = "https://api.challonge.com/v1/tournaments.json";
            url += "?api_key=" + IO.IOGetter.GetChallongeKey();
            return url;
        }
        private static string GetPlayerBaseUrl(int challongeId)
        {
            var baseurl = "https://api.challonge.com/v1/tournaments/" + challongeId.ToString() + "/participants.json";
            baseurl += "?api_key=" + IO.IOGetter.GetChallongeKey();
            return baseurl;
        }
    }
}
