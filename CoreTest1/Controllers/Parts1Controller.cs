using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoreTest1.Data;
using CoreTest1.Models;

namespace CoreTest1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Parts1Controller : ControllerBase
    {
        private readonly RocketContext _context;

        public Parts1Controller(RocketContext context)
        {
            _context = context;
        }

        [Produces("application/json")]
        [HttpGet("search")]
        public async Task<IActionResult> Populate()
        {
            try
            {
                string term = HttpContext.Request.Query["term"].ToString();
                var names = _context.PartTypes.Where(p => p.Name.Contains(term)).Select(p => p.Name).ToList();
                return Ok(names);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET: api/Parts1
        [HttpGet]
        public IEnumerable<Part> GetParts()
        {
            return _context.Parts;
        }

        // GET: api/Parts1/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPart([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var part = await _context.Parts.FindAsync(id);

            if (part == null)
            {
                return NotFound();
            }

            return Ok(part);
        }

        // PUT: api/Parts1/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPart([FromRoute] int id, [FromBody] Part part)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != part.ID)
            {
                return BadRequest();
            }

            _context.Entry(part).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PartExists(id))
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

        // POST: api/Parts1
        [HttpPost]
        public async Task<IActionResult> PostPart([FromBody] Part part)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Parts.Add(part);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPart", new { id = part.ID }, part);
        }

        // DELETE: api/Parts1/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePart([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var part = await _context.Parts.FindAsync(id);
            if (part == null)
            {
                return NotFound();
            }

            _context.Parts.Remove(part);
            await _context.SaveChangesAsync();

            return Ok(part);
        }

        private bool PartExists(int id)
        {
            return _context.Parts.Any(e => e.ID == id);
        }
    }
}