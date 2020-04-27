using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using pastomatai.Models;
using Microsoft.EntityFrameworkCore;

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
    }
}