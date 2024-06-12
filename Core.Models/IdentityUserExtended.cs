using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class IdentityUserExtended : IdentityUser
    {
        [DisplayName("Approved?")]
        public bool IsApproved { get; set; } = false;
        [DisplayName("Online?")]
        public bool IsOnline { get; set; } = false;
        [DisplayName("SystemAdmin?")]
        public bool IsSystemAdmin { get; set; } = false;
        [DisplayName("Last Login")]
        public DateTime? LastLoginDate { get; set; }
        [DisplayName("Last Logout")]
        public DateTime? LastLogoutDate { get; set; }
        [DisplayName("Activity Duration")]
        public int OnlineActiveDurationInMs { get; set; } = 0;
        [MaxLength(500)]
        public string? Comment { get; set; }
    }
}
