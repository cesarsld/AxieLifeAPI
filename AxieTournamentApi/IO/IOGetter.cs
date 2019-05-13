using System;
using System.Text;
using System.IO;
using System.Numerics;
using System.Threading.Tasks;

namespace AxieTournamentApi.IO
{
    class IOGetter
    {
        private static readonly string dbUrlPath = "DbKey/DbKey.txt";
        private static readonly string discordKeyPath = "Discord/Key.txt";
        private static readonly string challongeKeyPath = "Challonge/Key.txt";
        public static string GetDiscordKey()
        {
            if (File.Exists(discordKeyPath))
            {
                using (StreamReader sr = new StreamReader(discordKeyPath, Encoding.UTF8))
                {
                    string key = sr.ReadToEnd();
                    return key;
                }
            }
            else
            {
                using (StreamReader sr = new StreamReader(discordKeyPath, Encoding.UTF8))
                {
                    string key = sr.ReadToEnd();
                    return key;
                }
            }
        }

        public static string GetChallongeKey()
        {
            using (StreamReader sr = new StreamReader(challongeKeyPath, Encoding.UTF8))
            {
                string key = sr.ReadToEnd();
                return key;
            }
        }

        public static string GetDBUrl()
        {
            if (File.Exists(dbUrlPath))
            {
                using (StreamReader sr = new StreamReader(dbUrlPath, Encoding.UTF8))
                {
                    string key = sr.ReadToEnd();
                    return key;
                }
            }
            else return "";
        }
        public static void SetDBUrl(string url)
        {
            if (File.Exists(dbUrlPath))
            {
                using (StreamWriter sw = new StreamWriter(dbUrlPath))
                {
                    sw.Write(url);
                }
            }
        }

        public static string GetABI(string name)
        {
            using (StreamReader sr = new StreamReader("AxieData/" + name + ".txt", Encoding.UTF8))
            {
                string abi = sr.ReadToEnd();
                return abi;
            }
        }

        public static BigInteger GetLastCheckedBlock()
        {
            using (StreamReader sr = new StreamReader("AxieData/LastBlock.txt", Encoding.UTF8))
            {
                string blockNumber = sr.ReadToEnd();
                return BigInteger.Parse(blockNumber);
            }
        }
        public static void SetLastCheckedBlock(BigInteger number)
        {
            using (StreamWriter sw = new StreamWriter("AxieData/LastBlock.txt"))
            {
                sw.Write(number.ToString());
            }
        }
        public static int GetLastCheckedAxie()
        {
            using (StreamReader sr = new StreamReader("AxieData/LastAxie.txt", Encoding.UTF8))
            {
                string axie = sr.ReadToEnd();
                return Convert.ToInt32(axie);
            }
        }
        public static void SetLastCheckedAxie(int number)
        {
            using (StreamWriter sw = new StreamWriter("AxieData/LastAxie.txt"))
            {
                sw.Write(number.ToString());
            }
        }
    }
}
