using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BanThietBiDiDongDATN.Data.Enums;

namespace BanThietBiDiDongDATN.Data.entities
{
    public class AppUser : IdentityUser<Guid>
    {
        [MaxLength(200)]
        [Required]
        [Column(TypeName = "nvarchar(200)")]
        public string FirstName { get; set; }
        [MaxLength(200)]
        [Required]
        [Column(TypeName = "nvarchar(200)")]
        public string LastName { get; set; }
        public DateTime Dob { get; set; }
        [MaxLength(250)]
        [Column(TypeName = "nvarchar(250)")]
        public string? Avatar { get; set; }
        [MaxLength(250)]
        [Column(TypeName = "nvarchar(250)")]
        public string Address { get; set; }
        public bool Gender { get; set; }
        //public StatusUser status { get; set; }
        public DateTime? LastSignIn { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public List<Cart> Carts { get; set; }
        public List<Order> Orders { get; set; }

    }
}
