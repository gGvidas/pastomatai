using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using pastomatai.Models;

namespace pastomatai.Controllers
{
    public class PackagesController : Controller
    {
        private readonly pastomataiContext _context;

        public PackagesController(pastomataiContext context)
        {
            _context = context;
        }

        // GET: Packages
        public async Task<IActionResult> asd()
        {
            var pastomataiContext = _context.Package.Include(p => p.FkEndUseridEndUserNavigation).Include(p => p.FkLoggedInUseridEndUserNavigation).Include(p => p.FkTerminalidTerminalNavigation);
            return View(await pastomataiContext.ToListAsync());
        }
        public IActionResult Index(string searchString)
        {
            if (!String.IsNullOrEmpty(searchString))
            {
                var pastomataiContext1 = _context.Package.Include(p => p.FkEndUseridEndUserNavigation).Include(p => p.FkLoggedInUseridEndUserNavigation).Include(p => p.FkTerminalidTerminalNavigation).Where(p => p.PackageState.Contains(searchString));
                return View(pastomataiContext1.ToList());
            }
            var pastomataiContext = _context.Package.Include(p => p.FkEndUseridEndUserNavigation).Include(p => p.FkLoggedInUseridEndUserNavigation).Include(p => p.FkTerminalidTerminalNavigation);
            return View(pastomataiContext.ToList());

        }
        // GET: Packages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var package = await _context.Package
                .Include(p => p.FkEndUseridEndUserNavigation)
                .Include(p => p.FkLoggedInUseridEndUserNavigation)
                .Include(p => p.FkTerminalidTerminalNavigation)
                .FirstOrDefaultAsync(m => m.IdPackage == id);
            if (package == null)
            {
                return NotFound();
            }

            return View(package);
        }
        private IEnumerable<SelectListItem> GetSize()
        {
            return new SelectListItem[]
            {
            new SelectListItem() { Text = "", Value = "Choose size" },
            new SelectListItem() { Text = "S", Value = "Small" },
            new SelectListItem() { Text = "M", Value = "Medium" },
            new SelectListItem() { Text = "L", Value = "Large" }
            };
        }
        // GET: Packages/Create
        public IActionResult Create()
        {
            ViewData["Size"] = new SelectList(GetSize(), "Text", "Value");
            ViewData["FkEndUseridEndUser"] = new SelectList(_context.EndUser, "IdEndUser", "PhoneNumber");
            ViewData["FkLoggedInUseridEndUser"] = new SelectList(_context.LoggedInUser, "IdEndUser", "Email");
            ViewData["FkTerminalidTerminal"] = new SelectList(_context.Terminal, "IdTerminal", "PhoneNumber");
            return View();
        }

        // POST: Packages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PutInTime,CollectionTime,Size,PackageState,Email,ReceiversNumber,IdPackage,FkLoggedInUseridEndUser,FkTerminalidTerminal,FkEndUseridEndUser")] Package package)
        {
            if (ModelState.IsValid)
            {
                package.PackageState = "WaitsForCourier";
                int id = _context.Package.Max(p => p.IdPackage) + 1;
                package.IdPackage = id;
                package.PutInTime = DateTime.Now;
                _context.Add(package);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FkEndUseridEndUser"] = new SelectList(_context.EndUser, "IdEndUser", "PhoneNumber", package.FkEndUseridEndUser);
            ViewData["FkLoggedInUseridEndUser"] = new SelectList(_context.LoggedInUser, "IdEndUser", "Email", package.FkLoggedInUseridEndUser);
            ViewData["FkTerminalidTerminal"] = new SelectList(_context.Terminal, "IdTerminal", "PhoneNumber", package.FkTerminalidTerminal);
            return View(package);
        }

        // GET: Packages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var package = await _context.Package.FindAsync(id);
            if (package == null)
            {
                return NotFound();
            }
            ViewData["FkEndUseridEndUser"] = new SelectList(_context.EndUser, "IdEndUser", "PhoneNumber", package.FkEndUseridEndUser);
            ViewData["FkLoggedInUseridEndUser"] = new SelectList(_context.LoggedInUser, "IdEndUser", "Email", package.FkLoggedInUseridEndUser);
            ViewData["FkTerminalidTerminal"] = new SelectList(_context.Terminal, "IdTerminal", "PhoneNumber", package.FkTerminalidTerminal);
            return View(package);
        }

        // POST: Packages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PutInTime,CollectionTime,Size,PackageState,Email,ReceiversNumber,IdPackage,FkLoggedInUseridEndUser,FkTerminalidTerminal,FkEndUseridEndUser")] Package package)
        {
            if (id != package.IdPackage)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(package);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PackageExists(package.IdPackage))
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
            ViewData["FkEndUseridEndUser"] = new SelectList(_context.EndUser, "IdEndUser", "PhoneNumber", package.FkEndUseridEndUser);
            ViewData["FkLoggedInUseridEndUser"] = new SelectList(_context.LoggedInUser, "IdEndUser", "Email", package.FkLoggedInUseridEndUser);
            ViewData["FkTerminalidTerminal"] = new SelectList(_context.Terminal, "IdTerminal", "PhoneNumber", package.FkTerminalidTerminal);
            return View(package);
        }

        // GET: Packages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var package = await _context.Package
                .Include(p => p.FkEndUseridEndUserNavigation)
                .Include(p => p.FkLoggedInUseridEndUserNavigation)
                .Include(p => p.FkTerminalidTerminalNavigation)
                .FirstOrDefaultAsync(m => m.IdPackage == id);
            if (package == null)
            {
                return NotFound();
            }

            return View(package);
        }

        // POST: Packages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var package = await _context.Package.FindAsync(id);
            _context.Package.Remove(package);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PackageExists(int id)
        {
            return _context.Package.Any(e => e.IdPackage == id);
        }
    }
}
