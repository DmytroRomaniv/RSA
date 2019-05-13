using System;
using System.Collections.Generic;
using System.Text;
using SupeRSAfe.DAL.Entities;
using Microsoft.AspNetCore.Identity;

namespace SupeRSAfe.DAL.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<Email> EmailRepository { get; set; }

        IRepository<Key> KeyRepository { get; set; }

        UserManager<User> UserManager { get; set; }

        void Save();
    }
}
