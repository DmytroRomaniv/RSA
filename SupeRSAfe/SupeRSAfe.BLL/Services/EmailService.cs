using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using AutoMapper;
using SupeRSAfe.BLL.Interfaces;
using SupeRSAfe.DAL.Entities;
using SupeRSAfe.DAL.Interfaces;
using SupeRSAfe.DTO.Models;


namespace SupeRSAfe.BLL.Services
{
    public class EmailService : IEmailService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmailService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public void Create(EmailDTO emailDTO)
        {
            var email = _mapper.Map<Email>(emailDTO);

            _unitOfWork.EmailRepository.Create(email);
            _unitOfWork.Save();
        }

        public void Delete(EmailDTO emailDTO)
        {
            var email = _mapper.Map<Email>(emailDTO);

            _unitOfWork.EmailRepository.Delete(email);
            _unitOfWork.Save();
        }

        public IEnumerable<EmailDTO> Find(string searchLine)
        {
            var foundEmails = _unitOfWork.EmailRepository.Find(em => em.Receiver.Email.Contains(searchLine));
            var emailDTOs = new List<EmailDTO>();

            if (foundEmails != null)
            {
                foundEmails.ToList().ForEach(em =>
                {
                    var emailDTO = _mapper.Map<EmailDTO>(em);
                    emailDTOs.Add(emailDTO);
                });
            }

            return emailDTOs;
        }

        public IEnumerable<EmailDTO> GetAll(User user)
        {
            var emails = _unitOfWork.EmailRepository.All.Where(em => em.Receiver.Email == user.Email);
            var emailDTOs = new List<EmailDTO>();

            if (emails != null)
            {
                emails.ToList().ForEach(em =>
                {
                    var emailDTO = _mapper.Map<EmailDTO>(em);
                    emailDTOs.Add(emailDTO);
                });
            }

            return emailDTOs;
        }
    }
}
