using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;

namespace AxieTournamentApi.Models.Challonge
{
    public class TournamentJson
    {
        public string name { get; set; }
        public string url { get; set; }
        public string tournament_type { get; set; }
        public string description { get; set; }
        public bool open_signup { get; set; }
        public bool hold_third_place_match { get; set; }

        public TournamentJson(string Creator, string urlId)
        {
            name = $"{Creator}'s single elimination Axie tournament";
            tournament_type = "single elimination";
            description = "Fight to prove you are the best Axie trainer!";
            open_signup = false;
            hold_third_place_match = false;
            url = urlId;
        }

        public string GetPostUrl()
        {
            var url = "https://api.challonge.com/v1/tournaments.json";
            url += "?api_key=" + IO.IOGetter.GetChallongeKey();
            foreach (PropertyInfo pi in this.GetType().GetProperties())
            {
                url += $"&tournament[{pi.Name}]={pi.GetValue(this)}";
            }
            url = url.Replace(" ", "%20");
            return url;
        }
    }
}
