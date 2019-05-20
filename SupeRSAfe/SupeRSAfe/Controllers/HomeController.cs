using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SupeRSAfe.DTO.Manage;
using SupeRSAfe.BLL.Interfaces;
using SupeRSAfe.DTO.Models;
using SupeRSAfe.Models;
using System.Security.Claims;
using SupeRSAfe.DAL.Entities;

namespace SupeRSAfe.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmailService _emailService;
        private readonly IUserService _userService;
        public HomeController(IEmailService emailService, IUserService userService)
        {
            _emailService = emailService;
            _userService = userService;
            
        }

        public IActionResult Send()
        {
            var emailForm = new EmailViewModel();

            return View(emailForm);
        }

        [HttpPost]
        public async Task<IActionResult> Send(EmailViewModel emailForm)
        {
            if (ModelState.IsValid)
            {
                var emailDTO = await Convert(emailForm);
                
                var usr = _userService.GetUser(User.Identity.Name).Result;
                _emailService.Create(emailDTO, usr);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Index(EmailDTO chosenEmail = null, KeyDTO choosenKey = null)
        {
            var user = await _userService.GetUser(User.Identity.Name);
            var emails = _emailService.GetAll(user);

            var keys = _emailService.GetAllKeys(user);

            if(chosenEmail != null && choosenKey != null)
            {
                chosenEmail = await _emailService.DecryptEmail(chosenEmail, choosenKey);
            }

            var model = new MainViewModel
            {
                Emails = emails,
                Keys = keys,
                ChoosenKey = choosenKey,
                ChosenEmail = chosenEmail,
            };

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task<EmailDTO> Convert(EmailViewModel viewModel)
        {
            var receiver = await _userService.GetUser(viewModel.Receiver);
            var user = _userService.GetUser(User.Identity.Name).Result;
            var emailDTO = new EmailDTO
            {
                Date = DateTime.Now,
                Message = viewModel.Message,
                Receiver = receiver,
                Sender = user,
            };

            return emailDTO;
        }
    }
}
