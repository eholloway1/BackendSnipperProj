using Microsoft.AspNetCore.Mvc;
using Snipper.server.Models;
using Snipper.server.Services;
using Snipper.server.Utilities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Snipper.server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IdentityService _identityService;

        public UserController(ILogger<UserController> logger, IdentityService identityService)
        {
            _logger = logger;
            _identityService = identityService;
        }

        // GET: api/<UserController>
        [HttpGet]
        public ActionResult<User> GetUser()
        {
            //fetch authenticated user from HttpContext Items
            User? user = HttpContext.Items["User"] as User;

            //if null, the authentication failed
            if(user == null)
            {
                return Unauthorized();
            }

            //don't send back hashed password
            return Ok(new { id = user.id, Email = user.Email });
        }

        //// GET api/<UserController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/<UserController>
        [HttpPost]
        public ActionResult<User> CreateUser()
        {
            //fetch created user from HttpContect Items
            User? user = HttpContext.Items["User"] as User;

            //if null, the creation failed
            if(user == null )
            {
                return BadRequest();
            }

            //Don't send back hashed password
            return Ok(new {id =  user.id, Email = user.Email});
        }

        //// PUT api/<UserController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<UserController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
