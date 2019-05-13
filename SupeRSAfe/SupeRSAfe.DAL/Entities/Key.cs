using System;
using System.Collections.Generic;
using System.Text;

namespace SupeRSAfe.DAL.Entities
{
    public class Key
    {
        public int Id { get; set; }
        public User User { get; set; }

        public long SecretKey { get; set; }

        public long OpenKey { get; set; }
    }
}
