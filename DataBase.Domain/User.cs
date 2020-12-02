using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Domain
{
    public class User
    {
        public int Id { get; set; }
        [Required, MaxLength(20)]
        public string Username { get; set; }
        [Required, MaxLength(20)]
        public string Password { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime RegisterDate { get; set; }
    }
}
