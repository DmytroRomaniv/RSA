using System;
using System.Collections.Generic;
using System.Text;

namespace SupeRSAfe.DTO.Models
{
    [Serializable]
    public class KeyDTO
    {
        public int Id { get; set; }

        public string SecretKey { get; set; }

        public string OpenKey { get; set; }
    }
}
