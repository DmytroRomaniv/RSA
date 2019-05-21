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
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;

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

        public async Task<IActionResult> Index()
        {
            var name = User.Identity.Name;
            var user = await _userService.GetUser(User.Identity.Name);
            var emails = _emailService.GetAll(user);
            var chosenEmail = TempData.Get<EmailDTO>("chosenEmail");
            var chosenKey = TempData.Get<KeyDTO>("chosenKey");

            var keys = _emailService.GetAllKeys(user);

            if(chosenEmail != null && chosenKey != null)
            {
                chosenEmail = await _emailService.DecryptEmail(chosenEmail, chosenKey);
            }

            var model = new MainViewModel
            {
                Emails = emails,
                Keys = keys,
                ChoosenKey = chosenKey,
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
                Subject = viewModel.Subject
            };

            return emailDTO;
        }

        public async Task<IActionResult> ChoseEmail(int id)
        {
            var user = await _userService.GetUser(User.Identity.Name);
            var email = _emailService.GetAll(user).Where(m => id == m.Id).FirstOrDefault();

            TempData.Put("chosenEmail", email);
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> ChoseKey(int id)
        {
            var user = await _userService.GetUser(User.Identity.Name);
            var key = _emailService.GetAllKeys(user).Where(k => id == k.Id).FirstOrDefault();

            TempData.Put("chosenKey", key);
            return RedirectToAction("Index", "Home");
        }
    }

    public static class TempDataExtensions
    {
        public static void Put<T>(this ITempDataDictionary tempData, string key, T value) where T : class
        {
            tempData[key] = JsonConvert.SerializeObject(value);
        }

        public static T Get<T>(this ITempDataDictionary tempData, string key) where T : class
        {
            object o;
            tempData.TryGetValue(key, out o);
            return o == null ? null : JsonConvert.DeserializeObject<T>((string)o);
        }
    }
}
