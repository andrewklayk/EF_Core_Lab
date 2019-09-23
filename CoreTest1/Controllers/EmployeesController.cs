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
    public class EmployeesController : Controller
    {
        private readonly RocketContext _context;

        public EmployeesController(RocketContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewData["SurnameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "Surname_desc" : "";
            ViewData["NameSortParm"] = (sortOrder == "Name") ? "name_desc" : "Name";
            ViewData["CurrentFilter"] = searchString;
            var employees = from s in _context.Employees.Include(p => p.Positions)
                        select s;
            if (!string.IsNullOrEmpty(searchString))
            {
                employees = employees.Where(s => s.FirstName.Contains(searchString) || s.Surname.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "Surname_desc":
                    employees = employees.OrderByDescending(s => s.Surname);
                    break;
                case "Name":
                    employees = employees.OrderBy(s => s.FirstName);
                    break;
                case "name_desc":
                    employees = employees.OrderByDescending(s => s.FirstName);
                    break;
                default:
                    employees = employees.OrderBy(s => (s.FirstName + s.Surname));
                    break;
            }

            //var a = from s in _context.Lefts group s by s.PartID into r where r

            return View(await employees.ToListAsync());
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(s => s.Positions)
                    .ThenInclude(position => position.Stock)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(m => m.ID == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,Surname")] Employee employee)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(employee);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", "Виникла помилка під час збереження даних. " + ex.Message);
            }
            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,FirstName,Surname")] Employee employee)
        {
            if (id != employee.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.ID))
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
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var employee = await _context.Employees
                .FirstOrDefaultAsync(m => m.ID == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            var stockWithEmpl = _context.Stocks.FirstOrDefault(s => s.Positions.Any(p => p.EmployeeID == id));
            if (stockWithEmpl != null)
            {
                ModelState.AddModelError("", "Існують зв'язані записи у таблиці складів: " + stockWithEmpl.Address);
                return View(employee);
            }

            foreach(var pos in _context.Positions.Where(p=> p.EmployeeID == id))
            {
                _context.Remove(pos);
            }
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.ID == id);
        }
    }
}
