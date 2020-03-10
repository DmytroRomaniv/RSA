using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using SupeRSAfe.DTO.Models;

namespace SupeRSAfe.DTO.Manage
{
    public class MainViewModel
    {
        public IEnumerable<EmailDTO> Emails { get; set; }

        public EmailDTO ChosenEmail { get; set; }

        public IEnumerable<KeyDTO> Keys { get; set; }

        public KeyDTO ChoosenKey { get; set; }
        
        public HttpPostedFileBase File { get; set; }
    }
}
