using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using System.Threading.Tasks;
using SupeRSAfe.BLL.Interfaces;
using SupeRSAfe.DAL.Interfaces;
using SupeRSAfe.DTO.Manage;
using Microsoft.AspNetCore.Identity;
using SupeRSAfe.DAL.Entities;

namespace SupeRSAfe.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly SignInManager<User> _signInManager;

        public UserService(IUnitOfWork unitOfWork, SignInManager<User> signInManager)
        {
            this._unitOfWork = unitOfWork;
            this._signInManager = signInManager;
        }
        public async Task<bool> CreateNewUser(RegisterViewModel registerViewModel)
        {
            var newUser = new User
            {
                Email = registerViewModel.Email,
                UserName = registerViewModel.Username,
            };

            var result = await _unitOfWork.UserManager.CreateAsync(newUser, registerViewModel.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(newUser, false);
            }
            return result.Succeeded;
        }

        public async Task<bool> DoesEmailExists(string email)
        {
            bool doesEmailExists = false;
            var user = await _unitOfWork.UserManager.FindByEmailAsync(email);
            if(user != null)
            {
                doesEmailExists = true;
            }
            return doesEmailExists;
        }

        public async Task<bool> LogIn(LoginViewModel loginViewModel)
        {
            var result = await
              _signInManager.PasswordSignInAsync(loginViewModel.Username, loginViewModel.Password, false, false);

            return result.Succeeded ? true : false;
        }

        public async void LogOut()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<User> GetUser(string email)
        {
            var user = await _unitOfWork.UserManager.FindByNameAsync(email);
            return user == null ? new User() : user;
        }
    }
}
