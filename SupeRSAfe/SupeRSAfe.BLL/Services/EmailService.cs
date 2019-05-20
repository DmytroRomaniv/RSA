﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Numerics;
using AutoMapper;
using SupeRSAfe.BLL.Interfaces;
using SupeRSAfe.DAL.Entities;
using SupeRSAfe.DAL.Interfaces;
using SupeRSAfe.DTO.Models;
using RSA.Interfaces;
using RSA;
using System.Threading.Tasks;

namespace SupeRSAfe.BLL.Services
{
    public class EmailService : IEmailService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private RsaAlgorithm _encryptionAlgorithm;

        public EmailService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public void Create(EmailDTO emailDTO, User user)
        {
            if (emailDTO == null || user == null)
                return;

            Key key;
            _encryptionAlgorithm = new RsaAlgorithm();
            var byteMessage = ConvertToByteString(emailDTO.Message.Replace("\n", ""));

            emailDTO.Message = _encryptionAlgorithm.Encrypt(byteMessage).Result;

            key = new Key
            {
                SecretKey = _encryptionAlgorithm.SecretKey.ToString(),
                OpenKey = _encryptionAlgorithm.SecretKey.ToString(),
                User = user
            };
            var email = _mapper.Map<Email>(emailDTO);
            _unitOfWork.EmailRepository.Create(email);
            _unitOfWork.KeyRepository.Create(key);
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


        public async Task<EmailDTO> DecryptEmail(EmailDTO emailDTO, KeyDTO keyDTO)
        {
            if (emailDTO == null || keyDTO == null)
                return new EmailDTO();

            _encryptionAlgorithm = new RsaAlgorithm(2, 3);
            _encryptionAlgorithm.PublicKey = keyDTO.OpenKey;
            _encryptionAlgorithm.SecretKey = keyDTO.SecretKey;

            var byteMessage = await _encryptionAlgorithm.Decrypt(emailDTO.Message);

            emailDTO.Message = ConvertToCharacters(byteMessage);

            return emailDTO;
        }

        public IEnumerable<KeyDTO> GetAllKeys(User user)
        {
            var keyCollection = _unitOfWork.KeyRepository.All.Where(k => k.User.Email == user.Email);
            var keyDTOCollection = new List<KeyDTO>();

            foreach (var key in keyCollection)
            {
                var keyDTO = _mapper.Map<KeyDTO>(key);
                keyDTOCollection.Add(keyDTO);
            }

            return keyDTOCollection;
        }

        private string ConvertToByteString(string message)
        {
            var byteMessage = Encoding.ASCII.GetBytes(message);
            var byteString = new StringBuilder();

            foreach (var byteCharacter in byteMessage)
            {
                byteString.Append(byteCharacter.ToString("D3"));
            }

            return byteString.ToString();
        }

        private string ConvertToCharacters(string message)
        {
            var messageString = new StringBuilder();
            var byteCharacters = new List<string>();
            int index = 0;

            while (index < message.Length - 2)
            {
                var byteCharacter = message.Substring(index, 3);
                byteCharacters.Add(byteCharacter);

                index = (index + 1) * 3;
            }


            foreach (var character in byteCharacters)
            {
                if (byte.TryParse(character, out var byteValue))
                {
                    var bytes = new List<byte>() { byteValue };
                    var symbol = Encoding.ASCII.GetString(bytes.ToArray());
                    messageString.Append(symbol);
                }
                else
                {
                    messageString.Append("0");
                }
            }

            return messageString.ToString();
        }
    }
}
