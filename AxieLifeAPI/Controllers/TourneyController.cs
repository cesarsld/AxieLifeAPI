using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using MongoDB.Driver.Linq;
using MongoDB.Driver;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AxieTournamentApi.Models.SingleElimination;
using AxieTournamentApi.Models.User;

namespace AxieTournamentApi.Controllers
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
        public async Task<ActionResult> GetTourney(string id)
        {
            var tourney = await SEModule.GetTourney(id);
            if (tourney != null)
                return Ok(tourney);
            else
                return NotFound();
        }

        // PUT api/Tourney/ID/join
        [HttpPut("{id}/join")]
        public async Task<ActionResult> JoinTourney(string tourneyId, [FromBody]string userAddress)
        {
            if(await SEModule.JoinTourney(tourneyId, userAddress))
                return Ok("User joined.");
            else
                return Ok("User could not join because Tournament has reach max capacity or user has already joined.");
        }

        // POST api/Tourney
        [HttpPost]
        public async Task<ActionResult> PostNewTourney(TourneyCreationData d)
        {
            // TODO refarctor authentification
            if (await CheckAuthentification())
            {
                var tourneyId = await SEModule.CreateTourney(d);
                return Ok(tourneyId);
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
                if (await SEModule.ResolveMatchup(id, resolveData))
                    return Ok("MatchUp Resolved");
                else
                    return Ok("MatchUp could not be resolved");
            }
            return Forbid("Missing or wrong user certification");
        }

        // PUT api/Tourney/ID/start
        [HttpPut("{id}/start")]
        public async Task<ActionResult> PutStartTourney(string id)
        {
            if (await CheckAuthentification())
            {
                if(await SEModule.StartTourney(id))
                    return Ok("Tournament Started");
                else return NotFound("Tournament ID not found");
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

    public class AuthentificationKey
    {
        public string id;
    }
    
}
