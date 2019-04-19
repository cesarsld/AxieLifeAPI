using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using MongoDB.Driver.Linq;
using MongoDB.Driver;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AxieLifeAPI.Models.SingleElimination;
using AxieLifeAPI.Models.User;

namespace AxieLifeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TourneyController : ControllerBase
    {
        // GET api/Tourney
        [HttpGet()]
        public ActionResult<IEnumerable<string>> GetTest(int match)
        {
            Console.WriteLine($"Match is {match}");
            return new string[] { "Hi, if you received this messages, the test was successful! :D" };
        }

        // GET api/Tourney/ID
        [HttpGet("{id}")]
        public async Task<ActionResult<SingleEliminationTournament>> GetTourney(string id)
        {
            var collec = Models.DatabaseConnection.GetDb().GetCollection<SingleEliminationTournament>("SingleEliminationTournaments");
            var entry = (await collec.FindAsync(a => a.id == id)).FirstOrDefault();
            if (entry != null)
                return entry;
            else
                return null;
        }

        // POST api/Tourney/ID/join
        [HttpPost("{id}/join")]
        public async Task<ActionResult> JoinTourney(string id, [FromBody]string address)
        {
            var tourneyCollec = Models.DatabaseConnection.GetDb().GetCollection<SingleEliminationTournament>("SingleEliminationTournaments");
            var userCollec = Models.DatabaseConnection.GetDb().GetCollection<UserData>("Users");
            var tourney = (await tourneyCollec.FindAsync(a => a.id == id)).FirstOrDefault();
            var user = (await userCollec.FindAsync(a => a.id == address.ToLower())).FirstOrDefault();
            if (tourney != null)
            {
                if (user != null)
                {
                    var success = await tourney.AddPlayer(user);
                    if (success)
                        return Ok("User joined.");
                    else
                        return Ok("User could not join because Tournament has reach max capacity or user has already joined.");
                }
                else
                    return NotFound("User not found.");
            }
            else
                return NotFound("Tournament not found.");
        }

        // POST api/Tourney
        [HttpPost]
        public async Task<ActionResult> PostNewTourney(TourneyCreationData d)
        {
            if (await CheckAuthentification())
            { 
                SingleEliminationTournament newTourney;
                try
                {
                    newTourney = new SingleEliminationTournament(d.time, d.creatorAddress.ToLower(), d.bo, d.max);
                    await newTourney.SaveDataToDb();
                }
                catch (Exception e)
                {
                    return Ok(e.Message + " Could not create tournament instance.");
                }
                return Ok(newTourney.id);
            }
            else
                return Forbid("Missing or wrong user certification");
        }

        // PUT api/Tourney/ID/resolve
        [HttpPut("{id}/resolve")]
        public async Task<ActionResult> PutResolveMatchup(string id, [FromBody] ResolveData resolveData)
        {
            if (await CheckAuthentification())
            {
                var tourneyCollec = Models.DatabaseConnection.GetDb().GetCollection<SingleEliminationTournament>("SingleEliminationTournaments");
                var tourney = (await tourneyCollec.FindAsync(a => a.id == id)).FirstOrDefault();
                if (tourney != null)
                {
                    if(await tourney.ResolveMatchUp(resolveData.matchIndex, resolveData.winner, resolveData.scoreWinner, resolveData.scoreLoser, resolveData.matchupList))
                        return Ok("MatchUp Resolved");
                    else
                        return Ok("MatchUp could not be resolved");
                }
                return NotFound("Tournament ID not found");
            }
            return Forbid("Missing or wrong user certification");
        }

        // PUT api/Tourney/ID/start
        [HttpPut("{id}/start")]
        public async Task<ActionResult> PutStartTourney(string id)
        {
            if (await CheckAuthentification())
            {
                var tourneyCollec = Models.DatabaseConnection.GetDb().GetCollection<SingleEliminationTournament>("SingleEliminationTournaments");
                var tourney = (await tourneyCollec.FindAsync(a => a.id == id)).FirstOrDefault();
                if (tourney != null)
                {
                    tourney.Start();
                    await tourney.SaveDataToDb();
                    return Ok("Tournament Started");
                }
                return NotFound("Tournament ID not found");
            }
            return Forbid("Missing or wrong user certification");

        }

        // DELETE api/Tourney/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        private async Task<bool> CheckAuthentification()
        {
            var headers = Request.Headers;
            if (headers.ContainsKey("Authorization"))
            {
                var authCollec = Models.DatabaseConnection.GetDb().GetCollection<AuthentificationKey>("AuthKeys");
                var key = (await authCollec.FindAsync(a => a.id == headers["Authorization"])).FirstOrDefault();
                if (key != null)
                    return true;
                else
                    return false;
            }
                return false;
        }
    }

    public class TourneyCreationData
    {
        public int time;
        public int bo;
        public int max;
        public string creatorAddress;
    }
    public class AuthentificationKey
    {
        public string id;
    }
    public class ResolveData
    {
        public int matchIndex;
        public string winner;
        public int scoreWinner;
        public int scoreLoser;
        public List<int> matchupList;
    }
}
