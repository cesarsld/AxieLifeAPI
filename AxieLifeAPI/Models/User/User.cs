using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AxieLifeAPI.Models.Cryptography;
namespace AxieLifeAPI.Models.User
{
    public class BaseUserData
    {
        public string id;
        public ulong discordId;
    }

    public class LoginData
    {
        public string id;
        public string password;

    }

    public class BaseUserDataPw
    {
        public string id;
        public ulong discordId;
        public string password;
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

            ongoingCreatedTournaments = new List<CompletedTournaments>();
            completedCreatedTournaments = new List<CompletedTournaments>();
        }
        public bool IsDiscordIdValid() => discordId != 0;
    }

    public class UserDataPW
    {
        public string id;
        public ulong discordId;

        public byte[] salt;
        public byte[] hash;

        public List<CompletedTournaments> ongoingTournaments;
        public List<CompletedTournaments> completedTournaments;

        public List<CompletedTournaments> ongoingCreatedTournaments;
        public List<CompletedTournaments> completedCreatedTournaments;

        public UserDataPW(BaseUserDataPw data)
        {
            id = data.id;
            discordId = data.discordId;

            completedTournaments = new List<CompletedTournaments>();
            ongoingTournaments = new List<CompletedTournaments>();

            ongoingCreatedTournaments = new List<CompletedTournaments>();
            completedCreatedTournaments = new List<CompletedTournaments>();
            salt = HashEncryption.GenerateSalt();
            hash = HashEncryption.GenerateHash(data.password, salt);
        }
        public bool Login(string password)
        {
            if (HashEncryption.ConfirmPassword(password, salt, hash))
                return true;
            else
                return false;
        }
        public bool IsDiscordIdValid() => discordId != 0;
    }

    public class CompletedTournaments
    {
        public string id;
        public int ranking;
    }
}
