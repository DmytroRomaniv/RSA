using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using SupeRSAfe.DAL.Entities;
using SupeRSAfe.DAL.Interfaces;
using SupeRSAfe.DAL.Context;
using SupeRSAfe.DAL.Repositories;

namespace SupeRSAfe.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MailDbContext _dbContext;

        private IRepository<Email> _emailRepository;
        public IRepository<Email> EmailRepository
        {
            get
            {
                if(_emailRepository == null)
                {
                    _emailRepository = new Repository<Email>(_dbContext);
                }
                return _emailRepository;
            }
            set
            {
                _emailRepository = value ?? new Repository<Email>(_dbContext);
            }
        }

        private IRepository<Key> _keyRepository;
        public IRepository<Key> KeyRepository
        {
            get
            {
                if (_keyRepository == null)
                {
                    _keyRepository = new Repository<Key>(_dbContext);
                }
                return _keyRepository;
            }
            set
            {
                _keyRepository = value ?? new Repository<Key>(_dbContext);
            }
        }

        public UserManager<User> UserManager { get; set; }

        public UnitOfWork(MailDbContext dbContext, UserManager<User> userManager)
        {
            _dbContext = dbContext;
            UserManager = userManager;
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}
