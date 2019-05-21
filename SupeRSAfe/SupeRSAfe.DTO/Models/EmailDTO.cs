using System;
using System.Collections.Generic;
using System.Text;
using SupeRSAfe.DAL.Entities;

namespace SupeRSAfe.DTO.Models
{
    [Serializable]
    public class EmailDTO
    {
        public int Id { get; set; }

        public User Sender { get; set; }

        public User Receiver { get; set; }

        public string Subject { get; set; }

        public string Message { get; set; }

        public DateTime Date { get; set; }
    }
}
