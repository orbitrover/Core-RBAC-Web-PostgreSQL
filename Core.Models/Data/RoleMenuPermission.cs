using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Data
{
    [Table(name: "AspNetRoleMenuPermission")]
    public class RoleMenuPermission : BaseEntity
    {
        public string? RoleId { get; set; }
        public Guid NavigationMenuId { get; set; }
        public bool? CanView { get; set; }
        public bool? CanAdd { get; set; }
        public bool? CanEdit { get; set; }
        public bool? CanDelete { get; set; }
        public bool? Active { get; set; }
        public NavigationMenu? NavigationMenu { get; set; }
    }
}
