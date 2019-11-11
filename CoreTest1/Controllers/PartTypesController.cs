﻿using System;
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
        /*public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewData["NameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["CurrentFilter"] = searchString;
            var stocks = from s in _context.PartTypes
                         select s;
            if (!string.IsNullOrEmpty(searchString))
            {
                stocks = stocks.Where(s => s.Name.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    stocks = stocks.OrderByDescending(s => s.Name);
                    break;
                default:
                    stocks = stocks.OrderBy(s => s.Name);
                    break;
            }
            return View(await stocks.ToListAsync());
        }*/

        public async Task<IActionResult> Index(string typeName)
        {
            if(typeName != null)
            {
                var typesList = _context.PartTypes.Where(pt => pt.Name.Contains(typeName)).ToListAsync();
                return View(await typesList);
            }
            else
            {
                return View(await _context.PartTypes.ToListAsync());
            }
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
            var partOfType = _context.Parts.FirstOrDefault(p => p.Type == id);
            if(partOfType != null)
            {
                ModelState.AddModelError("", "Існують найменування, що належать до цього типу: " + partOfType.Name);
                return View(partType);
            }
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
