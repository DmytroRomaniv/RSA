using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SupeRSAfe.DAL.Entities
{
    public class Key
    {
        public int Id { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string UserId { get; set; }
        public User User { get; set; }

        public string SecretKey { get; set; }

        public string OpenKey { get; set; }
    }
}
