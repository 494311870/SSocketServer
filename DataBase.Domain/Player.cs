using SServer.Common.Specification;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataBase.Domain
{
    public class Player
    {
        public int Id { get; set; }
        [Required, MaxLength(20)]
        public string PlayerName { get; set; }

        [Required, Range(1, 99)]
        public int Level { get; set; }

        public PlayerClass PlayerClass { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreateDate { get; set; }
    }

    
}
