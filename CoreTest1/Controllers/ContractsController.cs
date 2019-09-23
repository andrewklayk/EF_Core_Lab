using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoreTest1.Data;
using CoreTest1.Models;
using CoreTest1.Models.MyViewModel;

namespace CoreTest1.Controllers
{
    public class ContractsController : Controller
    {
        private readonly RocketContext _context;

        public ContractsController(RocketContext context)
        {
            _context = context;
        }

        // GET: Contracts
        public async Task<IActionResult> Index(int? id, int? partID)
        {
            var rocketContext = _context.Contracts.Include(c => c.Customer)
                .Include(c=>c.PartsInContr)
                    .ThenInclude(pc=>pc.Part)
                    .ThenInclude(p=>p.PartType);
            return View(await rocketContext.ToListAsync());
        }

        // GET: Contracts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contract = await _context.Contracts
                .Include(c => c.Customer)
                .Include(c => c.PartsInContr)
                    .ThenInclude(pc => pc.Part)
                    .ThenInclude(p => p.PartType).AsNoTracking()
                    .FirstOrDefaultAsync(m => m.ID == id);
            if (contract == null)
            {
                return NotFound();
            }

            return View(contract);
        }

        // GET: Contracts/Create
        public IActionResult Create()
        {
            PopulateCustomersDropDownList();
            var viewModel = new List<PartInConData>();
            foreach (var part in _context.Parts)
            {
                if (part == null)
                {
                    continue;
                }
                {
                    viewModel.Add(new PartInConData
                    {
                        PartID = part.ID,
                        PartName = part.Name,
                        PartTypeName = _context.PartTypes.First(pt => pt.ID == part.Type).Name,
                        Assigned = false
                    });
                }
            }
            ViewData["Parts"] = viewModel;
            return View();
        }

        // POST: Contracts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerID,SignDate")] string[] selectedParts, int[] quantity, Contract contract)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contract);
                contract.PartsInContr = new List<PartInContract>();
                UpdateContractParts(selectedParts, quantity, contract);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["CustomerID"] = new SelectList(_context.Customers, "ID", "ID", contract.CustomerID);
            //PopulateCustomersDropDownList();
            return View();
        }

        // GET: Contracts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contract = await _context.Contracts
                .Include(c => c.Customer)
                .Include(c => c.PartsInContr)
                    .ThenInclude(p => p.Part)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (contract == null)
            {
                return NotFound();
            }
            PopulatePartsData(contract);
            PopulateCustomersDropDownList();
            return View(contract);
        }

        // POST: Contracts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, string[] selectedParts, int[] quantity)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ContractToUpdate = await _context.Contracts
                .Include(i => i.PartsInContr)
                    .ThenInclude(i => i.Part)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (await TryUpdateModelAsync<Contract>(
                ContractToUpdate,
                "",
                c => c.SignDate, c => c.CustomerID))
            {
                UpdateContractParts(selectedParts, quantity, ContractToUpdate);
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
            UpdateContractParts(selectedParts, quantity, ContractToUpdate);
            PopulatePartsData(ContractToUpdate);
            return View(ContractToUpdate);
        }

        private void PopulateCustomersDropDownList(object selectedCustomer = null)
        {
            var CustomersQuery = from c in _context.Customers
                                   orderby c.Name
                                   select c;
            ViewBag.CustomerID = new SelectList(CustomersQuery, "ID", "Name", selectedCustomer);
            ViewData["Customers"] = CustomersQuery;
        }

        private void PopulatePartsData(Contract contract)
        {
            var allParts = _context.Parts;
            var ContractParts = new HashSet<int>(contract.PartsInContr.Select(c => c.PartID));
            var viewModel = new List<PartInConData>();
            foreach (var part in allParts)
            {
                if(part == null)
                {
                    continue;
                }
                if (contract.PartsInContr.Any(p => p.PartID == part.ID))
                {
                    viewModel.Add(new PartInConData
                    {
                        PartID = part.ID,
                        Quantity = contract.PartsInContr.First(p => p.PartID == part.ID).Quantity,
                        PartName = part.Name,
                        PartTypeName = _context.PartTypes.First(pt => pt.ID == part.Type).Name,
                        Assigned = ContractParts.Contains(part.ID)
                    });
                }
                else
                {
                    viewModel.Add(new PartInConData
                    {
                        PartID = part.ID,
                        PartName = part.Name,
                        PartTypeName = _context.PartTypes.First(pt => pt.ID == part.Type).Name,
                        Assigned = ContractParts.Contains(part.ID)
                    });
                }
            }
            ViewData["Parts"] = viewModel;
        }
        
        private void UpdateContractParts(string[] selectedParts, int[] quantity, Contract contractToUpdate)
        {
            if (selectedParts == null)
            {
                contractToUpdate.PartsInContr = new List<PartInContract>();
                return;
            }
            var selectedPartsHS = new HashSet<string>(selectedParts);
            HashSet<int> contractParts = new HashSet<int>();
            if (contractToUpdate.PartsInContr != null)
            {
                contractParts = new HashSet<int>
                    (contractToUpdate.PartsInContr.Select(c => c.Part.ID));
            }
            int counter = 0;
            foreach (var Part in _context.Parts)
            {
                if (selectedPartsHS.Contains(Part.ID.ToString()))
                {
                    if (!contractParts.Contains(Part.ID))
                    {
                        contractToUpdate.PartsInContr.Add(new PartInContract { ContractID = contractToUpdate.ID, PartID = Part.ID, Quantity = quantity[counter] });
                    }
                    else if (contractToUpdate.PartsInContr.ElementAt(counter).Quantity != quantity[counter])
                    {
                        contractToUpdate.PartsInContr.ElementAt(counter).Quantity = quantity[counter];
                    }
                }
                else
                {

                    if (contractParts.Contains(Part.ID))
                    {
                        PartInContract PartsToRemove = contractToUpdate.PartsInContr.FirstOrDefault(i => i.PartID == Part.ID);
                        _context.Remove(PartsToRemove);
                    }
                }
                counter++;
            }
        }

        // GET: Contracts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contract = await _context.Contracts
                .Include(c => c.Customer)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (contract == null)
            {
                return NotFound();
            }

            return View(contract);
        }

        // POST: Contracts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contract = await _context.Contracts.FindAsync(id);
            foreach (var ci in _context.ContractItems.Where(p => p.ContractID == id))
            {
                _context.ContractItems.Remove(ci);
            }                    
            _context.Contracts.Remove(contract);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContractExists(int id)
        {
            return _context.Contracts.Any(e => e.ID == id);
        }
    }
}
