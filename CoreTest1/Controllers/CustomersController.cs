using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreTest1.Models;
using CoreTest1.Data;

namespace CoreTest1.Controllers
{
    [Route("api/Customers")]
    [ApiController]
    public class CustomersController : ControllerBase
    { 
        private readonly RocketContext _context;

        public CustomersController(RocketContext context)
        {
            _context = context;
        }

        // GET: Customers
        [HttpGet]
        public IQueryable<Customer> Index(string sortOrder, string searchString)
        {
            //ViewData["NameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            //ViewData["CurrentFilter"] = searchString;
            var customers = from s in _context.Customers
                         select s;
            if (!string.IsNullOrEmpty(searchString))
            {
                customers = customers.Where(s => s.Name.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    customers = customers.OrderByDescending(s => s.Name);
                    break;
                default:
                    customers = customers.OrderBy(s => s.Name);
                    break;
            }
            var c = from cust in _context.Customers select cust;
            //return View(await customers.ToListAsync());
            //return Content(System.Net.HttpStatusCode.OK, new List<string> { "", ""});
            return c;
        }

        // GET: Customers/Details/5
       [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.ID == id);
            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }

        // GET: Customers/Create
        /*[HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> Create()
        {
            return await _context.Customers.ToListAsync();
        }*/

        //POST: Customers/Create
        [HttpPost]
        public async Task<ActionResult<Customer>> Create(/*[Bind("ID,Name")]*/ Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return CreatedAtAction(nameof(Details), new { id = customer.ID }, customer);
        }

        // POST: Customers/Edit/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, /*[Bind("ID,Name")] */Customer customer)
        {
            if (id != customer.ID)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Entry(customer).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.ID))
                    {
                        return BadRequest();
                    }
                    else
                    {
                        throw;
                    }
                }
                return NoContent();
            }
            return NoContent();
        }

        //POST: Customers/Delete/5
        //[HttpPost, ActionName("Delete")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.ID == id);
        }
    }
}
