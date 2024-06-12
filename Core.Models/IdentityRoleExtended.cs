using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class IdentityRoleExtended : IdentityRole
    {
        [ForeignKey(nameof(IdentityRoleExtended))]
        public string? ParentRoleId { get; set; }
        public string CreatedBy { get; set; } = "1";
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool? IsActive { get; set; } = true;
        public virtual IdentityRoleExtended? ParentRole { get; set; }
    }

    
}
