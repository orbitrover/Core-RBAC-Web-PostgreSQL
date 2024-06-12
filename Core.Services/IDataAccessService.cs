using Core.Models;
using Core.Models.Data;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Core.Services
{
    public interface IDataAccessService
    {
        Task<List<NavigationMenuViewModel>> GetAllMenuControlAsync(string filterOption = "All");
        Task<NavigationMenuViewModel> GetMenuControlByIdAsync(string Id);
        Task<bool> AddMenuControlByIdAsync(NavigationMenu model);
        Task<bool> EditMenuControlByIdAsync(NavigationMenu model);
        Task<bool> DeleteMenuControlByIdAsync(NavigationMenu model);
        Task<bool> GetMenuItemsAsync(ClaimsPrincipal ctx, string? ctrl, string? act);
        Task<List<NavigationMenuViewModel>> GetMenuItemsAsync(ClaimsPrincipal principal);
        Task<List<NavigationMenuViewModel>> GetPermissionsByRoleIdAsync(string? id);
        Task<List<NavigationMenuViewModel>> GetPermissionsByRoleGroupIdAsync(string? id);
		Task<bool> SetPermissionsByRoleIdAsync(string? id, IEnumerable<Guid> permissionIds);
        Task MakeUserStatusOnline(SignInManager<IdentityUserExtended> _signInManager, bool IsOnline, string _userName);
        Task<List<DemoUsers>> GetDetmoUsers();
    }
}
