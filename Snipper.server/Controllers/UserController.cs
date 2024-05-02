using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snipper.server.Models;
using Snipper.server.Services;
using Snipper.server.Utilities;
using Snipper.server.Middleware;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Snipper.server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IdentityService _identityService;
        private readonly JwtSettings _jwtSettings;

        public UserController(IdentityService identityService, IOptions<JwtSettings> jwtSettings)
        {
            _identityService = identityService;
            _jwtSettings = jwtSettings.Value;
        }

        // GET: api/<UserController>
        [HttpGet]
        [Authorize]
        public ActionResult<User> GetUser()
        {

            //Retrieving the authenticated User (ClaimsPrincipal) from HttpContext
            //which is populated by JwtMiddleware
            var user = User;

            //If null, the authentication failed
            if(user == null)
            {
                return Unauthorized(new {error = "Couldn't access user data."});
            }

            //Parsing the user's Id and Email from the user claims
            long Id = long.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            string Email = user.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;

            //Dont' send back hashed password
            return Ok(new {id = Id, email = Email});

            /*
            //fetch authenticated user from HttpContext Items
            User? user = HttpContext.Items["User"] as User;

            //if null, the authentication failed
            if(user == null)
            {
                return Unauthorized();
            }

            //don't send back hashed password
            return Ok(new { id = user.id, Email = user.Email });
            */
        }

        //// GET api/<UserController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/<UserController>
        [HttpPost]
        public ActionResult<User> CreateUser(User? inUser)
        {
            //Fetch created user from HttpContext Items
            User? user = HttpContext.Items["User"] as User;

            //If null, the creation failed
            if(user == null)
            {
                return BadRequest();
            }

            //Don't send back hashed password
            return Ok(new {id = user.id, Email = user.Email });

            /*
            if(inUser == null)
            {
                //fetch created user from HttpContect Items
                User? user = HttpContext.Items["User"] as User;

                //if null, the creation failed
                if (user == null)
                {
                    return BadRequest();
                }

                //Don't send back hashed password
                return Ok(new { id = user.id, Email = user.Email });
            }
            else
            {
                HttpContext.Items.Add("User", inUser);
                //fetch created user from HttpContect Items
                User? user = HttpContext.Items["User"] as User;

                //if null, the creation failed
                if (user == null)
                {
                    return BadRequest();
                }

                //Don't send back hashed password
                return Ok(new { id = user.id, Email = user.Email });
            }
            */

        }

        [HttpPost("login")]
        public ActionResult Login()
        {
            //Fetch created user from HttpContext Items
            User? user = HttpContext.Items["User"] as User;

            //If null, the authentication failed
            if (user == null)
            {
                return Unauthorized(new { error = "Invalid email or password." });
            }

            var token = _identityService.GenerateToken(user);

            return Ok(new {token, user.id, user.Email});
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
