using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.Linq;
using AxieLifeAPI.IO;
namespace AxieLifeAPI.Models
{
    class DatabaseConnection
    {
        private static MongoClient Client;
        private static IMongoDatabase AxieDatabase;
        private static IMongoCollection<BsonDocument> Collection;


        public static void SetupConnection(string db = "TournamentData")
        {
            var connectionString = IOGetter.GetDBUrl();

            Client = new MongoClient(connectionString);
            AxieDatabase = Client.GetDatabase(db);
        }

        public static void UpdateIpAddress(string ip)
        {
            var connectionString = IOGetter.GetDBUrl();
            var newString = connectionString.Substring(0, 26);
            newString += ip + ":27017";
            
        }

        public static IMongoDatabase GetDb()
        {

            if (Client == null)
            {
                SetupConnection();
            }
            return AxieDatabase;
        }

    }
}
