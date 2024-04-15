using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Snipper.server.Models;

namespace Snipper.server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SnippetsController : ControllerBase
    {

        public static List<Snippet> snippets = new List<Snippet>();

        public static long uniqueId = snippets.Count;

        private readonly SnippetContext _context;

        public SnippetsController(SnippetContext context)
        {
            _context = context;
        }

        // GET: api/Snippets
        [HttpGet]
        public ActionResult<List<Snippet>> GetSnippets(string? lang = null)
        {
            if(lang == null)
            {
                return snippets;
            }

            List<Snippet> filteredSnippets = snippets.Where(snippet => snippet.language == lang).ToList();

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

            return snippet;
        }

        // PUT: api/Snippets/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSnippet(long id, Snippet snippet)
        {

            var index = snippets.FindIndex(snippet => snippet.id == id);

            snippets[index] = snippet;

            return NoContent();
        }

        // POST: api/Snippets
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult<Snippet> PostSnippet(Snippet snippet)
        {
            snippet.id = ++uniqueId;
            snippets.Add(snippet);

            return CreatedAtAction("GetSnippet", new { id = snippet.id }, snippet);
        }

        // DELETE: api/Snippets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSnippet(long id)
        {
            var index = snippets.FindIndex(snippet => snippet.id == id);

            snippets.RemoveAt(index);

            return NoContent();
        }

        private bool SnippetExists(long id)
        {
            return _context.Snippets.Any(e => e.id == id);
        }
    }
}
