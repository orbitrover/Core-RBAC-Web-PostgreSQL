using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Data
{
    [Table(name: "AspNetNavigationMenu")]
    public class NavigationMenu : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [ForeignKey(nameof(ParentNavigationMenu))]
        public Guid? ParentMenuId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? IconClass { get; set; }
        public string? Area { get; set; }
        public string? ControllerName { get; set; }
        public string? ActionName { get; set; }
        public bool IsExternal { get; set; }
        public string? ExternalUrl { get; set; }
        public int DisplayOrder { get; set; }
        public int ChildNodeCount { get; set; }
        [NotMapped]
        public bool Permitted { get; set; }
        public bool Visible { get; set; }
        public virtual NavigationMenu? ParentNavigationMenu { get; set; }
    }
}
