//using Microsoft.AspNetCore.Mvc;
//using Snipper.server.Models;

//// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

//namespace Snipper.server.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class UserController : ControllerBase
//    {
//        public static List<User> users = new List<User>();

//        public static long userId = users.Count;

//        private readonly ILogger<UserController> _logger;

//        public UserController(ILogger<UserController> logger)
//        {
//            _logger = logger;
//        }

//        // GET: api/<UserController>
//        [HttpGet]
//        public ActionResult<List<User>> GetUsers(string username, string password)
//        {
//            return new string[] { "value1", "value2" };
//        }

//        // GET api/<UserController>/5
//        [HttpGet("{id}")]
//        public string Get(int id)
//        {
//            return "value";
//        }

//        // POST api/<UserController>
//        [HttpPost]
//        public void Post([FromBody] string value)
//        {
//        }

//        // PUT api/<UserController>/5
//        [HttpPut("{id}")]
//        public void Put(int id, [FromBody] string value)
//        {
//        }

//        // DELETE api/<UserController>/5
//        [HttpDelete("{id}")]
//        public void Delete(int id)
//        {
//        }
//    }
//}
