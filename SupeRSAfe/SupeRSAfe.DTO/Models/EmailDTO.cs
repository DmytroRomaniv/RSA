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

        public long[] Message { get; set; }

        public DateTime Date { get; set; }
    }
}
