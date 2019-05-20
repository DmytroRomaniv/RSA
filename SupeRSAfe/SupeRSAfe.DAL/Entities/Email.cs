using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SupeRSAfe.DAL.Entities
{
    public class Email
    {
        public int Id { get; set; }

        public string Subject { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string SenderId { get; set; }

        public User Sender { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ReceiverId { get; set; }

        public User Receiver { get; set; }

        public string Message { get; set; }

        public DateTime Date { get; set; }
    }
}
