using System;
using System.Collections.Generic;
using System.Text;

namespace SupeRSAfe.DAL
{
    class Email
    {
        public string Sender { get; set; }
        
        public string[] Receivers { get; set; }

        public byte[] Message { get; set; }

        public DateTime Date { get; set; }
    }
}
