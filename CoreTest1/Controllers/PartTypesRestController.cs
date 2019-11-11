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
    public class PartTypesRestController : ControllerBase
    {
        private readonly RocketContext _context;

        public PartTypesRestController(RocketContext context)
        {
            _context = context;
        }

        // GET: api/PartTypesRest
        [HttpGet]
        public IEnumerable<PartType> GetPartTypes()
        {
            return _context.PartTypes;
        }

        // GET: api/PartTypesRest/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPartType([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var partType = await _context.PartTypes.FindAsync(id);

            if (partType == null)
            {
                return NotFound();
            }

            return Ok(partType);
        }

        // PUT: api/PartTypesRest/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPartType([FromRoute] int id, [FromBody] PartType partType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != partType.ID)
            {
                return BadRequest();
            }

            _context.Entry(partType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PartTypeExists(id))
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

        // POST: api/PartTypesRest
        [HttpPost]
        public async Task<IActionResult> PostPartType([FromBody] PartType partType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.PartTypes.Add(partType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPartType", new { id = partType.ID }, partType);
        }

        // DELETE: api/PartTypesRest/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePartType([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var partType = await _context.PartTypes.FindAsync(id);
            if (partType == null)
            {
                return NotFound();
            }

            _context.PartTypes.Remove(partType);
            await _context.SaveChangesAsync();

            return Ok(partType);
        }

        private bool PartTypeExists(int id)
        {
            return _context.PartTypes.Any(e => e.ID == id);
        }


        //Populate autocomplete source
        [Produces("application/json")]
        [HttpGet("search")]
        public IActionResult Search()
        {
            try
            {
                string term = HttpContext.Request.Query["term"].ToString();
                var dropDownContent = _context.PartTypes.Where(p => p.Name.Contains(term)).Select(p => new { value = p.ID, label = p.Name }).ToList();
                return Ok(dropDownContent);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}