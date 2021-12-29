using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ArmadilloParty.ViewModels;
using System.Linq;
using System.Security.Claims;
using Auth0.AspNetCore.Authentication;
using System;

namespace ArmadilloParty.Controllers
{
    public class EssaysController : Controller
    {
        Random random = new Random();

        public IActionResult Essays()
        {            
            var n = random.Next(0, 5);

            string[] availableEssays = { "kidnap", "tucosattack", "mytunnel","escape","confusion" };

            return View(availableEssays[n]);
        }
        
    }
}
