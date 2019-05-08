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
    public class PartTypesController : Controller
    {
        private readonly RocketContext _context;

        public PartTypesController(RocketContext context)
        {
            _context = context;
        }

        // GET: PartTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.PartTypes.ToListAsync());
        }

        // GET: PartTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var partType = await _context.PartTypes
                .FirstOrDefaultAsync(m => m.ID == id);
            if (partType == null)
            {
                return NotFound();
            }

            return View(partType);
        }

        // GET: PartTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PartTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Units")] PartType partType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(partType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(partType);
        }

        // GET: PartTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var partType = await _context.PartTypes.FindAsync(id);
            if (partType == null)
            {
                return NotFound();
            }
            return View(partType);
        }

        // POST: PartTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Units")] PartType partType)
        {
            if (id != partType.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(partType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PartTypeExists(partType.ID))
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
            return View(partType);
        }

        // GET: PartTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var partType = await _context.PartTypes
                .FirstOrDefaultAsync(m => m.ID == id);
            if (partType == null)
            {
                return NotFound();
            }

            return View(partType);
        }

        // POST: PartTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var partType = await _context.PartTypes.FindAsync(id);
            _context.PartTypes.Remove(partType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PartTypeExists(int id)
        {
            return _context.PartTypes.Any(e => e.ID == id);
        }
    }
}
