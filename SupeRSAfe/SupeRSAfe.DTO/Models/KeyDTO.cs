using System;
using System.Collections.Generic;
using System.Text;

namespace SupeRSAfe.DTO.Models
{
    public class KeyDTO
    {
        public int Id { get; set; }

        public long SecretKey { get; set; }

        public long OpenKey { get; set; }
    }
}
