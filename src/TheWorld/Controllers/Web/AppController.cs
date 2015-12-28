using Microsoft.AspNet.Mvc;
using System;
using TheWorld.Services;
using TheWorld.ViewModels;

namespace TheWorld.Controllers.Web
{
    public class AppController:Controller
    {
        private IMailService _mailService;

        public AppController(IMailService sevice)
        {
            _mailService = sevice;
        }
        public IActionResult Index()
        {
            return View();
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
            var email = Startup.Confgiuration["AppSettings:SiteEmailAddress"];

            _mailService.SendMail(email, email,
                $"Contact Page from {model.Email }",model.Message );
            return View();
        }
    }
}