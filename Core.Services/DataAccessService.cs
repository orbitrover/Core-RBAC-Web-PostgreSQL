using Core.Models;
using Core.Models.Data;
using Core.Repo;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class DataAccessService : IDataAccessService
    {
        private readonly IMemoryCache _cache;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUserExtended> _userManager;
        public DataAccessService(UserManager<IdentityUserExtended> userManager, ApplicationDbContext context, IMemoryCache cache)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<List<NavigationMenuViewModel>> GetAllMenuControlAsync(string filterOption = "All")
        {
            List<NavigationMenuViewModel> navigationMenuViewModels = new List<NavigationMenuViewModel>();
            List<NavigationMenu> menuControls = null;
            if (filterOption == "All")
                menuControls = await _context.NavigationMenu.ToListAsync();
            else
                menuControls = await _context.NavigationMenu.Where(x => x.ActionName.StartsWith(filterOption)).ToListAsync();
            string jsonData = JsonConvert.SerializeObject(menuControls);
            navigationMenuViewModels = JsonConvert.DeserializeObject<List<NavigationMenuViewModel>>(jsonData);

            return navigationMenuViewModels;
        }
        public async Task<NavigationMenuViewModel> GetMenuControlByIdAsync(string Id)
        {
            NavigationMenuViewModel navigationMenuViewModel = new NavigationMenuViewModel();
            var menuContol = await _context.NavigationMenu.Where(x => x.Id.ToString() == Id).SingleOrDefaultAsync();
            string jsonData = JsonConvert.SerializeObject(menuContol);
            navigationMenuViewModel = JsonConvert.DeserializeObject<NavigationMenuViewModel>(jsonData);

            return navigationMenuViewModel;
        }
        public async Task<bool> AddMenuControlByIdAsync(NavigationMenu model)
        {
            try
            {
                _context.Add(model);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> EditMenuControlByIdAsync(NavigationMenu model)
        {
            try
            {
                _context.Update(model);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> DeleteMenuControlByIdAsync(NavigationMenu model)
        {
            try
            {
                _context.Remove(model);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<List<NavigationMenuViewModel>> GetMenuItemsAsync(ClaimsPrincipal principal)
        {
            var isAuthenticated = principal.Identity?.IsAuthenticated;
            if (isAuthenticated == false)
            {
                return new List<NavigationMenuViewModel>();
            }

            var roleIds = await GetUserRoleIds(principal);

            var permissions = await _cache.GetOrCreateAsync("Permissions",
                async x => await (from menu in _context.NavigationMenu select menu).ToListAsync());

            var rolePermissions = await _cache.GetOrCreateAsync("RolePermissions",
                async x => await (from menu in _context.RoleMenuPermission select menu).Include(x => x.NavigationMenu).ToListAsync());

            var data = (from menu in rolePermissions
                        join p in permissions on menu.NavigationMenuId equals p.Id
                        where roleIds.Contains(menu.RoleId)
                        select p)
                              .Select(m => new NavigationMenuViewModel()
                              {
                                  Id = m.Id,
                                  Name = m.Name,
                                  Area = m.Area,
                                  Visible = m.Visible,
                                  IsExternal = m.IsExternal,
                                  ActionName = m.ActionName,
                                  ExternalUrl = m.ExternalUrl,
                                  DisplayOrder = m.DisplayOrder,
                                  ParentMenuId = m.ParentMenuId,
                                  ControllerName = m.ControllerName,
                              }).Distinct().ToList();

            return data;
        }

        public async Task<bool> GetMenuItemsAsync(ClaimsPrincipal ctx, string? ctrl, string? act)
        {
            var result = false;
            var roleIds = await GetUserRoleIds(ctx);
            var data = await (from menu in _context.RoleMenuPermission
                              where roleIds.Contains(menu.RoleId)
                              select menu)
                              .Select(m => m.NavigationMenu)
                              .Distinct()
                              .ToListAsync();

            foreach (var item in data)
            {
                result = (item.ControllerName == ctrl && item.ActionName == act);
                if (result)
                {
                    break;
                }
            }

            return result;
        }

        public async Task<List<NavigationMenuViewModel>> GetPermissionsByRoleIdAsync(string? id)
        {
            var items = await (from m in _context.NavigationMenu
                               join rm in _context.RoleMenuPermission
                                on new { X1 = m.Id, X2 = id } equals new { X1 = rm.NavigationMenuId, X2 = rm.RoleId }
                                into rmp
                               from rm in rmp.DefaultIfEmpty()
                               select new NavigationMenuViewModel()
                               {
                                   Id = m.Id,
                                   Name = m.Name,
                                   Area = m.Area,
                                   ActionName = m.ActionName,
                                   ControllerName = m.ControllerName,
                                   IsExternal = m.IsExternal,
                                   ExternalUrl = m.ExternalUrl,
                                   DisplayOrder = m.DisplayOrder,
                                   ParentMenuId = m.ParentMenuId,
                                   Visible = m.Visible,
                                   Permitted = rm.RoleId == id,
                               })
                               .AsNoTracking()
                               .ToListAsync();

            return items;
        }
        public async Task<List<NavigationMenuViewModel>> GetPermissionsByRoleGroupIdAsync(string? id)
        {
            var parentRoleId = (await _context.Roles.Where(x => x.Id == id).SingleOrDefaultAsync())?.ParentRoleId;
            //var items = await (from m in _context.NavigationMenu
            //				   join rm in _context.RoleMenuPermission
            //				   on m.Id equals rm.NavigationMenuId into rmp
            //				   from rm in rmp.DefaultIfEmpty()
            //				   where rm != null && rm.RoleId == parentRoleId
            //				   select new NavigationMenuViewModel()
            //				   {
            //					   Id = m.Id,
            //					   Name = m.Name,
            //					   Area = m.Area,
            //					   ActionName = m.ActionName,
            //					   ControllerName = m.ControllerName,
            //					   IsExternal = m.IsExternal,
            //					   ExternalUrl = m.ExternalUrl,
            //					   DisplayOrder = m.DisplayOrder,
            //					   ParentMenuId = m.ParentMenuId,
            //					   Visible = m.Visible,
            //					   Permitted = rm != null && rm.RoleId == id,
            //				   })
            //	   .AsNoTracking()
            //	   .ToListAsync();
            //var items = await (from m in _context.NavigationMenu
            //						 join rm in _context.RoleMenuPermission on m.Id equals rm.NavigationMenuId
            //						 where rm.RoleId == parentRoleId
            //						 select new NavigationMenuViewModel() {
            //							 Id = m.Id,
            //							 Name = m.Name,
            //							 Area = m.Area,
            //							 ActionName = m.ActionName,
            //							 ControllerName = m.ControllerName,
            //							 IsExternal = m.IsExternal,
            //							 ExternalUrl = m.ExternalUrl,
            //							 DisplayOrder = m.DisplayOrder,
            //							 ParentMenuId = m.ParentMenuId,
            //							 Visible = m.Visible,
            //							 Permitted = rm != null && rm.RoleId == id,
            //						 }).AsNoTracking()
            //						 .ToListAsync();

            var items = await (from m in _context.NavigationMenu
                               where _context.RoleMenuPermission
                                     .Where(rm => rm.RoleId == parentRoleId)
                                     .Select(rm => rm.NavigationMenuId)
                                     .Distinct()
                                     .Contains(m.Id)
                               select new NavigationMenuViewModel()
                               {
                                   Id = m.Id,
                                   Name = m.Name,
                                   Area = m.Area,
                                   ActionName = m.ActionName,
                                   ControllerName = m.ControllerName,
                                   IsExternal = m.IsExternal,
                                   ExternalUrl = m.ExternalUrl,
                                   DisplayOrder = m.DisplayOrder,
                                   ParentMenuId = m.ParentMenuId,
                                   Visible = m.Visible,
                                   Permitted = _context.RoleMenuPermission.Any(rm => rm.NavigationMenuId == m.Id && rm.RoleId == id) ? true : false
                               }).AsNoTracking()
                                     .ToListAsync();


            return items;
        }
        public async Task<List<RoleNodes>> GetNodesByRoleIdAsync(string? id)
        {
            var items = await (from m in _context.NavigationMenu
                               join rm in _context.RoleMenuPermission
                                on new { X1 = m.Id, X2 = id } equals new { X1 = rm.NavigationMenuId, X2 = rm.RoleId }
                                into rmp
                               from rm in rmp.DefaultIfEmpty()
                               select new RoleNodes()
                               {
                                   Id = m.Id,
                                   Name = m.Name,
                                   Area = m.Area,
                                   ActionName = m.ActionName,
                                   ControllerName = m.ControllerName,
                                   IsExternal = m.IsExternal,
                                   ExternalUrl = m.ExternalUrl,
                                   DisplayOrder = m.DisplayOrder,
                                   ParentMenuId = m.ParentMenuId,
                                   Visible = m.Visible,
                                   Permitted = rm.RoleId == id,
                                   CanView = Convert.ToBoolean(rm.CanView),
                                   CanAdd = Convert.ToBoolean(rm.CanAdd),
                                   CanEdit = Convert.ToBoolean(rm.CanEdit),
                                   CanDelete = Convert.ToBoolean(rm.CanDelete),
                                   Active = Convert.ToBoolean(rm.Active)
                               })
                               .AsNoTracking()
                               .ToListAsync();

            return items;
        }

        public async Task<bool> SetPermissionsByRoleIdAsync(string? id, IEnumerable<Guid> permissionIds)
        {
            if (string.IsNullOrWhiteSpace(id)) return false;

            var existing = await _context.RoleMenuPermission.Where(x => x.RoleId == id).ToListAsync();
            _context.RemoveRange(existing);

            foreach (var item in permissionIds)
            {
                await _context.RoleMenuPermission.AddAsync(new RoleMenuPermission()
                {
                    RoleId = id,
                    NavigationMenuId = item,
                });
            }

            var result = await _context.SaveChangesAsync();

            // Remove existing permissions to roles so it can re evaluate and take effect
            _cache.Remove("RolePermissions");

            return result > 0;
        }

        private async Task<List<string>> GetUserRoleIds(ClaimsPrincipal ctx)
        {
            var userId = GetUserId(ctx);
            var data = await (from role in _context.UserRoles
                              where role.UserId == userId
                              select role.RoleId).ToListAsync();

            return data;
        }

        private static string? GetUserId(ClaimsPrincipal user) =>
            (user.Identity) == null ? string.Empty : ((ClaimsIdentity)user.Identity).FindFirst(ClaimTypes.NameIdentifier)?.Value;

        public async Task MakeUserStatusOnline(SignInManager<IdentityUserExtended> _signInManager, bool IsOnline, string _userName)
        {
            try
            {
                var user = await _signInManager.UserManager.Users.Where(x => x.UserName == _userName).SingleOrDefaultAsync();
                user.IsOnline = IsOnline;
                if (IsOnline)
                {
                    user.LastLoginDate = DateTime.UtcNow;
                    user.LastLogoutDate = null;
                    user.OnlineActiveDurationInMs = 0;
                }
                else
                {
                    user.LastLogoutDate = DateTime.UtcNow;
                    TimeSpan timespan = (TimeSpan)(user.LastLogoutDate - user.LastLoginDate);
                    user.OnlineActiveDurationInMs = (int)timespan.TotalMilliseconds;
                }
                _context.Users.Update(user);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                //return false;
            }
        }

        public async Task<List<DemoUsers>> GetDetmoUsers()
        {
            List<DemoUsers> demoUsers = new List<DemoUsers>();
            var users = await _context.Users.ToListAsync();
            int count = 1;
            foreach (var item in users)
            {
                var userRoles = await _userManager.GetRolesAsync(item);
                var demoUser = new DemoUsers()
                {
                    Id = count,
                    UserId = item.Id,
                    UserName = item.UserName,
                    Email = item.Email,
                    Password = "P@ssw0rd",
                    RoleGroup = userRoles?[0],
                    Role = userRoles?[0].ToLower().Contains("client") == true ? "ClientAdmin" : "DevAdmin"
                };
                count++;
                demoUsers.Add(demoUser);
            }
            return demoUsers;
        }
    }
}
