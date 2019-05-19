using System;
using System.Collections.Generic;
using System.Text;

namespace SupeRSAfe.DTO.Models
{
    public class EmailDTO
    {
        public int Id { get; set; }

        public string Sender { get; set; }

        public string Receiver { get; set; }

        public string Message { get; set; }

        public DateTime Date { get; set; }
    }
}
