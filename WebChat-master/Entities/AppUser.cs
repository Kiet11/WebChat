using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebChat.Entities
{
    [Table("AppUser")]
    [Index("Username", IsUnique = true)]
    public class AppUser
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Username { get; set; }

        [Required]
        [MaxLength(200)]
        public byte[] PasswordHash { get; set; }

        [Required]
        [MaxLength(200)]
        public byte[] PasswordSalt { get; set; }

        [Required]
        [MaxLength(200)]
        public string FullName { get; set; }

        public DateTime? CreateDate { get; set; }   // => Allow null

    }
}
