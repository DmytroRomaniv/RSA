using System;
using System.Collections.Generic;
using System.Text;

namespace SupeRSAfe.DAL.Entities
{
    public class Email
    {
        public int Id { get; set; }
        public string Sender { get; set; }
        
        public User Receiver { get; set; }

        public string Message { get; set; }

        public DateTime Date { get; set; }
    }
}
