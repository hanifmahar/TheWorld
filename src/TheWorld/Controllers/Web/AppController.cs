using Microsoft.AspNet.Mvc;
using System;
using System.Linq;
using TheWorld.Models;
using TheWorld.Services;
using TheWorld.ViewModels;

namespace TheWorld.Controllers.Web
{
    public class AppController:Controller
    {
        private IMailService _mailService;
        private IWorldRepository _repository;

        public AppController(IMailService sevice, IWorldRepository repository)
        {
            _mailService = sevice;
            _repository = repository;
        }
        public IActionResult Index()
        {
            var trips = _repository.GetAllTrips();

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