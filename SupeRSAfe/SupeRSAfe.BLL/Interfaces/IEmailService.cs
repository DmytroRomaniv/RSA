using System;
using System.Collections.Generic;
using System.Text;
using SupeRSAfe.DTO.Models;
using SupeRSAfe.DAL.Entities;

namespace SupeRSAfe.BLL.Interfaces
{
    public interface IEmailService
    {
        void Create(EmailDTO email);

        void Delete(EmailDTO email);

        IEnumerable<EmailDTO> Find(string searchLine);

        IEnumerable<EmailDTO> GetAll(User user);
    }
}
