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
    public class DeliveryController : Controller
    {
        private readonly pastomataiContext _context;

        public DeliveryController(pastomataiContext context)
        {
            _context = context;
        }

        //GET: Delivery
        public async Task<IActionResult> Index(string state, bool hasterminal)
        {
            if (!String.IsNullOrEmpty(state))
            {
                var pastomataiContext = _context.Package.Include(p => p.FkEndUseridEndUserNavigation)
                .Include(p => p.FkLoggedInUseridEndUserNavigation)
                .Include(p => p.FkTerminalidTerminalNavigation)
                .Where(p => p.PackageState.Contains(state))
                .Where(p=> p.FkTerminalidTerminal.HasValue.Equals(hasterminal));

                return View(await pastomataiContext.ToListAsync());
            }
            else
            {
                var pastomataiContext = _context.Package.Include(p => p.FkEndUseridEndUserNavigation)
                                .Include(p => p.FkLoggedInUseridEndUserNavigation)
                                .Include(p => p.FkTerminalidTerminalNavigation);
                return View(await pastomataiContext.ToListAsync());
            }

        }

        // GET: Delivery/UpdateState/5
        public async Task<IActionResult> UpdateState(int? id)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateState(int id, [Bind("PutInTime,CollectionTime,Size,PackageState,IdPackage,FkLoggedInUseridEndUser,FkTerminalidTerminal,FkEndUseridEndUser")] Package package)
        {
            if (id != package.IdPackage)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (package.PackageState.Contains("WaitsForCourier"))
                    { package.PackageState = "EnRoute"; }
                    else if (package.PackageState.Contains("InTerminal"))
                    { package.PackageState = "EnRoute"; }
                    //else if (package.PackageState.Contains("WaitsForPickup"))
                    //{ package.PackageState = "Delivered"; }
                    else if (package.PackageState.Contains("EnRoute") && package.FkTerminalidTerminal.HasValue)
                    { package.PackageState = "WaitsForPickup"; }
                    else if (package.PackageState.Contains("EnRoute") && !(package.FkTerminalidTerminal.HasValue))
                    { package.PackageState = "InTerminal"; }

                    //Paprastam useriui

                    if (package.PackageState.Contains("Created"))
                    { package.PackageState = "Activated"; }
                    else if (package.PackageState.Contains("Activated"))
                    { package.PackageState = "WaitsForCourier"; }
                    else if (package.PackageState.Contains("WaitsForPickup"))
                    { package.PackageState = "Delivered"; }

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

        [HttpPost]
        public ActionResult UpdateStatesAll(int[] idsToUpdate)
        {
            List<Package> packages = new List<Package>(idsToUpdate.Length);
            for (int i = 0; i < idsToUpdate.Length; i++)
            {
                packages.Add(_context.Package.Find(idsToUpdate[i]));
            }

            foreach (var package in packages)
            {
                if (package.PackageState.Contains("WaitsForCourier"))
                { package.PackageState = "EnRoute"; }
                else if (package.PackageState.Contains("InTerminal"))
                { package.PackageState = "EnRoute"; }
                //else if (package.PackageState.Contains("WaitsForPickup"))
                //{ package.PackageState = "Delivered"; }
                else if (package.PackageState.Contains("EnRoute") && package.FkTerminalidTerminal.HasValue)
                { package.PackageState = "WaitsForPickup"; }
                else if (package.PackageState.Contains("EnRoute") && !(package.FkTerminalidTerminal.HasValue))
                { package.PackageState = "InTerminal"; }

                //Paprastam useriui

                if (package.PackageState.Contains("Created"))
                { package.PackageState = "Activated"; }
                else if (package.PackageState.Contains("Activated"))
                { package.PackageState = "WaitsForCourier"; }
                else if (package.PackageState.Contains("WaitsForPickup"))
                { package.PackageState = "Delivered"; }

                _context.Update(package);           
            }
             _context.SaveChangesAsync();
            return View(packages);
        }

        private bool PackageExists(int id)
        {
            return _context.Package.Any(e => e.IdPackage == id);
        }
        public async Task<IActionResult> ConfirmDetails(int? id)
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
    }
}
