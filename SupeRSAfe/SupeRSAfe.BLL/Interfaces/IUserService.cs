using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SupeRSAfe.DTO.Manage;
using SupeRSAfe.DAL.Entities;

namespace SupeRSAfe.BLL.Interfaces
{
    public interface IUserService
    {
        Task<bool> DoesEmailExists(string email);

        Task<bool> CreateNewUser(RegisterViewModel registerViewModel);

        Task<bool> LogIn(LoginViewModel loginViewModel);

        void LogOut();

        Task<User> GetUser(string email);
    }
}
