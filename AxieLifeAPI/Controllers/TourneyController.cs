using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using MongoDB.Driver.Linq;
using MongoDB.Driver;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AxieLifeAPI.Models.SingleElimination;

namespace AxieLifeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AxiesController : ControllerBase
    {
        // GET api/axies
        [HttpGet()]
        public ActionResult<IEnumerable<string>> Get(int match)
        {
            Console.WriteLine($"Match is {match}");
            return new string[] { "value1", "value2" };
        }

        // GET api/axies/5
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

        // POST api/axies
        [HttpPost]
        public async Task<ActionResult<string>> PostNewTourney(TourneyCreationData d)
        {
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

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
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
