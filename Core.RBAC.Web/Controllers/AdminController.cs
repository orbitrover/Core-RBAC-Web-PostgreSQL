using Core.Models;
using Core.Models.Data;
using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SQLitePCL;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Runtime.InteropServices.JavaScript;
using System.Security.Claims;

namespace Core.RBAC.Web.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly IDataAccessService _dataAccessService;
        private readonly UserManager<IdentityUserExtended> _userManager;
        private readonly RoleManager<IdentityRoleExtended> _roleManager;

        public AdminController(
                UserManager<IdentityUserExtended> userManager,
                RoleManager<IdentityRoleExtended> roleManager,
                IDataAccessService dataAccessService,
                ILogger<AdminController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            _dataAccessService = dataAccessService ?? throw new ArgumentNullException(nameof(dataAccessService));
        }
        public async Task<IActionResult> Index()
        {
            var demoUsers = await _dataAccessService.GetDetmoUsers();
            return View(demoUsers);
        }
        #region Roles
        [Authorize("Authorization")]
        public async Task<IActionResult> Roles()
        {
            var roleViewModel = new List<RoleViewModel>();
            //var roles = await _roleManager.Roles.Where(x => string.IsNullOrEmpty(x.ParentRoleId.ToString()) == true).ToListAsync();
            //foreach (var item in roles)
            //{
            //    roleViewModel.Add(new RoleViewModel()
            //    {
            //        Id = item.Id,
            //        Name = item.Name,
            //    });
            //}

            return View(roleViewModel);
        }
        //[Authorize("Authorization")]
        [ActionName("RoleList")]
        public async Task<IActionResult> _RoleList(string filterOption = "All")
        {
            var roleViewModel = new List<RoleViewModel>();
            var roles = new List<IdentityRoleExtended>();
            try
            {
                if (filterOption == "All")
                    roles = await _roleManager.Roles.Where(x => string.IsNullOrEmpty(x.ParentRoleId) == true).ToListAsync();
                else
                    roles = await _roleManager.Roles.Where(x => string.IsNullOrEmpty(x.ParentRoleId) == true && x.Name.StartsWith(filterOption)).ToListAsync();
                string userJson = JsonConvert.SerializeObject(roles);
                roleViewModel = JsonConvert.DeserializeObject<List<RoleViewModel>>(userJson);
            } catch(Exception ex) 
            {
                _logger.LogError($"Exception: {ex.Message}");
            }
            return PartialView("_RoleList", roleViewModel);
        }
        // GET: Master/CreateRole
        [HttpGet]
        [Authorize("Roles")]
        [ActionName("CreateRole")]
        public IActionResult _CreateRole()
        {
            return PartialView("_CreateRole", new RoleViewModel());
        }

        // POST: Master/CreateRole
        [HttpPost]
        [Authorize("Roles")]
        [ActionName("CreateRole")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _CreateRole(RoleViewModel viewModel)
        {
            var jsonResponse = new { msg = "", msgType = "", Url = "" };
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _roleManager.CreateAsync(new IdentityRoleExtended() { Name = viewModel.Name, ParentRoleId = null });
                    if (result.Succeeded)
                    {
                        return Json(new { msg = "Role Created Successfully", msgType = "success", Url = "/Admin/RoleList" });
                    }
                    else
                    {
                        return Json(new { msg = string.Join(",", result.Errors), msgType = "error", Url = "/Admin/RoleList" });
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { msg = "Exception: " + ex.Message, msgType = "error", Url = "/Admin/RoleList" });
            }

            return Json(new { msg = "Failed", msgType = "success", Url = "/Admin/RoleList" });
        }

        // GET: Master/EditRole
        [HttpGet]
        [ActionName("EditRole")]
        public async Task<IActionResult> _EditRole(string id)
        {
            var role = await _roleManager.Roles.Where(x => x.Id == id).SingleOrDefaultAsync();
            var jsonString = JsonConvert.SerializeObject(role);
            RoleViewModel viewModel = JsonConvert.DeserializeObject<RoleViewModel>(jsonString);
            return PartialView("_EditRole", viewModel);
        }

        // POST: Master/EditRole
        [HttpPost]
        [ActionName("EditRole")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _EditRole([Bind] RoleViewModel viewModel)
        {
            try
            {
                var result = await _roleManager.UpdateAsync(new IdentityRoleExtended() { Id = viewModel.Id, Name = viewModel.Name, ParentRoleId = null });
                if (result.Succeeded)
                {
                    return Json(new { msg = "Role Updated Successfully", msgType = "success", Url = "/Admin/RoleList" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { msg = "Exception: " + ex.Message, msgType = "error", Url = "/Admin/RoleList" });
            }
            return Json(new { msg = "Updated Failed", msgType = "failed", Url = "/Admin/RoleList" });
        }
        // POST: Master/DeleteRole
        [HttpGet]
        public async Task<IActionResult> _DeleteRole(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }
            var role = await _roleManager.Roles.Where(x => x.Id == id).SingleOrDefaultAsync();
            return PartialView("_DeleteRole", role);
        }
        // POST: Master/DeleteRole
        [HttpPost]
        public async Task<IActionResult> _DeleteRole(RoleViewModel viewModel)
        {
            try
            {
                var result = await _roleManager.DeleteAsync(new IdentityRoleExtended() { Id = viewModel.Id, Name = viewModel.Name, ParentRoleId = null });
                if (result.Succeeded)
                {
                    return Json(new { msg = "Role Deleted Successfully", msgType = "success", Url = "/Admin/RoleList" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { msg = "Exception: " + ex.Message, msgType = "error", Url = "/Preps/TopicList" });
            }
            return Json(new { msg = "Role Deleted Successfully", msgType = "success", Url = "/Admin/RoleList" });
        }
        #endregion;

        #region Role_Groups
        public async Task<IActionResult> GetRoleDropDownList()
        {
            var roleViewModel = new List<RoleViewModel>();
            var role = await _roleManager.Roles.Where(x => string.IsNullOrEmpty(x.ParentRoleId) == true).ToListAsync();
            var selectList = new SelectList(role, "Id", "Name");
            return Json(selectList);
        }

        public async Task<IActionResult> GetRoleGroupDropDownList(string id)
        {
            var roleViewModel = new List<RoleViewModel>();
            var role = await _roleManager.Roles.Where(x => string.IsNullOrEmpty(x.ParentRoleId) == false && x.ParentRoleId.Contains(id)).ToListAsync();
            var selectList = new SelectList(role, "Id", "Name");
            return Json(selectList);
        }


        [Authorize("Authorization")]
        public async Task<IActionResult> RoleGroups(string id)
        {
            var roleViewModel = new List<RoleViewModel>();
            var roles = await _roleManager.Roles.Where(x => string.IsNullOrEmpty(x.ParentRoleId) == true).ToListAsync();
            FilterAlphabates();
            ViewData["Id"] = id;
            ViewData["RoleName"] = new SelectList(roles, "Id", "Name", id);
            return View(roleViewModel);
        }
        [Authorize("Authorization")]
        [ActionName("RoleGroupList")]
        public async Task<IActionResult> _RoleGroupList(string id, string filterOption = "All")
        {
            var roleViewModel = new List<RoleViewModel>();
            var roles = new List<IdentityRoleExtended>();
            if (filterOption == "All")
                roles = await _roleManager.Roles.Where(x => string.IsNullOrEmpty(x.ParentRoleId) == false && (id == null || x.ParentRoleId.Contains(id))).ToListAsync();
            else
                roles = await _roleManager.Roles.Where(x => string.IsNullOrEmpty(x.ParentRoleId) == false && (id == null || x.ParentRoleId.Contains(id)) && x.Name.StartsWith(filterOption)).ToListAsync();
            string userJson = JsonConvert.SerializeObject(roles);
            roleViewModel = JsonConvert.DeserializeObject<List<RoleViewModel>>(userJson);

            return PartialView("_RoleGroupList", roleViewModel);
        }
        // GET: Master/CreateRole
        [HttpGet]
        [Authorize("Authorization")]
        [ActionName("CreateRoleGroup")]
        public IActionResult _CreateRoleGroup()
        {
            var parentRole = _roleManager.Roles.Where(x => string.IsNullOrEmpty(x.ParentRoleId) == true).ToListAsync().Result;
            ViewData["ParentRoleId"] = new SelectList(parentRole, "Id", "Name");
            return PartialView("_CreateRoleGroup", new RoleViewModel());
        }

        // POST: Master/CreateRole
        [HttpPost]
        [Authorize("Authorization")]
        [ActionName("CreateRoleGroup")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _CreateRoleGroup(RoleViewModel viewModel)
        {
            var jsonResponse = new { msg = "", msgType = "", Url = "" };
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _roleManager.CreateAsync(new IdentityRoleExtended() { Name = viewModel.Name, ParentRoleId = viewModel.ParentRoleId });
                    if (result.Succeeded)
                    {
                        return Json(new { msg = "Role Created Successfully", msgType = "success", Url = "/Admin/RoleGroupList" + viewModel.ParentRoleId });
                    }
                    else
                    {
                        return Json(new { msg = string.Join(",", result.Errors), msgType = "error", Url = "/Admin/RoleGroupList" + viewModel.ParentRoleId });
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { msg = "Exception: " + ex.Message, msgType = "error", Url = "/Admin/RoleGroupList" + viewModel.ParentRoleId });
            }

            return Json(new { msg = "Failed", msgType = "failed", Url = "/Admin/RoleGroupList" + viewModel.ParentRoleId });
        }

        // GET: Master/EditRole
        [HttpGet]
        [Authorize("Authorization")]
        [ActionName("EditRoleGroup")]
        public async Task<IActionResult> _EditRoleGroup(string id)
        {


            var role = await _roleManager.Roles.Where(x => x.Id == id).SingleOrDefaultAsync();
            var jsonString = JsonConvert.SerializeObject(role);
            RoleViewModel viewModel = JsonConvert.DeserializeObject<RoleViewModel>(jsonString);
            var parentRole = await _roleManager.Roles.Where(x => string.IsNullOrEmpty(x.ParentRoleId) == true).ToListAsync();
            ViewData["ParentRoleId"] = new SelectList(parentRole, "Id", "Name", role.ParentRoleId);
            return PartialView("_EditRoleGroup", viewModel);
        }

        // POST: Master/EditRole
        [HttpPost]
        [Authorize("Authorization")]
        [ActionName("EditRoleGroup")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _EditRoleGroup([Bind] RoleViewModel viewModel)
        {
            try
            {
                var result = await _roleManager.UpdateAsync(new IdentityRoleExtended() { Id = viewModel.Id, Name = viewModel.Name, ParentRoleId = viewModel.ParentRoleId });
                if (result.Succeeded)
                {
                    return Json(new { msg = "Group Updated Successfully", msgType = "success", Url = "/Admin/RoleGroupList" + viewModel.ParentRoleId });
                }
            }
            catch (Exception ex)
            {
                return Json(new { msg = "Exception: " + ex.Message, msgType = "error", Url = "/Admin/RoleGroupList" + viewModel.ParentRoleId });
            }
            return Json(new { msg = "Updated Failed", msgType = "failed", Url = "/Admin/RoleGroupList" + viewModel.ParentRoleId });
        }
        // POST: Master/DeleteRole
        [HttpGet]
        [Authorize("Authorization")]
        public async Task<IActionResult> _DeleteRoleGroup(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }
            var role = await _roleManager.Roles.Where(x => x.Id == id).SingleOrDefaultAsync();
            return PartialView("_DeleteRoleGroup", role);
        }
        // POST: Master/DeleteRole
        [HttpPost]
        [Authorize("Authorization")]
        public async Task<IActionResult> _DeleteRoleGroup(RoleViewModel viewModel)
        {
            try
            {
                var result = await _roleManager.DeleteAsync(new IdentityRoleExtended() { Id = viewModel.Id, Name = viewModel.Name, ParentRoleId = viewModel.ParentRoleId });
                if (result.Succeeded)
                {
                    return Json(new { msg = "Group Deleted Successfully", msgType = "success", Url = "/Admin/RoleGroupList/" + viewModel.ParentRoleId });
                }
            }
            catch (Exception ex)
            {
                return Json(new { msg = "Exception: " + ex.Message, msgType = "error", Url = "/Preps/RoleGroupList" + viewModel.ParentRoleId });
            }
            return Json(new { msg = "Group Deleted Successfully", msgType = "failed", Url = "/Admin/RoleGroupList" + viewModel.ParentRoleId });
        }
        #endregion;

        #region Users

        [Authorize("Authorization")]
        public async Task<IActionResult> Users()
        {
            FilterAlphabates();
            return View();
        }

        //[Authorize("Authorization")]
        [HttpGet]
        [ActionName("UserList")]
        public async Task<IActionResult> _UserList(string filterOption = "All")
        {
            var userViewModel = new List<UserViewModel>();
            var users = new List<IdentityUserExtended>();
            if (filterOption == "All")
                users = await _userManager.Users.ToListAsync();
            else
                users = await _userManager.Users.Where(x => x.UserName.StartsWith(filterOption)).ToListAsync();
            string userJson = JsonConvert.SerializeObject(users);
            userViewModel = JsonConvert.DeserializeObject<List<UserViewModel>>(userJson);

            return PartialView("_UserList", userViewModel);
        }

        [HttpGet]
        [ActionName("UserListByRoleId")]
        public async Task<IActionResult> _UserList(string rolename, string filterOption = "All")
        {
            var userViewModel = new List<UserViewModel>();
            var users = new List<IdentityUserExtended>();
            //var roleusers = _userManager.GetUsersInRoleAsync(rolename).Result;
            if (filterOption == "All")
                users = _userManager.GetUsersInRoleAsync(rolename).Result.ToList();
            else
                users = _userManager.GetUsersInRoleAsync(rolename).Result.Where(x => x.UserName.StartsWith(filterOption)).ToList();
            string userJson = JsonConvert.SerializeObject(users);
            userViewModel = JsonConvert.DeserializeObject<List<UserViewModel>>(userJson);

            return PartialView("_UserList", userViewModel);
        }

        [Authorize("Users")]
        [ActionName("EditUser")]
        public async Task<IActionResult> EditUser(string id)
        {
            var viewModel = new UserViewModel();
            if (!string.IsNullOrWhiteSpace(id))
            {
                var user = await _userManager.FindByIdAsync(id);
                var userRoles = await _userManager.GetRolesAsync(user);

                viewModel.Email = user?.Email;
                viewModel.UserName = user?.UserName;

                var allRoles = await _roleManager.Roles.ToListAsync();
                viewModel.Roles = allRoles.Select(x => new RoleViewModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Selected = userRoles.Contains(x.Name)
                }).ToArray();

            }

            return View(viewModel);
        }

        [HttpPost, Authorize("Users")]
        [ActionName("EditUser")]
        public async Task<IActionResult> EditUser(UserViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(viewModel.Id);
                var userRoles = await _userManager.GetRolesAsync(user);

                await _userManager.RemoveFromRolesAsync(user, userRoles);
                await _userManager.AddToRolesAsync(user, viewModel.Roles?.Where(x => x.Selected).Select(x => x.Name));
            }

            return Json(new { msg = "User Updated Successfully", msgType = "success", Url = "/Admin/UserList" });
        }



        #endregion;

        #region MenuControl
        [ActionName("MenuControl")]
        //[Authorize("Authorization")]
        public async Task<IActionResult> MenuControl()
        {
            FilterAlphabates();
            return View();
        }
        //[Authorize("Authorization")]
        [ActionName("MenuControlList")]
        public async Task<IActionResult> _MenuControlList(string filterOption = "All")
        {
            var menuControl = await _dataAccessService.GetAllMenuControlAsync(filterOption);
            return PartialView("_MenuControlList", menuControl);
        }
        // GET: Master/CreateMenuControl
        [HttpGet]
        //[Authorize("Roles")]
        [ActionName("CreateMenuControl")]
        public async Task<IActionResult> _CreateMenuControl(string id)
        {
            var menuControls = await _dataAccessService.GetAllMenuControlAsync();
            ViewData["ParentMenuId"] = new SelectList(menuControls, "Id", "Name", id);
            return PartialView("_CreateMenuControl", new NavigationMenuViewModel());
        }

        // POST: Master/CreateMenuControl
        [HttpPost]
        //[Authorize("Roles")]
        [ActionName("CreateMenuControl")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _CreateMenuControl(NavigationMenu model)
        {
            var jsonResponse = new { msg = "", msgType = "", Url = "" };
            try
            {
                if (ModelState.IsValid)
                {
                    if (model.Id != null) { model.Id = Guid.Empty; }
                    var result = await _dataAccessService.AddMenuControlByIdAsync(model);
                    if (result)
                    {
                        return Json(new { msg = "Role Created Successfully", msgType = "success", Url = "/Admin/MenuControlList" });
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { msg = "Exception: " + ex.Message, msgType = "error", Url = "/Admin/MenuControlList" });
            }

            return Json(new { msg = "Failed", msgType = "error", Url = "/Admin/MenuControlList" });
        }

        // GET: Master/EditMenuControl
        [HttpGet]
        [ActionName("EditMenuControl")]
        public async Task<IActionResult> _EditMenuControl(string id)
        {
            var menuControl = await _dataAccessService.GetMenuControlByIdAsync(id);
            var menuControls = await _dataAccessService.GetAllMenuControlAsync();
            ViewData["ParentMenuId"] = new SelectList(menuControls, "Id", "Name", menuControl.Id);
            return PartialView("_EditMenuControl", menuControl);
        }

        // POST: Master/EditMenuControl
        [HttpPost]
        [ActionName("EditMenuControl")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _EditMenuControl([Bind] NavigationMenu model)
        {
            try
            {
                var result = await _dataAccessService.EditMenuControlByIdAsync(model);
                if (result)
                {
                    return Json(new { msg = "MenuControl Updated Successfully", msgType = "success", Url = "/Admin/MenuControlList" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { msg = "Exception: " + ex.Message, msgType = "error", Url = "/Admin/MenuControlList" });
            }
            return Json(new { msg = "Updated Failed", msgType = "failed", Url = "/Admin/MenuControlList" });
        }

        // POST: Master/DeleteRole
        [HttpPost]
        public async Task<IActionResult> _DeleteMenuControl(NavigationMenu model)
        {
            try
            {
                var result = await _dataAccessService.DeleteMenuControlByIdAsync(model);
                if (result)
                {
                    return Json(new { msg = "MenuControl Deleted Successfully", msgType = "success", Url = "/Admin/MenuControlList" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { msg = "Exception: " + ex.Message, msgType = "error", Url = "/Preps/MenuControlList" });
            }
            return Json(new { msg = "MenuControl Deleted Successfully", msgType = "success", Url = "/Admin/MenuControlList" });
        }
        #endregion;

        #region Permissions
        //[Authorize("Authorization")]
        [ActionName("NodePermissions")]
        public async Task<IActionResult> NodePermissions()
        {
            FilterAlphabates();
            return View();
        }

        //[Authorize("Authorization")]
        [ActionName("PermissionList")]
        public async Task<IActionResult> _PermissionList(string roleid = null, string filterOption = "All")
        {
            var roleList = await _roleManager.Roles.ToListAsync();
            ViewData["RoleId"] = new SelectList(roleList, "Id", "Name", roleid);
            var permissions = new List<NavigationMenuViewModel>();
            if (!string.IsNullOrWhiteSpace(roleid))
            {
                permissions = await _dataAccessService.GetPermissionsByRoleIdAsync(roleid);
            }
            string jsonData = JsonConvert.SerializeObject(permissions);
            List<RoleNodes> roleNodes = JsonConvert.DeserializeObject<List<RoleNodes>>(jsonData);
            ViewData["RoleId"] = roleid;
            return PartialView("_PermissionList", roleNodes);
        }
        // GET: Master/CreateRole
        [HttpGet]
        [Authorize("Roles")]
        [ActionName("AddPermission")]
        public IActionResult _AddPermission()
        {
            return PartialView("_AddPermission", new RoleViewModel());
        }

        // POST: Master/CreateRole
        [HttpPost]
        [Authorize("Roles")]
        [ActionName("AddPermission")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _AddPermission(RoleViewModel viewModel)
        {
            var jsonResponse = new { msg = "", msgType = "", Url = "" };
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _roleManager.CreateAsync(new IdentityRoleExtended() { Name = viewModel.Name, ParentRoleId = viewModel.ParentRoleId });
                    if (result.Succeeded)
                    {
                        return Json(new { msg = "Role Created Successfully", msgType = "success", Url = "/Admin/RoleList" });
                    }
                    else
                    {
                        return Json(new { msg = string.Join(",", result.Errors), msgType = "error", Url = "/Admin/RoleList" });
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { msg = "Exception: " + ex.Message, msgType = "error", Url = "/Admin/RoleList" });
            }

            return Json(new { msg = "Failed", msgType = "success", Url = "/Admin/RoleList" });
        }

        [Authorize("Authorization")]
        public async Task<IActionResult> EditRolePermission(string id)
        {
            var permissions = new List<NavigationMenuViewModel>();
            if (!string.IsNullOrWhiteSpace(id))
            {
                permissions = await _dataAccessService.GetPermissionsByRoleIdAsync(id);
            }

            return View(permissions);
        }

        

        [HttpPost, Authorize("Authorization")]
        public async Task<IActionResult> EditRolePermission(string id, List<NavigationMenuViewModel> viewModel)
        {
            if (ModelState.IsValid)
            {
                var permissionIds = viewModel.Where(x => x.Permitted).Select(x => x.Id);

                await _dataAccessService.SetPermissionsByRoleIdAsync(id, permissionIds);
                return RedirectToAction(nameof(Roles));
            }

            return View(viewModel);
        }

        [Authorize("Authorization")]
        public async Task<IActionResult> EditRoleGroupPermission(string id)
        {
            var permissions = new List<NavigationMenuViewModel>();
            if (!string.IsNullOrWhiteSpace(id))
            {
                permissions = await _dataAccessService.GetPermissionsByRoleGroupIdAsync(id);
            }

            return View(permissions);
        }
        [HttpPost, Authorize("Authorization")]
        public async Task<IActionResult> EditRoleGroupPermission(string id, List<NavigationMenuViewModel> viewModel)
        {
            if (ModelState.IsValid)
            {
                var permissionIds = viewModel.Where(x => x.Permitted).Select(x => x.Id);

                await _dataAccessService.SetPermissionsByRoleIdAsync(id, permissionIds);
                return RedirectToAction(nameof(Roles));
            }

            return View(viewModel);
        }

        [HttpPost]
        [Authorize("Authorization")]
        public async Task<IActionResult> NodeRolePermission(string RoleId, List<RoleNodes> nodes)
        {
            List<NavigationMenuViewModel> viewModel = new List<NavigationMenuViewModel>();
            List<Guid> permissionIds = new List<Guid>();
            if (nodes.Count > 0)
            {
                foreach (var node in nodes.Where(x => x.Permitted))
                {
                    permissionIds.Add(node.Id);
                    if (node.ChildRoleNodes != null && node.ChildRoleNodes.Count > 0)
                    {
                        foreach (var child in node.ChildRoleNodes.Where(x => x.Permitted))
                        {
                            permissionIds.Add(child.Id);
                            if (child.ChildRoleNodes != null && child.ChildRoleNodes.Count > 0)
                            {
                                foreach (var subChild in child.ChildRoleNodes.Where(x => x.Permitted))
                                {
                                    permissionIds.Add(subChild.Id);
                                }
                            }
                        }
                    }
                    //var model = new NavigationMenuViewModel { 
                    //    Id = node.Id,
                    //    Name = node.Name,
                    //    Area = node.Area,
                    //    ControllerName = node.ControllerName,
                    //    ActionName = node.ActionName,
                    //    DisplayOrder = node.DisplayOrder,
                    //    ExternalUrl = node.ExternalUrl,
                    //    IsExternal = node.IsExternal,
                    //    ParentMenuId = node.ParentMenuId,
                    //    Permitted = node.Permitted,
                    //    Visible = node.Visible
                    //};
                    //viewModel.Add(model);
                }
                await _dataAccessService.SetPermissionsByRoleIdAsync(RoleId, permissionIds);
                return Json(new { msg = "Permission Added Successfully", msgType = "success", Url = "/Admin/PermissionList?roleid=" + RoleId });
            }

            return Json(new { msg = "Permission Can Not Add", msgType = "error", Url = "/Admin/PermissionList?roleid=" + RoleId });

        }
        #endregion;


        #region ManageUserRoles
        [Authorize("Authorization")]
        public async Task<IActionResult> ManageUserRoles()
        {
            FilterAlphabates();
            return View();
        }
        #endregion;

        #region Claims
        [HttpGet]
        public async Task<IActionResult> Claims(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                var name = User.Identity.Name;
                var user = _userManager.FindByNameAsync(name).Result;
                id = user.Id;
            }
            ViewData["UserId"] = id;
            return View();
        }

        [HttpGet]
        [ActionName("ClaimList")]
        public async Task<IActionResult> _ClaimList(string id, string filterOption = "All")
        {
            if (string.IsNullOrEmpty(id))
            {
                return View();
            }
            ViewData["UserId"] = id;
            IdentityUserExtended user = await _userManager.FindByIdAsync(id);
            IEnumerable<Claim> Claims = await _userManager.GetClaimsAsync(user);
            if (filterOption == "All")
                return PartialView("_ClaimList", Claims);
            else
                return PartialView("_ClaimList", Claims.Where(x => x.Value.StartsWith(filterOption)));

        }
        [HttpGet]
        [ActionName("AddClaims")]
        public async Task<IActionResult> _AddClaims(string Id)
        {
            ViewData["UserId"] = Id;
            ViewData["ClaimTypes"] = ApplicationClaimTypes.AppClaimTypes;
            return PartialView("_AddClaims");
        }

        [HttpPost]
        [ActionName("AddClaims")]
        public async Task<IActionResult> _AddClaims([Required] string Id, [Required] string type, [Required] string value)
        {
            IdentityUserExtended user = await _userManager.FindByIdAsync(Id);
            ViewData["ClaimTypes"] = ApplicationClaimTypes.AppClaimTypes;
            if (ModelState.IsValid)
            {
                var claim = new Claim(type, value);
                var result = await _userManager.AddClaimAsync(user, claim);
                if (result.Succeeded)
                {
                    return Json(new { msg = "Claim Added Successfully", msgType = "success", Url = "/Admin/ClaimList?id=" + Id });
                }
            }
            return Json(new { msg = "Claim Add Failed", msgType = "error", Url = "/Admin/ClaimList?id=" + Id });
        }

        [HttpGet]
        [ActionName("EditClaims")]
        public async Task<IActionResult> _EditClaims(string id, string type, string value)
        {
            if (string.IsNullOrEmpty(id))
            {
                return View();
            }
            ViewData["UserId"] = id;
            IdentityUserExtended user = await _userManager.FindByIdAsync(id);
            IEnumerable<Claim> Claims = await _userManager.GetClaimsAsync(user);
            var claim = Claims.Where(x => x.Type == type && x.Value == value).SingleOrDefault();
            return PartialView("_EditClaims", claim);
        }

        [HttpPost]
        [ActionName("EditClaims")]
        public async Task<IActionResult> _EditClaims([Required] string Id, [Required] string type,
                                                              [Required] string value,
                                                              [Required] string oldValue)
        {
            IdentityUserExtended user = await _userManager.FindByIdAsync(Id);
            if (ModelState.IsValid)
            {
                var claimNew = new Claim(type, value);
                var claimOld = new Claim(type, oldValue);
                var result = await _userManager.ReplaceClaimAsync(user, claimOld, claimNew);
                if (result.Succeeded)
                {
                    return Json(new { msg = "Claim Updated Successfully", msgType = "success", Url = "/Admin/ClaimList?id=" + Id });
                }
            }
            return Json(new { msg = "Claim Updated Failed", msgType = "error", Url = "/Admin/ClaimList?id=" + Id });
        }

        [HttpGet]
        [ActionName("DeleteClaims")]
        public async Task<IActionResult> _DeleteClaims(string id, string type, string value)
        {
            if (string.IsNullOrEmpty(id))
            {
                return View();
            }
            ViewData["UserId"] = id;
            IdentityUserExtended user = await _userManager.FindByIdAsync(id);
            IEnumerable<Claim> Claims = await _userManager.GetClaimsAsync(user);
            var claim = Claims.Where(x => x.Type == type && x.Value == value).SingleOrDefault();
            return PartialView("_DeleteClaims", claim);
        }


        [HttpPost]
        [ActionName("DeleteClaims")]
        public async Task<IActionResult> _DeleteClaimsPost([Required] string Id, [Required] string type,
                                                              [Required] string value)
        {
            IdentityUserExtended user = await _userManager.FindByIdAsync(Id);
            //var Claims = await _userManager.GetClaimsAsync(user);
            //var claimResult = Claims.Where(x => x.Type == type && x.Value == value).SingleOrDefault();
            if (ModelState.IsValid)
            {
                var claim = new Claim(type, value);
                var result = await _userManager.RemoveClaimAsync(user, claim);
                if (result.Succeeded)
                {
                    return Json(new { msg = "Claim Deleted Successfully", msgType = "success", Url = "/Admin/ClaimList?id=" + Id });
                }
            }
            return Json(new { msg = "Claim Delete Failed", msgType = "error", Url = "/Admin/ClaimList?id" + Id });
        }
        #endregion;

        #region Privatemethods
        private void FilterAlphabates()
        {
            string[] filterOptions = { "All", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            ViewData["FilterOption"] = filterOptions;
        }
        #endregion;
    }
}
