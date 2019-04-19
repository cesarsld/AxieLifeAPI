using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AxieLifeAPI.Models.User;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using AxieLifeAPI.Models.Notification;
namespace AxieLifeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        // GET: api/User
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/User/5
        [HttpGet("{address}", Name = "Get")]
        public async Task<IActionResult> Get(string address)
        {
            var collec = Models.DatabaseConnection.GetDb().GetCollection<UserData>("Users");
            var user = (await collec.FindAsync(u => u.id == address)).FirstOrDefault();
            if (user != null)
            {
                await NotificationSender.StartClient();
                await NotificationSender.SendMessage(user, "This is a test");
                return Ok(user);
            }
            else
                return NotFound();
        }

        // POST: api/User
        [HttpPost]
        public async Task<string> Post(BaseUserData value)
        {
            var collec = Models.DatabaseConnection.GetDb().GetCollection<UserData>("Users");
            var user = (await collec.FindAsync(u => u.id == value.id.ToLower())).FirstOrDefault();
            if (user == null)
            {
                value.id = value.id.ToLower();
                await collec.InsertOneAsync(new UserData(value));
                return "User registered.";
            }
            else
                return "User already registered.";

        }

        // POST: api/User/register
        [HttpPost("register")]
        public async Task<string> PostUserData(BaseUserDataPw value)
        {
            var collec = Models.DatabaseConnection.GetDb().GetCollection<UserDataPW>("UserData");
            var user = (await collec.FindAsync(u => u.id == value.id.ToLower())).FirstOrDefault();
            if (user == null)
            {
                value.id = value.id.ToLower();
                await collec.InsertOneAsync(new UserDataPW(value));
                return "User registered.";
            }
            else
                return "User already registered.";
        }

        // POST: api/User
        [HttpGet("login")]
        public async Task<ActionResult> GetLogin(LoginData value)
        {
            var collec = Models.DatabaseConnection.GetDb().GetCollection<UserDataPW>("UserData");
            var user = (await collec.FindAsync(u => u.id == value.id.ToLower())).FirstOrDefault();
            if (user != null)
            {
                if (user.Login(value.password))
                    return Ok("User logged in.");
                else
                    return
                        Ok("Wrong password.");
            }
            else
                return NotFound("User already registered.");

        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
