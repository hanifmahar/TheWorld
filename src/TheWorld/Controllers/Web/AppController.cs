using Microsoft.AspNet.Mvc;
using System;
using System.Linq;
using Theworld.Models;
using TheWorld.Services;
using TheWorld.ViewModels;

namespace TheWorld.Controllers.Web
{
    public class AppController:Controller
    {
        private IMailService _mailService;
        private WorldContext _context;

        public AppController(IMailService sevice, WorldContext context)
        {
            _mailService = sevice;
            _context = context;
        }
        public IActionResult Index()
        {
            var trips = _context.Trips.OrderBy(t => t.Name).ToList();

            return View(trips);
        }
        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact( ContactViewModel model )
        {
            if (ModelState.IsValid)
            {
                var email = Startup.Confgiuration["AppSettings:SiteEmailAddress"];

                if (string.IsNullOrWhiteSpace(email))
                {
                    ModelState.AddModelError("", "Could not send email, configuration problem");
                }

                if (_mailService.SendMail(email, email,
                      $"Contact Page from {model.Email }", model.Message))
                {
                    ModelState.Clear();

                    ViewBag.Message = "Mail sent. Thanks!";

                }
            }

            return View();
        }
    }
}