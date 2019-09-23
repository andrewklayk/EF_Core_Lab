using CoreTest1.Data;
using CoreTest1.Models;
using CoreTest1.Models.MyViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreTest1.Controllers
{
    public class StocksController : Controller
    {
        private readonly RocketContext _context;

        public StocksController(RocketContext context)
        {
            _context = context;
        }

        // GET: Stocks
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewData["AddressSortParm"] = string.IsNullOrEmpty(sortOrder) ? "address_desc" : "";
            ViewData["CurrentFilter"] = searchString;
            var stocks = from s in _context.Stocks
                         select s;
            if (!string.IsNullOrEmpty(searchString))
            {
                stocks = stocks.Where(s => s.Address.Contains(searchString));
            }
            switch(sortOrder)
            {
                case "address_desc":
                    stocks = stocks.OrderByDescending(s => s.Address);
                    break;
                default:
                    stocks = stocks.OrderBy(s => s.Address);
                    break;
            }
            return View(await stocks.ToListAsync());
        }

        // GET: Stocks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stock = await _context.Stocks
                .Include(s => s.Positions)
                    .ThenInclude(position => position.Employee)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(m => m.ID == id);
            if (stock == null)
            {
                return NotFound();
            }

            return View(stock);
        }

        // GET: Stocks/Create
        public IActionResult Create()
        {
            var viewModel = new List<PositionData>();
            PopulateFullPositionList();
            return View();
        }

        private void PopulateFullPositionList()
        {
            List<PositionData> viewModel = new List<PositionData>();
            foreach (var employee in _context.Employees)
            {
                if (employee == null)
                {
                    continue;
                }
                {
                    viewModel.Add(new PositionData
                    {
                        EmployeeID = employee.ID,
                        Name = employee.FirstName + employee.Surname,
                    });
                }
            }
            ViewData["Positions"] = viewModel;
        }

        // POST: Stocks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Address")] string[] selectedEmployees, Stock stock)
        {
            if (_context.Stocks.Any(s => s.Address == stock.Address))
            {
                ModelState.AddModelError("Address", "Запис за такою адресою вже існує");
                stock.Positions = new List<Position>();
                UpdateStockEmployees(selectedEmployees, stock);
                PopulatePositionsData(stock);
                return View(stock);
            }
            if (ModelState.IsValid)
            {
                _context.Add(stock);
                UpdateStockEmployees(selectedEmployees, stock);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //PopulatePositionsData(stock);
            return View(stock);
        }

        // GET: Stocks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stock = await _context.Stocks
                .Include(s=>s.Positions)
                    .ThenInclude(p => p.Employee)
                    .AsNoTracking()
                    .FirstOrDefaultAsync( s => s.ID == id);
            if (stock == null)
            {
                return NotFound();
            }
            PopulatePositionsData(stock);
            return View(stock);
        }

        // POST: Stocks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, string[] selectedEmployees)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stockToUpdate = await _context.Stocks
                .Include(i => i.Positions)
                    .ThenInclude(i => i.Employee)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (await TryUpdateModelAsync<Stock>(
                stockToUpdate,
                "",
                i => i.Address))
            {
                UpdateStockEmployees(selectedEmployees, stockToUpdate);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
                return RedirectToAction(nameof(Index));
            }
            UpdateStockEmployees(selectedEmployees, stockToUpdate);
            return View(stockToUpdate);
        }

        private void UpdateStockEmployees(string[] selectedEmployees, Stock stockToUpdate)
        {
            if (!selectedEmployees.Any())
            {
                stockToUpdate.Positions = new List<Position>();
                return;
            }

            var selectedEmployeesHS = new HashSet<string>(selectedEmployees);
            var StockEmployees = new HashSet<int>
                (stockToUpdate.Positions.Select(c => c.Employee.ID));
            foreach (var Employee in _context.Employees)
            {
                if (selectedEmployeesHS.Contains(Employee.ID.ToString()))
                {
                    if (!StockEmployees.Contains(Employee.ID))
                    {
                        var pos = new Position { StockID = stockToUpdate.ID, EmployeeID = Employee.ID };
                        stockToUpdate.Positions.Add(pos);
                        if (Employee.Positions == null)
                            Employee.Positions = new List<Position>();
                        Employee.Positions.Add(pos);
                    }
                }
                else
                {

                    if (StockEmployees.Contains(Employee.ID))
                    {
                        Position EmployeesToRemove = stockToUpdate.Positions.FirstOrDefault(i => i.EmployeeID == Employee.ID);
                        _context.Remove(EmployeesToRemove);
                    }
                }
            }
        }

        private void PopulatePositionsData(Stock Stock)
        {
            var allEmployees = _context.Employees;
            var StockEmployees = new HashSet<int>(Stock.Positions.Select(c => c.EmployeeID));
            var viewModel = new List<PositionData>();
            foreach (var Employee in allEmployees)
            {
                viewModel.Add(new PositionData
                {
                    EmployeeID = Employee.ID,
                    Name = Employee.FirstName + " " + Employee.Surname,
                    Assigned = StockEmployees.Contains(Employee.ID)
                });
            }
            ViewData["Positions"] = viewModel;
        }

        // GET: Stocks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stock = await _context.Stocks
                .Include(s => s.Positions)
                .ThenInclude(p => p.Employee)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (stock == null)
            {
                return NotFound();
            }

            return View(stock);
        }

        // POST: Stocks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var stock = await _context.Stocks.FindAsync(id);
            if(stock.Positions != null && stock.Positions.Any())
            {
                foreach(var a in stock.Positions)
                {
                    _context.Positions.Remove(a);
                }
            }
            _context.Stocks.Remove(stock);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StockExists(int id)
        {
            return _context.Stocks.Any(e => e.ID == id);
        }
    }
}
