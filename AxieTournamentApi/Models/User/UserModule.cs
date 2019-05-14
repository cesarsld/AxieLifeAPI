using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace AxieTournamentApi.Models.User
{
    public class UserModule
    {
        public static async Task<bool> CreateUser(BaseUserData value)
        {
            if (!Cryptography.CryptographyModule.IsSignatureValid(value.signature, value.id)) return false;
            var collec = Models.DatabaseConnection.GetDb().GetCollection<UserData>("Users");
            var user = (await collec.FindAsync(u => u.id == value.id.ToLower())).FirstOrDefault();
            if (user == null)
            {
                value.id = value.id.ToLower();
                await collec.InsertOneAsync(new UserData(value));
                return true;
            }
            else
                return false;
        }

        public static async Task<bool> UpdateUser(BaseUserData value)
        {
            if (!Cryptography.CryptographyModule.IsSignatureValid(value.signature, value.id)) return false;
            var collec = Models.DatabaseConnection.GetDb().GetCollection<UserData>("Users");
            var user = (await collec.FindAsync(u => u.id == value.id.ToLower())).FirstOrDefault();
            if (user != null)
            {
                user.userName = value.nickName;
                user.discordId = value.discordId;
                await collec.ReplaceOneAsync(u => u.id == value.id.ToLower(), user);
                return true;
            }
            else
                return false;
        }


        public static async Task<bool> CreateUser(BaseUserDataPw value)
        {
            var collec = Models.DatabaseConnection.GetDb().GetCollection<UserDataPW>("UserData");
            var user = (await collec.FindAsync(u => u.id == value.id.ToLower())).FirstOrDefault();
            if (user == null)
            {
                value.id = value.id.ToLower();
                await collec.InsertOneAsync(new UserDataPW(value));
                return true;
            }
            else
                return false;
        }

        public static async Task<UserData> GetUser(string id)
        {
            var collec = Models.DatabaseConnection.GetDb().GetCollection<UserData>("Users");
            var user = (await collec.FindAsync(u => u.id == id.ToLower())).FirstOrDefault();
            return user;
        }
        public static async Task<UserDataPW> GetUserPw(string id)
        {
            var collec = Models.DatabaseConnection.GetDb().GetCollection<UserDataPW>("UserData");
            var user = (await collec.FindAsync(u => u.id == id.ToLower())).FirstOrDefault();
            return user;
        }

        public static async Task<bool> DeleteUser(string id)
        {
            //TODO ask for signature
            var user = await GetUser(id);
            if (user != null)
            {
                var collec = Models.DatabaseConnection.GetDb().GetCollection<UserData>("Users");
                await collec.DeleteOneAsync(u => u.id == id.ToLower());
                return true;
            }
            else
                return false;
        }
    }
}
