using Core.Models.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
	public class NavigationMenuViewModel
	{
        public Guid Id { get; set; }
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
        public bool Permitted { get; set; }
        public bool Visible { get; set; }
        public virtual NavigationMenuViewModel? ParentNavigationMenu { get; set; }
		
    }

    public class RoleNodes
    {
        public Guid Id { get; set; }
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
        public bool Permitted { get; set; }
        public bool Visible { get; set; }
        public string? RoleId { get; set; }
        public bool CanView { get; set; }
        public bool CanAdd { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
        public bool Active { get; set; }
        public virtual List<RoleNodes>? ChildRoleNodes { get; set; }
    }
}