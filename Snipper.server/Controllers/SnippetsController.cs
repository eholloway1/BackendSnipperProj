using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly SnippetContext _context;

        public SnippetsController(SnippetContext context)
        {
            _context = context;
        }

        // GET: api/Snippets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Snippet>>> GetSnippets()
        {
            return await _context.Snippets.ToListAsync();
        }

        // GET: api/Snippets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Snippet>> GetSnippet(long id)
        {
            var snippet = await _context.Snippets.FindAsync(id);

            if (snippet == null)
            {
                return NotFound();
            }

            return snippet;
        }

        // PUT: api/Snippets/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSnippet(long id, Snippet snippet)
        {
            if (id != snippet.id)
            {
                return BadRequest();
            }

            _context.Entry(snippet).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SnippetExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Snippets
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Snippet>> PostSnippet(Snippet snippet)
        {
            _context.Snippets.Add(snippet);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSnippet", new { id = snippet.id }, snippet);
        }

        // DELETE: api/Snippets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSnippet(long id)
        {
            var snippet = await _context.Snippets.FindAsync(id);
            if (snippet == null)
            {
                return NotFound();
            }

            _context.Snippets.Remove(snippet);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SnippetExists(long id)
        {
            return _context.Snippets.Any(e => e.id == id);
        }
    }
}
