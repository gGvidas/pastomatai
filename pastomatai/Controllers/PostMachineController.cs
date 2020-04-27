using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Index()
        {
            return View(context.PostMachine.ToList());
        }
    }
}