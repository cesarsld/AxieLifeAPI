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
        // GET: api/User/5
        [HttpGet("{address}", Name = "Get")]
        public async Task<IActionResult> GetUser(string address)
        {
            var user = await UserModule.GetUser(address);
            if (user != null)
                return Ok(user);
            else
                return NotFound();
        }

        // POST: api/User
        [HttpPost("register")]
        public async Task<IActionResult> PostCreateUser(BaseUserData value)
        {
            if (await UserModule.CreateUser(value))
                return Ok("User registered.");
            else
                return Ok("User already registered.");
        }

        // POST: api/User/register
        [HttpPost("registerPW")]
        public async Task<IActionResult> PostCreateUSerPw(BaseUserDataPw value)
        {
            if (await UserModule.CreateUser(value))
                return Ok("User registered.");
            else
                return Ok("User already registered.");
        }

        // POST: api/User/login
        [HttpGet("login")]
        public async Task<ActionResult> GetLogin(LoginData value)
        {
            var user = await UserModule.GetUserPw(value.id);
            if (user != null)
            {
                if (user.Login(value.password))
                    return Ok("User logged in.");
                else
                    return
                        Ok("Wrong password.");
            }
            else
                return NotFound("User not registered.");
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/User/5
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteUser(string id)
        {
            //TODO add auth tod delete
            if (await UserModule.DeleteUser(id))
                return Ok("User Deleted");
            else
                return NotFound("User not found");

        }
    }
}
