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
        public ActionResult<IEnumerable<string>> Get(int match)
        {
            Console.WriteLine($"Match is {match}");
            return new string[] { "Hi RCTech, if you received this messages, the test was successful! :D" };
        }

        // GET api/Tourney/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SingleEliminationTournament>> Get(string id)
        {
            var collec = Models.DatabaseConnection.GetDb().GetCollection<SingleEliminationTournament>("SingleEliminationTournaments");
            var entry = (await collec.FindAsync(a => a.id == id)).FirstOrDefault();
            if (entry != null)
                return entry;
            else
                return null;
        }

        // GET api/Tourney/5
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

        [HttpPost("{id}/start")]
        public async Task<ActionResult> StartTourney(string id, [FromBody]string address)
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
        public async Task<ActionResult<string>> PostNewTourney(TourneyCreationData d)
        {
            var headers = Request.Headers;
            if (headers.ContainsKey("Authorization"))
            {
                //TODO change to fetch list of authorised keys
                if (headers["Authorization"] != "1234")
                {
                    return "Wrong user certification";
                }
                SingleEliminationTournament newTourney;
                try
                {
                    newTourney = new SingleEliminationTournament(d.time, d.creatorAddress.ToLower(), d.bo, d.max);
                    await newTourney.SaveDataToDb();
                }
                catch (Exception e)
                {
                    return e.Message + " Could not create tournament instance.";
                }
                return newTourney.id;
            }
            else
                return "Missing user certification";
        }

        // PUT api/Tourney/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/Tourney/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }

    public class TourneyCreationData
    {
        public int time;
        public int bo;
        public int max;
        public string creatorAddress;
    }
}
