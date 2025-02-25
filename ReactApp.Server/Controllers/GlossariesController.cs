using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReactApp.Server.Data;
using ReactApp.Server.Entity;

namespace ReactApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GlossariesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GlossariesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Glossaries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Glossary>>> GetGlossaries()
        {
            return await _context.Glossaries.ToListAsync();
        }

        // GET: api/Glossaries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Glossary>> GetGlossary(Guid id)
        {
            var glossary = await _context.Glossaries.FindAsync(id);

            if (glossary == null)
            {
                return NotFound();
            }

            return glossary;
        }

        // PUT: api/Glossaries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGlossary(Guid id, Glossary glossary)
        {
            if (id != glossary.Guid)
            {
                return BadRequest();
            }

            _context.Entry(glossary).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GlossaryExists(id))
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

        // POST: api/Glossaries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Glossary>> PostGlossary(Glossary glossary)
        {
            _context.Glossaries.Add(glossary);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGlossary", new { id = glossary.Guid }, glossary);
        }

        // DELETE: api/Glossaries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGlossary(Guid id)
        {
            var glossary = await _context.Glossaries.FindAsync(id);
            if (glossary == null)
            {
                return NotFound();
            }

            _context.Glossaries.Remove(glossary);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GlossaryExists(Guid id)
        {
            return _context.Glossaries.Any(e => e.Guid == id);
        }
    }
}
