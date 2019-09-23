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
    public class PartsController : Controller
    {
        private readonly RocketContext _context;

        public PartsController(RocketContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewData["NameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "Name_desc" : "";
            ViewData["CurrentFilter"] = searchString;
            var parts = from s in _context.Parts.Include(p=>p.PartType)
                         select s;
            if (!string.IsNullOrEmpty(searchString))
            {
                parts = parts.Where(s => s.Name.Contains(searchString));
            }
            switch(sortOrder)
            {
                case "Name_desc":
                    parts = parts.OrderByDescending(s => s.Name);
                    break;
                default:
                    parts = parts.OrderBy(s => s.Name);
                    break;
            }
            return View(await parts.ToListAsync());
        }

        // GET: Parts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var part = await _context.Parts
                .FirstOrDefaultAsync(m => m.ID == id);
            if (part == null)
            {
                return NotFound();
            }

            return View(part);
        }

        // GET: Parts/Create
        public IActionResult Create()
        {
            PopulateTypesDropDownList();
            return View();
        }

        // POST: Parts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Type")] Part part)
        {
            if (ModelState.IsValid)
            {
                _context.Add(part);
                part.PartType = _context.PartTypes.Where(pt => pt.ID == part.Type).FirstOrDefault();
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(part);
        }

        // GET: Parts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var part = await _context.Parts.FindAsync(id);
            if (part == null)
            {
                return NotFound();
            }
            PopulateTypesDropDownList();
            return View(part);
        }

        // POST: Parts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Type")] Part part)
        {
            if (id != part.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    part.PartType = _context.PartTypes.Where(pt => pt.ID == part.Type).FirstOrDefault();
                    _context.Update(part);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PartExists(part.ID))
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
            PopulateTypesDropDownList();
            return View(part);
        }

        private void PopulateTypesDropDownList(object selectedType = null)
        {
            var CustomersQuery = from c in _context.PartTypes
                                 orderby c.Name
                                 select c;
            ViewBag.TypeID = new SelectList(CustomersQuery, "ID", "Name", selectedType);
        }

        // GET: Parts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var part = await _context.Parts.Include(p => p.PartType)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (part == null)
            {
                return NotFound();
            }

            return View(part);
        }

        // POST: Parts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var part = await _context.Parts.FindAsync(id);
            if(_context.Lefts.Any(c=>c.PartID == id))
            {
                ModelState.AddModelError("", "Неможливо видалити: існують залежні записи");
                return View(part);
            }
            _context.Parts.Remove(part);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PartExists(int id)
        {
            return _context.Parts.Any(e => e.ID == id);
        }
    }
}
