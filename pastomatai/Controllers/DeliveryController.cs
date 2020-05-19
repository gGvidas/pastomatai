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
                    // take out from post machine
                    if (package.PackageState.Contains("WaitsForCourier"))
                    { package.PackageState = "EnRoute"; }
                    // take out from terminal
                    else if (package.PackageState.Contains("InTerminal"))
                    { package.PackageState = "EnRoute"; }
                    // place in post machine
                    else if (package.PackageState.Contains("EnRoute") && package.FkTerminalidTerminal.HasValue)
                    {
                        package.PackageState = "WaitsForPickup";
                        // TODO: package is put into post machine box with id = 666 <-- change it later
                        package.PostMachineBox = _context.PostMachineBox.Find(666);
                    }
                    // place in terminal
                    else if (package.PackageState.Contains("EnRoute") && !package.FkTerminalidTerminal.HasValue)
                    {
                        package.PackageState = "InTerminal";
                        // TODO: package is into terminal with id = 20 <-- change it later
                        package.FkTerminalidTerminal = 20;
                    }

                    //Paprastam useriui

                    if (package.PackageState.Contains("Created"))
                    { package.PackageState = "Activated"; }
                    else if (package.PackageState.Contains("Activated"))
                    { 
                        package.PackageState = "WaitsForCourier";
                        package.PostMachineBox = _context.PostMachineBox.Find(666);
                    }
                    else if (package.PackageState.Contains("WaitsForPickup"))
                    { 
                        package.PackageState = "Delivered";
                        package.PostMachineBox = null;
                    }                   

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
                // TODO for testing
                var currentTerminal = 20;

                if (package.PackageState.Contains("WaitsForCourier"))
                { package.PackageState = "EnRoute"; }
                else if (package.PackageState.Contains("InTerminal"))
                { package.PackageState = "EnRoute"; }
                else if (package.PackageState.Contains("EnRoute") && package.FkTerminalidTerminal.HasValue)
                { 
                    package.PackageState = "WaitsForPickup";
                    var postbox = getEmptyByTerminal(currentTerminal);
                    package.PostMachineBox = postbox;
                    if (package.PostMachineBox == null)
                    {
                        // post machine is full 
                        return View(packages);
                    }
                }
                else if (package.PackageState.Contains("EnRoute") && !(package.FkTerminalidTerminal.HasValue))
                { package.PackageState = "InTerminal"; }
                _context.Update(package);           
            }
             _context.SaveChangesAsync();
            return View(packages);
        }

        private PostMachineBox getEmptyByTerminal(int id)
        {
            var postmb = _context.PostMachineBox;
            foreach (PostMachineBox pmb in postmb)
            {
                if (pmb.FkPostMachineidPostMachine == id)
                {
                    if (pmb.FkPackageidPackage == null)
                    {
                        return pmb;
                    }
                }
            }
            return null;
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

        [HttpGet]
        public async Task<IActionResult> PinConfirmation(int? id)
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
            int pmbId = getPostMachineId(package);
            if (pmbId == -1)
            {
                return NotFound();
            }
            package.PostMachineBox = await _context.PostMachineBox.FindAsync(pmbId);
            ViewData["FkEndUseridEndUser"] = new SelectList(_context.EndUser, "IdEndUser", "PhoneNumber", package.FkEndUseridEndUser);
            ViewData["FkLoggedInUseridEndUser"] = new SelectList(_context.LoggedInUser, "IdEndUser", "Email", package.FkLoggedInUseridEndUser);
            ViewData["FkTerminalidTerminal"] = new SelectList(_context.Terminal, "IdTerminal", "PhoneNumber", package.FkTerminalidTerminal);
            ViewData["PostMachineBox"] = new SelectList(_context.PostMachineBox, "FkPackageidPackage", "Pin", package.PostMachineBox);

            return View(package);
        }

        private int getPostMachineId(Package package)
        {
            var postmb = _context.PostMachineBox;
            foreach (PostMachineBox pmb in postmb)
            {
                if (pmb.FkPackageidPackage == package.IdPackage)
                    return pmb.IdPostMachineBox;
            }
            return -1;
        }

        [HttpPost]
        public async Task<IActionResult> PinConfirmation(int? packageId, string pin)
        {
            if (packageId == null)
            {
                return NotFound();
            }
            var package = await _context.Package.FindAsync(packageId);

            int pmbId = getPostMachineId(package);
            if (pmbId == -1)
            {
                return NotFound();
            }
            package.PostMachineBox = await _context.PostMachineBox.FindAsync(pmbId);
            var correct = checkPin(package, pin);
            if (correct == true)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        if (package.PackageState.Contains("WaitsForPickup"))
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
                }
                return RedirectToAction("Index", package);
            }
            else
                // try again if pin is incorrect
                return RedirectToAction("PinConfirmation");
        }

        private bool checkPin(Package package, string pin)
        {
            if (package.PostMachineBox.Pin.Contains(pin))
                return true;
            return false;
        }
    }
}
