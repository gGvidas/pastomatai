using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
        public async Task<IActionResult> Index(string state, bool hasterminal, string terminalId, string postMachineId, bool haspostmachine)
        {
            if (!String.IsNullOrEmpty(terminalId))
            {
                if (String.IsNullOrEmpty(state))
                {
                    var pastomataiContext = _context.Package.Include(p => p.FkEndUseridEndUserNavigation)
                                .Include(p => p.FkLoggedInUseridEndUserNavigation)
                                .Include(p => p.FkTerminalidTerminalNavigation);
                    ViewBag.FkTerminalidTerminal = GetTerminalAddressList();
                    ViewBag.PostMachines = GetPostMachineAddressList();
                    ViewData["TerminalFilter"] = terminalId;
                    return View(await pastomataiContext.ToListAsync());
                }
                else if (hasterminal == false)
                {
                    var id = Int16.Parse(terminalId);
                    var pastomataiContext = _context.Package.Include(p => p.FkEndUseridEndUserNavigation)
                        .Include(p => p.FkLoggedInUseridEndUserNavigation)
                        .Include(p => p.FkTerminalidTerminalNavigation)
                        .Where(p => p.PackageState.Contains(state))
                        .Where(p => p.FkTerminalidTerminal.HasValue.Equals(hasterminal));
                    ViewBag.FkTerminalidTerminal = GetTerminalAddressList();
                    ViewBag.PostMachines = GetPostMachineAddressList();
                    ViewData["TerminalFilter"] = terminalId;
                    return View(await pastomataiContext.ToListAsync());
                }
                else
                {
                    var id = Int16.Parse(terminalId);
                    var pastomataiContext = _context.Package.Include(p => p.FkEndUseridEndUserNavigation)
                        .Include(p => p.FkLoggedInUseridEndUserNavigation)
                        .Include(p => p.FkTerminalidTerminalNavigation)
                        .Where(p => p.PackageState.Contains(state))
                        .Where(p => p.FkTerminalidTerminal.HasValue.Equals(hasterminal))
                        .Where(p => p.FkTerminalidTerminalNavigation.IdTerminal.Equals(id));
                    ViewBag.FkTerminalidTerminal = GetTerminalAddressList();
                    ViewBag.PostMachines = GetPostMachineAddressList();
                    ViewData["TerminalFilter"] = terminalId;
                    return View(await pastomataiContext.ToListAsync());
                }

            }
            else if (!String.IsNullOrEmpty(postMachineId))
            {
                if (String.IsNullOrEmpty(state))
                {
                    var pastomataiContext = _context.Package.Include(p => p.FkEndUseridEndUserNavigation)
                                .Include(p => p.FkLoggedInUseridEndUserNavigation)
                                .Include(p => p.FkTerminalidTerminalNavigation);
                    ViewData["PostMachineFilter"] = postMachineId;
                    ViewBag.FkTerminalidTerminal = GetTerminalAddressList();
                    ViewBag.PostMachines = GetPostMachineAddressList();
                    return View(await pastomataiContext.ToListAsync());
                }
                if (haspostmachine == false)
                {
                    var id = Int16.Parse(postMachineId);
                    var pastomataiContext = _context.Package.Include(p => p.FkEndUseridEndUserNavigation)
                        .Include(p => p.FkLoggedInUseridEndUserNavigation)
                        .Include(p => p.FkTerminalidTerminalNavigation)
                        .Where(p => p.PackageState.Contains(state))
                        .Where(p => p.FkTerminalidTerminal.HasValue.Equals(hasterminal));
                    ViewBag.FkTerminalidTerminal = GetTerminalAddressList();
                    ViewBag.PostMachines = GetPostMachineAddressList();
                    ViewData["PostMachineFilter"] = postMachineId;
                    return View(await pastomataiContext.ToListAsync());

                }
                else
                {
                    var id = Int16.Parse(postMachineId);
                    var pastomataiContext = _context.Package.Include(p => p.FkEndUseridEndUserNavigation)
                        .Include(p => p.FkLoggedInUseridEndUserNavigation)
                        .Include(p => p.FkTerminalidTerminalNavigation)
                        .Where(p => p.PackageState.Contains(state))
                        .Where(p => p.FkTerminalidTerminal.HasValue.Equals(hasterminal))
                        .Where(p => p.PostMachineBox.FkPostMachineidPostMachineNavigation.IdPostMachine.Equals(id));
                    ViewBag.FkTerminalidTerminal = GetTerminalAddressList();
                    ViewBag.PostMachines = GetPostMachineAddressList();
                    ViewData["PostMachineFilter"] = postMachineId;
                    return View(await pastomataiContext.ToListAsync());
                }


            }

            else
            {
                var pastomataiContext = _context.Package.Include(p => p.FkEndUseridEndUserNavigation)
                                .Include(p => p.FkLoggedInUseridEndUserNavigation)
                                .Include(p => p.FkTerminalidTerminalNavigation);
                ViewBag.FkTerminalidTerminal = GetTerminalAddressList();
                ViewBag.PostMachines = GetPostMachineAddressList();
                return View(await pastomataiContext.ToListAsync());
            }
            

        }

        

        // GET: Delivery/UpdateState/5
        public async Task<IActionResult> UpdateState(int? id, int currentPostMachine, int currentTerminal)
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
            ViewData["PostMachineFilter"] = currentPostMachine;
            ViewData["TerminalFilter"] = currentTerminal;
            return View(package);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateState(int id, int currentPostMachine, int currentTerminal, [Bind("PutInTime,CollectionTime,Size,PackageState,IdPackage,FkLoggedInUseridEndUser,FkTerminalidTerminal,FkEndUseridEndUser")] Package package)
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
                        var postbox = getEmptyByPostMachine(currentPostMachine);
                        package.PostMachineBox = postbox;
                        if (package.PostMachineBox == null)
                        {
                            // post machine is full 
                            return View(package);
                        }
                    }
                    // place in terminal
                    else if (package.PackageState.Contains("EnRoute") && !package.FkTerminalidTerminal.HasValue)
                    {
                        package.PackageState = "InTerminal";
                        // TODO: package is into terminal with id = 20 <-- change it later
                        package.FkTerminalidTerminal = currentTerminal;
                    }

                    //Paprastam useriui

                    if (package.PackageState.Contains("Created"))
                    { package.PackageState = "Activated"; }
                    else if (package.PackageState.Contains("Activated"))
                    { 
                        package.PackageState = "WaitsForCourier";
                        var postbox = getEmptyByPostMachine(currentPostMachine);
                        package.PostMachineBox = postbox;
                        if(package.PostMachineBox == null)
                        {
                            // post machine is full
                            return View(package);
                        }
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
        public ActionResult UpdateStatesAll(int[] idsToUpdate, int currentPostMachine, int currentTerminal)
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
                // take out from terminal
                else if (package.PackageState.Contains("InTerminal"))
                { package.PackageState = "EnRoute"; }
                // place in post machine
                else if (package.PackageState.Contains("EnRoute") && package.FkTerminalidTerminal.HasValue)
                {
                    package.PackageState = "WaitsForPickup";
                    var postbox = getEmptyByPostMachine(currentPostMachine);
                    package.PostMachineBox = postbox;
                    if (package.PostMachineBox == null)
                    {
                        // post machine is full 
                        return View(package);
                    }
                }
                // place in terminal
                else if (package.PackageState.Contains("EnRoute") && !package.FkTerminalidTerminal.HasValue)
                {
                    package.PackageState = "InTerminal";
                    // TODO: package is into terminal with id = 20 <-- change it later
                    package.FkTerminalidTerminal = currentTerminal;
                }


                _context.Update(package);           
            }
            _context.SaveChanges();
            return View(packages);
        }

        
        public async Task<IActionResult> ConfirmDetails(int? id, int currentPostMachine, int currentTerminal)
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
            ViewData["PostMachineFilter"] = currentPostMachine;
            ViewData["TerminalFilter"] = currentTerminal;
            return View(package);
        }

        [HttpGet]
        public async Task<IActionResult> PinConfirmation(int? id, int currentPostMachine, int currentTerminal)
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
            ViewData["PostMachineFilter"] = currentPostMachine;
            ViewData["TerminalFilter"] = currentTerminal;
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
        public async Task<IActionResult> PinConfirmation(int? packageId, string pin, int currentPostMachine, int currentTerminal)
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

        private SelectList GetTerminalAddressList()
        {
            SelectList temp =
               new SelectList((from s in _context.Address.ToList().Where(s => s.Terminal != null)
                               select new
                               {
                                   IdAddress = s.IdAddress,
                                   Adress = "id " + s.IdAddress + " - " + s.Street + " " + s.HouseNumber + ", " + s.City + " " + s.ZipCode
                               }),
                   "IdAddress",
                   "Adress",
                   null);
            return temp;
        }

        private SelectList GetPostMachineAddressList()
        {
            var tmp = GetAddressIds();
            SelectList temp =
               new SelectList((from s in _context.Address.ToList().Where(s => tmp.Contains(s.IdAddress) == true)
                               select new
                               {
                                   IdAddress = s.IdAddress,
                                   Adress = "id " + s.IdAddress + " - " + s.Street + " " + s.HouseNumber + ", " + s.City + " " + s.ZipCode
                               }),
                   "IdAddress",
                   "Adress",
                   null);
            return temp;
        }

        private List<int> GetAddressIds()
        {
            var newList = new List<int>();
            var postmachines = _context.PostMachine;
            foreach (PostMachine postmachine in postmachines)
            {
                newList.Add(postmachine.FkAddressidAddress);
            }
            return newList;
        }

        private PostMachineBox getEmptyByPostMachine(int id)
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
    }
}
