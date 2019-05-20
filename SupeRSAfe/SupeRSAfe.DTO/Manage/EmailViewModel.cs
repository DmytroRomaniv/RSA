using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace SupeRSAfe.DTO.Manage
{
    public class EmailViewModel
    {
        [Required]
        [Display(Name = "Receiver")]
        public string Receiver { get; set; }

        [Required]
        [Display(Name = "Subject")]
        public string Subject { get; set; }

        [Required]
        [Display(Name = "Message")]
        public string Message { get; set; }
    }
}
