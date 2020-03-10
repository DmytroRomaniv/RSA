using System;
using System.Collections.Generic;
using System.Text;
using SupeRSAfe.DTO.Models;
using SupeRSAfe.DAL.Entities;
using System.Threading.Tasks;

namespace SupeRSAfe.BLL.Interfaces
{
    public interface IEmailService
    {
        byte[] Create(EmailDTO emailDTO, User user);

        byte[] Create(EmailDTO emailDTO, User user, string pValue, string qValue);

        void Delete(EmailDTO email);

        IEnumerable<EmailDTO> Find(string searchLine);

        IEnumerable<EmailDTO> GetAll(User user);

        Task<EmailDTO> DecryptEmail(EmailDTO emailDTO, KeyDTO keyDTO);

        IEnumerable<KeyDTO> GetAllKeys(User user);
    }
}
