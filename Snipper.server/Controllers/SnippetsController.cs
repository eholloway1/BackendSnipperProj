using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Snipper.server.Models;
using Snipper.server.Utilities;

namespace Snipper.server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SnippetsController : ControllerBase
    {

        public static List<Snippet> snippets = new List<Snippet>();

        public static long uniqueId = snippets.Count;

        private readonly ILogger<SnippetsController> _logger;

        private readonly EncryptUtility _encryptUtility;

        public SnippetsController(ILogger<SnippetsController> logger, EncryptUtility encryptUtility)
        {
            _logger = logger;
            //assign encryptUtility instance to field to be able to use Encrypt and Decrypt functions
            _encryptUtility = encryptUtility;
        }

        //private readonly SnippetContext _context;

        //public SnippetsController(SnippetContext context)
        //{
        //    _context = context;
        //}

        // GET: api/Snippets
        [HttpGet]
        public ActionResult<List<Snippet>> GetSnippets(string? lang = null)
        {
            List<Snippet> decodedSnippets = snippets.ConvertAll<Snippet>(snip => new Snippet
            {
                id = snip.id,
                language = snip.language,
                code = _encryptUtility.Decrypt(snip.code)
            });

            if(lang == null)
            {
                return decodedSnippets;
            } 

            List<Snippet> filteredSnippets = decodedSnippets.Where(snippet => snippet.language == lang).ToList();

            return filteredSnippets;
        }

        // GET: api/Snippets/5
        [HttpGet("{id}")]
        public ActionResult<Snippet> GetSnippet(long id)
        {
            Snippet? snippet = snippets.Find(snippet => snippet.id == id);

            if (snippet == null)
            {
                return NotFound("Snippet not found.");
            }

            Snippet decodedSnippet = new Snippet
            {
                id = snippet.id,
                language = snippet.language,
                code = _encryptUtility.Decrypt(snippet.code)
            };

            return decodedSnippet;
        }

        // PUT: api/Snippets/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public ActionResult PutSnippet(Snippet inSnippet)
        {

            var index = snippets.FindIndex(snippet => snippet.id == inSnippet.id);

            if (index == -1)
            {
                return BadRequest();
            }

            Snippet codedSnippet = new Snippet
            {
                id = inSnippet.id,
                language = inSnippet.language,
                code = _encryptUtility.Encrypt(inSnippet.code)
            };

            snippets[index] = codedSnippet;

            
            return NoContent();
        }

        // POST: api/Snippets
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult<Snippet> PostSnippet(Snippet snippet)
        {
            snippet.id = ++uniqueId;

            snippet.code = _encryptUtility.Encrypt(snippet.code);

            snippets.Add(snippet);

            return CreatedAtAction("GetSnippet", new { id = snippet.id }, snippet);
        }

        // DELETE: api/Snippets/5
        [HttpDelete("{id}")]
        public ActionResult DeleteSnippet(long id)
        {
            var index = snippets.FindIndex(snippet => snippet.id == id);

            snippets.RemoveAt(index);

            return NoContent();
        }

        //private bool SnippetExists(long id)
        //{
        //    return _context.Snippets.Any(e => e.id == id);
        //}
    }
}
