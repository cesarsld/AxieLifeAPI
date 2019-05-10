using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization;
using Newtonsoft.Json;
using AxieTournamentApi.Models.SingleElimination;

namespace AxieTournamentApi.Models
{
    class Utils
    {
        public static async Task UpdateChallengesDB()
        {
            var collec = DatabaseConnection.GetDb().GetCollection<ChallengeData>("ChallengeCollec");
            var count = await collec.CountDocumentsAsync(new BsonDocument());
            var serverError = false;
            while (!serverError)
            {
                var json = "";
                using (System.Net.WebClient wc = new System.Net.WebClient())
                {
                    try
                    {
                        count++;
                        json = wc.DownloadString("https://api.axieinfinity.com/v1/battle/challenge/match/" + count.ToString());
                    }
                    catch (Exception ex)
                    {
                        serverError = true;
                        continue;
                    }
                }
                JObject axieJson = JObject.Parse(json);
                JObject script = JObject.Parse((string)axieJson["script"]);
                var data = new ChallengeData
                {
                    _id = (int)axieJson["id"],
                    winner = (string)axieJson["winner"],
                    loser = (string)axieJson["loser"],
                };
                await collec.InsertOneAsync(data);
            }

        }
    }
}
