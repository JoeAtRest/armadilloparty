using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SampleMvcApp.ViewModels;
using System.Linq;
using System.Security.Claims;
using Auth0.AspNetCore.Authentication;
using System;

namespace SampleMvcApp.Controllers
{
    public class EssaysController : Controller
    {
        Random random = new Random();

        public IActionResult Essays()
        {            
            var n = random.Next(0, 4);

            string[] availableEssays = { "kidnap", "tucosattack", "mytunnel","escape" };

            return View(availableEssays[n]);
        }
        
    }
}
