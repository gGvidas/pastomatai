using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using pastomatai.Models;
using Microsoft.AspNetCore.Mvc;


namespace pastomatai.Controllers
{
    public class UserController
    {
        private UserController context;
        public UserController(UserController context)
        {
            this.context = context;
        }
    }
}
