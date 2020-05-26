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
    public class PostMachineController : Controller
    {
        private pastomataiContext context;
        public PostMachineController(pastomataiContext context)
        {
            this.context = context;
        }
        public IActionResult Index(string searchString)
        {
            if (!String.IsNullOrEmpty(searchString))
            {
                var postMachineList = context.PostMachine.Include(postMachine => postMachine.FkAddressidAddressNavigation)
                    .Include(postMachine => postMachine.FkLoggedInUseridEndUser1Navigation)
                    .Include(postMachine => postMachine.FkLoggedInUseridEndUserNavigation)
                    .Where(postMachine => postMachine.FkAddressidAddressNavigation.City.Contains(searchString));
                return View(postMachineList);
            }
            return View(context.PostMachine.Include(postMachine => postMachine.FkAddressidAddressNavigation)
                    .Include(postMachine => postMachine.FkLoggedInUseridEndUser1Navigation)
                    .Include(postMachine => postMachine.FkLoggedInUseridEndUserNavigation).ToList());
        }

        [NonAction]
        public Address GetPostMachine(int id)
        {
            return context.Address.Where(s => s.IdAddress == 3).FirstOrDefault();
        }
        // GET: Packages/Create
        public IActionResult Create()
        {
            ViewData["FkAddressidAddress"] = GetAddressList();
            ViewData["FkLoggedInUseridEndUser"] = GetUseridEndUser("Courier");
            ViewData["FkLoggedInUseridEndUser1"] = GetUseridEndUser("Worker");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("TurnedOn,PostMachineState,IdPostMachine,FkLoggedInUseridEndUser,FkLoggedInUseridEndUser1,FkAddressidAddress")] PostMachine package)
        {
            if (ModelState.IsValid)
            {
                package.TurnedOn = false;
                package.PostMachineState = "WaitsForMaintenance";
                int id = context.PostMachine.Max(p => p.IdPostMachine) + 1;
                package.IdPostMachine = id;
                context.Add(package);
                context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FkAddressidAddress"] = GetAddressList();
            ViewData["FkLoggedInUseridEndUser"] = GetUseridEndUser("Courier");
            ViewData["FkLoggedInUseridEndUser1"] = GetUseridEndUser("Worker");

            return View(package);
        }


        [HttpGet]
        public ActionResult Edit(int id)
        {
            PostMachine postMachine = context.PostMachine.Single(emp => emp.IdPostMachine == id);
            ViewData["FkAddressidAddress"] = GetAddressList();
            ViewData["FkLoggedInUseridEndUser"] = GetUseridEndUser("Courier");
            ViewData["FkLoggedInUseridEndUser1"] = GetUseridEndUser("Worker");

            return View(postMachine);
        }

        [HttpPost]
        public ActionResult Edit(PostMachine postMachine)
        {

            if (ModelState.IsValid)
            {
                context.Update(postMachine);
                context.SaveChangesAsync(); // not async works
                return RedirectToAction(nameof(Index));
            }
            return View(postMachine);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            //PostMachine postMachine = context.PostMachine.Single(emp => emp.IdPostMachine == id);
            if (id == null)
            {
                return NotFound();
            }

            var package = await context.PostMachine
                .Include(p => p.FkAddressidAddressNavigation)
                .Include(p => p.FkLoggedInUseridEndUserNavigation)
                .Include(p => p.FkAddressidAddressNavigation)
                .FirstOrDefaultAsync(m => m.IdPostMachine == id);
            if (package == null)
            {
                return NotFound();
            }


            return View(package);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var package = await context.PostMachine.FindAsync(id);

            context.PostMachine.Remove(package);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));


            //context.Remove(postMachine);
            //context.SaveChangesAsync(); // not async works
            //return RedirectToAction(nameof(Index));
        }

        private SelectList GetUseridEndUser(string role)
        {
            SelectList temp = new SelectList((from s in context.LoggedInUser.Where(s => s.Role == role).ToList()
                            select new
                            {
                                IdUser = s.IdEndUser,
                                User = "id " + s.IdEndUser + " - " + "(" + s.Role + ") " + s.Email
                            }),
                    "IdUser",
                    "User",
                    null);
            return temp;
        }
        private SelectList GetAddressList()
        {
            SelectList temp =
               new SelectList((from s in context.Address.ToList()
                               select new
                               {
                                   IdAddress = s.IdAddress,
                                   Adress = "id " + s.IdAddress + " - " + s.City + ", " + s.Street + " (ZIP: " + s.ZipCode + ")"
                               }),
                   "IdAddress",
                   "Adress",
                   null);
            return temp;
        }
    }
}