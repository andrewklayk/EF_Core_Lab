using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoreTest1.Data;
using CoreTest1.Models;

namespace CoreTest1.Controllers
{
    public class LeftsController : Controller
    {
        private readonly RocketContext _context;

        public LeftsController(RocketContext context)
        {
            _context = context;
        }

        // GET: Lefts
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewData["DateSortParm"] = string.IsNullOrEmpty(sortOrder) ? "date_asc" : "";
            ViewData["QuantSortParm"] = sortOrder == "Quantity" ? "quantity_desc" : "Quantity";
            ViewData["CurrentFilter"] = searchString;
            IQueryable<Left> lefts = _context.Lefts.Include(l => l.Stock).Include(l => l.Part).ThenInclude(p => p.PartType);
            int a; switch (sortOrder)
            {
                case "date_asc":
                    lefts = lefts.OrderBy(s => s.ArrDate);
                    break;
                case "Quantity":
                    lefts = lefts.OrderBy(s => s.Quantity);
                    break;
                case "quantity_desc":
                    lefts = lefts.OrderByDescending(s => s.Quantity);
                    break;
                default:
                    lefts = lefts.OrderByDescending(s => s.ArrDate);
                    break;
            }
            if (!string.IsNullOrEmpty(searchString))
            {
                lefts = lefts.Where(l => l.Part.Name.Contains(searchString) || l.Stock.Address.Contains(searchString) || (int.TryParse(searchString, out a) && a == l.Quantity));
            }
            return View(await lefts.ToListAsync());
        }

        // GET: Lefts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var left = await _context.Lefts
                .Include(l => l.Part)
                .Include(l => l.Stock)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (left == null)
            {
                return NotFound();
            }

            return View(left);
        }

        // GET: Lefts/Create
        public IActionResult Create()
        {
            ViewData["PartID"] = new SelectList(_context.Parts, "ID", "Name");
            ViewData["StockID"] = new SelectList(_context.Stocks, "ID", "Address");
            return View();
        }

        // POST: Lefts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PartID,StockID,ArrDate,Quantity")] Left left)
        {
            if (ModelState.IsValid)
            {
                _context.Add(left);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PartID"] = new SelectList(_context.Parts, "ID", "Name", left.PartID);
            ViewData["StockID"] = new SelectList(_context.Stocks, "ID", "Address", left.StockID);
            return View(left);
        }

        // GET: Lefts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var left = await _context.Lefts.FindAsync(id);
            if (left == null)
            {
                return NotFound();
            }
            ViewData["PartID"] = new SelectList(_context.Parts, "ID", "Name", left.PartID);
            ViewData["StockID"] = new SelectList(_context.Stocks, "ID", "Address", left.StockID);
            return View(left);
        }

        // POST: Lefts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID, PartID,StockID,ArrDate,Quantity")] Left left)
        {
            if (id != left.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(left);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeftExists(left.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["PartID"] = new SelectList(_context.Parts, "ID", "Name", left.PartID);
            ViewData["StockID"] = new SelectList(_context.Stocks, "ID", "Address", left.StockID);
            return View(left);
        }

        // GET: Lefts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var left = await _context.Lefts
                .Include(l => l.Part)
                .Include(l => l.Stock)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (left == null)
            {
                return NotFound();
            }

            return View(left);
        }

        // POST: Lefts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var left = await _context.Lefts.FindAsync(id);
            _context.Lefts.Remove(left);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LeftExists(int id)
        {
            return _context.Lefts.Any(e => e.ID == id);
        }
    }
}
