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

        [Display(Name = "Q value for encoding")]
        public string QValue { get; set; }

        [Display(Name = "P value for encoding")]
        public string PValue { get; set; }

        [Display(Name="Generate random values for keys")]
        public bool UseRandomValues { get; set; }
    }
}
