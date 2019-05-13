using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using System.Threading.Tasks;
using SupeRSAfe.BLL.Interfaces;
using SupeRSAfe.DAL.Interfaces;
using SupeRSAfe.DTO.Manage;

namespace SupeRSAfe.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        public Task<bool> CreateNewUser(RegisterViewModel registerViewModel)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DoesEmailExists(string email)
        {
            throw new NotImplementedException();
        }

        public Task<bool> LogIn(LoginViewModel loginViewModel)
        {
            throw new NotImplementedException();
        }

        public void LogOut()
        {
            throw new NotImplementedException();
        }
    }
}
