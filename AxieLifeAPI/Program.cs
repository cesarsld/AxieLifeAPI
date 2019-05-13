using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AxieTournamentApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine(Environment.CurrentDirectory);
            Console.WriteLine("");
            //AxieTournamentApi.Models.Challonge.ChallongeModule.CreateTournament().GetAwaiter().GetResult();
            CreateWebHostBuilder(args).Build().Run();
        }
#if DEBUG
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
#else
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseSetting("https_port", "1337")
            .UseStartup<Startup>()
            .UseUrls("https://*:1337");
#endif
    }
}
