using Core.Models.Data;
using Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Core.Repo.Initializers
{
    public static class DbInitializer
    {
        public static void Initialize(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                if (context != null)
                {
                    context.Database.EnsureCreated();

                    var _userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<IdentityUserExtended>>();
                    var _roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRoleExtended>>();
                    #region Dev_Admin_Section
                    if (!context.Users.Any(usr => usr.UserName == "superadmin@rbac.com"))
                    {
                        var user = new IdentityUserExtended()
                        {
                            UserName = "superadmin@rbac.com",
                            Email = "superadmin@rbac.com",
                            EmailConfirmed = true,
                            LockoutEnabled = false,
                            IsApproved = true,
                            IsSystemAdmin = true,
                            Comment = "System Admin Can Access Entire Application with all permissions"
                        };

                        var userResult = _userManager.CreateAsync(user, "P@ssw0rd").Result;
                    }
                    if (!context.Users.Any(usr => usr.UserName == "authadmin@rbac.com"))
                    {
                        var user = new IdentityUserExtended()
                        {
                            UserName = "authadmin@rbac.com",
                            Email = "authadmin@rbac.com",
                            EmailConfirmed = true,
                            LockoutEnabled = false,
                            IsApproved = true,
                            IsSystemAdmin = true,
                            Comment = "Auth Admin Can Access All Administrative Section(Role,User,Menu,RolePermission etc)"
                        };

                        var userResult = _userManager.CreateAsync(user, "P@ssw0rd").Result;
                    }
                    if (!context.Users.Any(usr => usr.UserName == "useradmin@rbac.com"))
                    {
                        var user = new IdentityUserExtended()
                        {
                            UserName = "useradmin@rbac.com",
                            Email = "useradmin@rbac.com",
                            EmailConfirmed = true,
                            LockoutEnabled = false,
                            IsApproved = true,
                            IsSystemAdmin = true,
                            Comment = "User Admin Can Access User Management Section Only."
                        };

                        var userResult = _userManager.CreateAsync(user, "P@ssw0rd").Result;
                    }
                    if (!context.Users.Any(usr => usr.UserName == "roleadmin@rbac.com"))
                    {
                        var user = new IdentityUserExtended()
                        {
                            UserName = "roleadmin@rbac.com",
                            Email = "roleadmin@rbac.com",
                            EmailConfirmed = true,
                            LockoutEnabled = false,
                            IsApproved = true,
                            IsSystemAdmin = true,
                            Comment = "Role Admin Can Access Role Management Section Only."
                        };

                        var userResult = _userManager.CreateAsync(user, "P@ssw0rd").Result;
                    }
                    if (!context.Users.Any(usr => usr.UserName == "menuadmin@rbac.com"))
                    {
                        var user = new IdentityUserExtended()
                        {
                            UserName = "menuadmin@rbac.com",
                            Email = "menuadmin@rbac.com",
                            EmailConfirmed = true,
                            LockoutEnabled = false,
                            IsApproved = true,
                            IsSystemAdmin = true,
                            Comment = "Menu Admin can access Menu/Node Management Only."
                        };

                        var userResult = _userManager.CreateAsync(user, "P@ssw0rd").Result;
                    }
                    if (!context.Users.Any(usr => usr.UserName == "rolegroupadmin@rbac.com"))
                    {
                        var user = new IdentityUserExtended()
                        {
                            UserName = "rolegroupadmin@rbac.com",
                            Email = "rolegroupadmin@rbac.com",
                            EmailConfirmed = true,
                            LockoutEnabled = false,
                            IsApproved = true,
                            IsSystemAdmin = true,
                            Comment = "Role Group Admin Can Manage Role Group Section Only."
                        };

                        var userResult = _userManager.CreateAsync(user, "P@ssw0rd").Result;
                    }
                    if (!context.Users.Any(usr => usr.UserName == "rolepermission@rbac.com"))
                    {
                        var user = new IdentityUserExtended()
                        {
                            UserName = "rolepermission@rbac.com",
                            Email = "rolepermission@rbac.com",
                            EmailConfirmed = true,
                            LockoutEnabled = false,
                            IsApproved = true,
                            IsSystemAdmin = true,
                            Comment = "Role Permission Admin Can Access Node Role Permisiion Section Only"
                        };

                        var userResult = _userManager.CreateAsync(user, "P@ssw0rd").Result;
                    }
                    #endregion;

                    #region User_Admin_Section
                    if (!context.Users.Any(usr => usr.UserName == "client_admin@rbac.com"))
                    {
                        var user = new IdentityUserExtended()
                        {
                            UserName = "client_admin@rbac.com",
                            Email = "client_admin@rbac.com",
                            EmailConfirmed = true,
                            LockoutEnabled = false,
                            IsApproved = true,
                            IsSystemAdmin = false,
                            Comment = "Client Admin User Can Access Entire User Application Login Section Only."
                        };

                        var userResult = _userManager.CreateAsync(user, "P@ssw0rd").Result;
                    }
                    if (!context.Users.Any(usr => usr.UserName == "client_hr@rbac.com"))
                    {
                        var user = new IdentityUserExtended()
                        {
                            UserName = "client_hr@rbac.com",
                            Email = "client_hr@rbac.com",
                            EmailConfirmed = true,
                            LockoutEnabled = false,
                            IsApproved = true,
                            IsSystemAdmin = false,
                            Comment = "Client HR User Can Access Human Resource Login Section Only."
                        };

                        var userResult = _userManager.CreateAsync(user, "P@ssw0rd").Result;
                    }
                    if (!context.Users.Any(usr => usr.UserName == "client_payroll@rbac.com"))
                    {
                        var user = new IdentityUserExtended()
                        {
                            UserName = "client_payroll@rbac.com",
                            Email = "client_payroll@rbac.com",
                            EmailConfirmed = true,
                            LockoutEnabled = false,
                            IsApproved = true,
                            IsSystemAdmin = false,
                            Comment = "Client Payroll User Can Access Account/Salary/Payroll Login Section Only."
                        };

                        var userResult = _userManager.CreateAsync(user, "P@ssw0rd").Result;
                    }
                    if (!context.Users.Any(usr => usr.UserName == "client_reception@rbac.com"))
                    {
                        var user = new IdentityUserExtended()
                        {
                            UserName = "client_reception@rbac.com",
                            Email = "client_reception@rbac.com",
                            EmailConfirmed = true,
                            LockoutEnabled = false,
                            IsApproved = true,
                            IsSystemAdmin = false,
                            Comment = "Client Reception User Can Access Reception Login Section Only."
                        };

                        var userResult = _userManager.CreateAsync(user, "P@ssw0rd").Result;
                    }
                    if (!context.Users.Any(usr => usr.UserName == "client_manager@rbac.com"))
                    {
                        var user = new IdentityUserExtended()
                        {
                            UserName = "client_manager@rbac.com",
                            Email = "client_manager@rbac.com",
                            EmailConfirmed = true,
                            LockoutEnabled = false,
                            IsApproved = true,
                            IsSystemAdmin = false,
                            Comment = "Client Manager User Can Access Manager Login Section Only."
                        };

                        var userResult = _userManager.CreateAsync(user, "P@ssw0rd").Result;
                    }
                    if (!context.Users.Any(usr => usr.UserName == "client_cc_support@rbac.com"))
                    {
                        var user = new IdentityUserExtended()
                        {
                            UserName = "client_cc_support@rbac.com",
                            Email = "client_cc_support@rbac.com",
                            EmailConfirmed = true,
                            LockoutEnabled = false,
                            IsApproved = true,
                            IsSystemAdmin = false,
                            Comment = "Client CC Support Network User Can Access Customer Care Support Login Section Only."
                        };

                        var userResult = _userManager.CreateAsync(user, "P@ssw0rd").Result;
                    }
                    if (!context.Users.Any(usr => usr.UserName == "client_it_network@rbac.com"))
                    {
                        var user = new IdentityUserExtended()
                        {
                            UserName = "client_it_network@rbac.com",
                            Email = "client_it_network@rbac.com",
                            EmailConfirmed = true,
                            LockoutEnabled = false,
                            IsApproved = true,
                            IsSystemAdmin = false,
                            Comment = "Client IT Network User Can Access IT/Network Login Section Only."
                        };

                        var userResult = _userManager.CreateAsync(user, "P@ssw0rd").Result;
                    }
                    if (!context.Users.Any(usr => usr.UserName == "client_tech@rbac.com"))
                    {
                        var user = new IdentityUserExtended()
                        {
                            UserName = "client_tech@rbac.com",
                            Email = "client_tech@rbac.com",
                            EmailConfirmed = true,
                            LockoutEnabled = false,
                            IsApproved = true,
                            IsSystemAdmin = false,
                            Comment = "Client Tech User Can access Techinal Login Section Only."
                        };

                        var userResult = _userManager.CreateAsync(user, "P@ssw0rd").Result;
                    }
                    #endregion;

                    #region Parent_Role_Section

                    if (!_roleManager.RoleExistsAsync("DevAdmin").Result)
                    {
                        var role = _roleManager.CreateAsync(new IdentityRoleExtended { Name = "DevAdmin" }).Result;
                    }

                    if (!_roleManager.RoleExistsAsync("ClientAdmin").Result)
                    {
                        var role = _roleManager.CreateAsync(new IdentityRoleExtended { Name = "ClientAdmin" }).Result;
                    }

                    #endregion;

                    #region Role_Group_Section
                    #region Dev_Admin_Groups
                    var perentRoleId = "";
                    if (_roleManager.RoleExistsAsync("DevAdmin").Result)
                    {
                        perentRoleId = _roleManager.Roles.Where(x => x.Name == "DevAdmin").SingleOrDefault().Id;
                    }
                    if (!string.IsNullOrEmpty(perentRoleId))
                    {
                        if (!_roleManager.RoleExistsAsync("SuperEditorGroup").Result)
                        {
                            var role = _roleManager.CreateAsync(new IdentityRoleExtended { Name = "SuperEditorGroup", ParentRoleId = perentRoleId }).Result;
                        }
                        if (!_roleManager.RoleExistsAsync("AuthEditorGroup").Result)
                        {
                            var role = _roleManager.CreateAsync(new IdentityRoleExtended { Name = "AuthEditorGroup", ParentRoleId = perentRoleId }).Result;
                        }
                        if (!_roleManager.RoleExistsAsync("UserEditorGroup").Result)
                        {
                            var role = _roleManager.CreateAsync(new IdentityRoleExtended { Name = "UserEditorGroup", ParentRoleId = perentRoleId }).Result;
                        }
                        if (!_roleManager.RoleExistsAsync("RoleEditorGroup").Result)
                        {
                            var role = _roleManager.CreateAsync(new IdentityRoleExtended { Name = "RoleEditorGroup", ParentRoleId = perentRoleId }).Result;
                        }
                        if (!_roleManager.RoleExistsAsync("MenuEditorGroup").Result)
                        {
                            var role = _roleManager.CreateAsync(new IdentityRoleExtended { Name = "MenuEditorGroup", ParentRoleId = perentRoleId }).Result;
                        }
                        if (!_roleManager.RoleExistsAsync("RoleGroupEditorGroup").Result)
                        {
                            var role = _roleManager.CreateAsync(new IdentityRoleExtended { Name = "RoleGroupEditorGroup", ParentRoleId = perentRoleId }).Result;
                        }
                        if (!_roleManager.RoleExistsAsync("RolePermissionEditorGroup").Result)
                        {
                            var role = _roleManager.CreateAsync(new IdentityRoleExtended { Name = "RolePermissionEditorGroup", ParentRoleId = perentRoleId }).Result;
                        }
                    }
                    #endregion;
                    #region Client_Admin_Groups
                    perentRoleId = "";
                    if (_roleManager.RoleExistsAsync("ClientAdmin").Result)
                    {
                        perentRoleId = _roleManager.Roles.Where(x => x.Name == "ClientAdmin").SingleOrDefault().Id;
                    }
                    if (!_roleManager.RoleExistsAsync("ClientAdminGroup").Result)
                    {
                        var role = _roleManager.CreateAsync(new IdentityRoleExtended { Name = "ClientAdminGroup", ParentRoleId = perentRoleId }).Result;
                    }
                    if (!_roleManager.RoleExistsAsync("ClientHRGroup").Result)
                    {
                        var role = _roleManager.CreateAsync(new IdentityRoleExtended { Name = "ClientHRGroup", ParentRoleId = perentRoleId }).Result;
                    }
                    if (!_roleManager.RoleExistsAsync("ClientPayrollGroup").Result)
                    {
                        var role = _roleManager.CreateAsync(new IdentityRoleExtended { Name = "ClientPayrollGroup", ParentRoleId = perentRoleId }).Result;
                    }
                    if (!_roleManager.RoleExistsAsync("ClientReceptionGroup").Result)
                    {
                        var role = _roleManager.CreateAsync(new IdentityRoleExtended { Name = "ClientReceptionGroup", ParentRoleId = perentRoleId }).Result;
                    }
                    if (!_roleManager.RoleExistsAsync("ClientManagerGroup").Result)
                    {
                        var role = _roleManager.CreateAsync(new IdentityRoleExtended { Name = "ClientManagerGroup", ParentRoleId = perentRoleId }).Result;
                    }
                    if (!_roleManager.RoleExistsAsync("ClientSupportGroup").Result)
                    {
                        var role = _roleManager.CreateAsync(new IdentityRoleExtended { Name = "ClientSupportGroup", ParentRoleId = perentRoleId }).Result;
                    }
                    if (!_roleManager.RoleExistsAsync("ClientITGroup").Result)
                    {
                        var role = _roleManager.CreateAsync(new IdentityRoleExtended { Name = "ClientITGroup", ParentRoleId = perentRoleId }).Result;
                    }
                    if (!_roleManager.RoleExistsAsync("ClientTechGroup").Result)
                    {
                        var role = _roleManager.CreateAsync(new IdentityRoleExtended { Name = "ClientTechGroup", ParentRoleId = perentRoleId }).Result;
                    }
                    #endregion;


                    #endregion;

                    #region Assign_User_Roles
                    perentRoleId = ""; int count = 0;
                    if (_roleManager.RoleExistsAsync("DevAdmin").Result)
                    {
                        perentRoleId = _roleManager.Roles.Where(x => x.Name == "DevAdmin").SingleOrDefault().Id;
                    }
                    var _users = _userManager.Users.Where(x => x.IsSystemAdmin == true).OrderBy(o => o.UserName).ToListAsync().Result;
                    var _roles = _roleManager.Roles.Where(x => x.ParentRoleId == perentRoleId).OrderBy(o => o.Name).ToListAsync().Result;
                    if ((_users != null && _users.Count() > 0) && (_roles != null && _roles.Count() > 0))
                    {
                        for (int i = 0; i < _users.Count(); i++)
                        {
                            var resultRole = _userManager.AddToRolesAsync(_users[i], new string[] { _roles[i].Name }).Result;

                        }
                    }

                    perentRoleId = ""; count = 0;
                    if (_roleManager.RoleExistsAsync("ClientAdmin").Result)
                    {
                        perentRoleId = _roleManager.Roles.Where(x => x.Name == "ClientAdmin").SingleOrDefault().Id;
                    }
                    var _clientUsers = _userManager.Users.Where(x => x.IsSystemAdmin == false).OrderBy(o => o.UserName).ToListAsync().Result;
                    var _clientRoles = _roleManager.Roles.Where(x => x.ParentRoleId == perentRoleId).OrderBy(o => o.Name).ToListAsync().Result;
                    if ((_clientUsers != null && _clientUsers.Count() > 0) && (_clientRoles != null && _clientRoles.Count() > 0))
                    {
                        for (int i = 0; i < _clientUsers.Count(); i++)
                        {
                            var resultRole = _userManager.AddToRolesAsync(_clientUsers[i], new string[] { _clientRoles[i].Name }).Result;

                        }
                    }

                    #endregion;

                    var nodes = GetPermissions();
                    foreach (var item in nodes)
                    {
                        if (context?.NavigationMenu?.Any(n => n.Name == item.Name) == false)
                        {
                            context.NavigationMenu.Add(item);
                        }
                    }
                    //context.SaveChanges();


                    var _adminRole = _roleManager.Roles.Where(x => x.Name == "SuperEditorGroup").FirstOrDefault();
                    var _clientRole = _roleManager.Roles.Where(x => x.Name == "ClientAdminGroup").FirstOrDefault();

                    var adminNodes = nodes.Where(x => x.Id.ToString() == "13e2f21a-4283-4ff8-bb7a-096e7b89e0f0").ToList();

                    if (_adminRole != null && (adminNodes != null && adminNodes.Count > 0))
                    {
                        //---Finding and Adding Admin Nodes
                        var _adminChildNodes = adminNodes.ToList();
                        var _adminSubChildNodes = new List<NavigationMenu>();
                        foreach (var admin in adminNodes)
                        {
                            var childNodes = nodes.Where(x => x.ParentMenuId == admin.Id).ToList();
                            _adminChildNodes.AddRange(childNodes);
                            foreach (var child in _adminChildNodes.Where(x => x.Id != admin.Id).ToList())
                            {
                                var subChildNodes = nodes.Where(x => x.ParentMenuId == child.Id).ToList();
                                _adminSubChildNodes.AddRange(subChildNodes);
                            }
                        }
                        _adminChildNodes.AddRange(_adminSubChildNodes);
                        foreach (var node in _adminChildNodes)
                        {
                            if (!context.RoleMenuPermission.Any(x => x.RoleId == _adminRole.Id && x.NavigationMenuId == node.Id))
                            {
                                context.RoleMenuPermission.Add(new RoleMenuPermission() { RoleId = _adminRole.Id, NavigationMenuId = node.Id, CanView = true, CanAdd = true, CanEdit = true, CanDelete = true, Active = true });
                                //context.SaveChanges();
                            }
                        }
                    }

                    var clientNodes = nodes.Where(x => x.Id.ToString() == "10d7cde0-997a-4142-a5eb-12b77c7b6e7b").ToList();
                    if (_clientRole != null)
                    {
                        //---Finding and Adding client Nodes
                        var _clientChildNodes = clientNodes.ToList();
                        var _adminSubChildNodes = new List<NavigationMenu>();
                        foreach (var admin in clientNodes)
                        {
                            var childNodes = nodes.Where(x => x.ParentMenuId == (admin.Id)).ToList();
                            _clientChildNodes.AddRange(childNodes);
                            foreach (var child in _clientChildNodes.Where(x => x.Id != (admin.Id)).ToList())
                            {
                                var subChildNodes = nodes.Where(x => x.ParentMenuId == (child.Id)).ToList();
                                _adminSubChildNodes.AddRange(subChildNodes);
                            }
                        }
                        _clientChildNodes.AddRange(_adminSubChildNodes);
                        foreach (var node in _clientChildNodes)
                        {
                            if (!context.RoleMenuPermission.Any(x => x.RoleId == _clientRole.Id && x.NavigationMenuId == node.Id))
                            {
                                context.RoleMenuPermission.Add(new RoleMenuPermission() { RoleId = _clientRole.Id, NavigationMenuId = node.Id, CanView = true, CanAdd = true, CanEdit = true, CanDelete = true, Active = true });
                                
                            }
                        }
                    }

                    context.SaveChanges();
                }
            }
        }

        private static List<NavigationMenu> GetPermissions()
        {
            return new List<NavigationMenu>()
            {
                #region #Parent_Menu
                new NavigationMenu()
                {
                    Id = new Guid("13e2f21a-4283-4ff8-bb7a-096e7b89e0f0"),
                    Name = "Admin",
                    ControllerName = "",
                    ActionName = "",
                    ParentMenuId = null,
                    DisplayOrder=1,
                    Visible = true,
                },
                new NavigationMenu()
                {
                    Id = new Guid("10d7cde0-997a-4142-a5eb-12b77c7b6e7b"),
                    Name = "Client",
                    ControllerName = "",
                    ActionName = "",
                    ParentMenuId = null,
                    DisplayOrder=2,
                    Visible = true,
                },
                #endregion;
                #region Child_Menu
                new NavigationMenu()
                {
                    Id = new Guid("02443909-b3b0-4e21-8e93-56738d9e5055"),
                    Name = "Dashboard",
                    ControllerName = "Admin",
                    ActionName = "Index",
                    ParentMenuId = new Guid("13e2f21a-4283-4ff8-bb7a-096e7b89e0f0"),
                    DisplayOrder=1,
                    Visible = true,
                },
                new NavigationMenu()
                {
                    Id = new Guid("283264d6-0e5e-48fe-9d6e-b1599aa0892c"),
                    Name = "Roles",
                    ControllerName = "Admin",
                    ActionName = "Roles",
                    ParentMenuId = new Guid("13e2f21a-4283-4ff8-bb7a-096e7b89e0f0"),
                    DisplayOrder=2,
                    Visible = true,
                },
                new NavigationMenu()
                {
                    Id = new Guid("6C4C0807-8D54-4579-9C07-4556E1660C4D"),
                    Name = "RoleGroups",
                    ControllerName = "Admin",
                    ActionName = "RoleGroups",
                    ParentMenuId = new Guid("13e2f21a-4283-4ff8-bb7a-096e7b89e0f0"),
                    DisplayOrder=3,
                    Visible = true,
                },
                new NavigationMenu()
                {
                    Id = new Guid("7cd0d373-c57d-4c70-aa8c-22791983fe1c"),
                    Name = "Users",
                    ControllerName = "Admin",
                    ActionName = "Users",
                    ParentMenuId = new Guid("13e2f21a-4283-4ff8-bb7a-096e7b89e0f0"),
                    DisplayOrder=4,
                    Visible = true,
                },
                new NavigationMenu()
                {
                    Id = new Guid("F704BDFD-D3EA-4A6F-9463-DA47ED3657AB"),
                    Name = "Role-based authorization in ASP.NET Core",
                    ControllerName = "",
                    ActionName = "",
                    IsExternal = true,
                    ExternalUrl = "https://learn.microsoft.com/en-us/aspnet/core/security/authorization/roles?view=aspnetcore-8.0",
                    ParentMenuId = new Guid("13e2f21a-4283-4ff8-bb7a-096e7b89e0f0"),
                    DisplayOrder=5,
                    Visible = true,
                },
                new NavigationMenu()
                {
                    Id = new Guid("09657163-3231-4b2c-964d-6fcd8e6f08b6"),
                    Name = "Menu Control",
                    ControllerName = "Admin",
                    ActionName = "MenuControl",
                    ParentMenuId = new Guid("13e2f21a-4283-4ff8-bb7a-096e7b89e0f0"),
                    DisplayOrder=6,
                    Visible = true,
                },
                new NavigationMenu()
                {
                    Id = new Guid("2513c5b5-b4f3-4d99-b3aa-d2ddda555465"),
                    Name = "Role Permissions",
                    ControllerName = "Admin",
                    ActionName = "NodePermissions",
                    ParentMenuId = new Guid("13e2f21a-4283-4ff8-bb7a-096e7b89e0f0"),
                    DisplayOrder=7,
                    Visible = true,
                },
                new NavigationMenu()
                {
                    Id = new Guid("5d4460f6-5fff-40c5-a8f1-294116d88588"),
                    Name = "Manage User Roles",
                    ControllerName = "Admin",
                    ActionName = "ManageUserRoles",
                    ParentMenuId = new Guid("13e2f21a-4283-4ff8-bb7a-096e7b89e0f0"),
                    DisplayOrder=8,
                    Visible = true,
                },


                new NavigationMenu()
                {
                    Id = new Guid("c82df19f-8e12-405c-8c8a-0a04a4e4d9fc"),
                    Name = "Dashbord",
                    ControllerName = "Home",
                    ActionName = "Index",
                    ParentMenuId = new Guid("10d7cde0-997a-4142-a5eb-12b77c7b6e7b"),
                    DisplayOrder=1,
                    Visible = false,
                },
                new NavigationMenu()
                {
                    Id = new Guid("68fa339b-0584-4a0c-8321-33b60af1ce70"),
                    Name = "Client Admin",
                    ControllerName = "Home",
                    ActionName = "ClientAdmin",
                    ParentMenuId = new Guid("10d7cde0-997a-4142-a5eb-12b77c7b6e7b"),
                    DisplayOrder=2,
                    Visible = false,
                },
                new NavigationMenu()
                {
                    Id = new Guid("1064ede4-2e64-4098-ab47-dee033a723ba"),
                    Name = "HR",
                    ControllerName = "Home",
                    ActionName = "HumanResource",
                    ParentMenuId = new Guid("10d7cde0-997a-4142-a5eb-12b77c7b6e7b"),
                    DisplayOrder=3,
                    Visible = false,
                },
                new NavigationMenu()
                {
                    Id = new Guid("b269ccdd-2ef4-415f-a173-9ff3947bff92"),
                    Name = "Pay Roll",
                    ControllerName = "Home",
                    ActionName = "Payroll",
                    ParentMenuId = new Guid("10d7cde0-997a-4142-a5eb-12b77c7b6e7b"),
                    DisplayOrder=4,
                    Visible = false,
                },
                new NavigationMenu()
                {
                    Id = new Guid("951e7766-f854-40ca-8edc-bd666cbeb464"),
                    Name = "Reception",
                    ControllerName = "Home",
                    ActionName = "Reception",
                    ParentMenuId = new Guid("10d7cde0-997a-4142-a5eb-12b77c7b6e7b"),
                    DisplayOrder=5,
                    Visible = false,
                },
                new NavigationMenu()
                {
                    Id = new Guid("a9980aaa-74ba-432f-acdb-d5a378d6c486"),
                    Name = "Manager",
                    ControllerName = "Home",
                    ActionName = "Manager",
                    ParentMenuId = new Guid("10d7cde0-997a-4142-a5eb-12b77c7b6e7b"),
                    DisplayOrder=6,
                    Visible = false,
                },
                new NavigationMenu()
                {
                    Id = new Guid("f406a668-30dd-4c76-ac25-374ccb877703"),
                    Name = "Customer Support",
                    ControllerName = "Home",
                    ActionName = "CustomerSupport",
                    ParentMenuId = new Guid("10d7cde0-997a-4142-a5eb-12b77c7b6e7b"),
                    DisplayOrder=7,
                    Visible = false,
                },
                new NavigationMenu()
                {
                    Id = new Guid("7fadd818-2d49-49d9-bb8d-7b435f6f51b8"),
                    Name = "IT Network",
                    ControllerName = "Home",
                    ActionName = "ITNetwork",
                    ParentMenuId = new Guid("10d7cde0-997a-4142-a5eb-12b77c7b6e7b"),
                    DisplayOrder=8,
                    Visible = false,
                },
                new NavigationMenu()
                {
                    Id = new Guid("e1ebd8e8-3961-44df-90c8-a12c75ce8ded"),
                    Name = "Engineering",
                    ControllerName = "Home",
                    ActionName = "Engineering",
                    ParentMenuId = new Guid("10d7cde0-997a-4142-a5eb-12b77c7b6e7b"),
                    DisplayOrder=9,
                    Visible = false,
                },
                #endregion;
                #region Sub_Child_Menu
                #region Role_Sub_Menu
                new NavigationMenu()
                {
                    Id = new Guid("2b182269-01ec-4631-80b4-3a329f15b190"),
                    Name = "Role List",
                    ControllerName = "Admin",
                    ActionName = "RoleList",
                    ParentMenuId = new Guid("283264d6-0e5e-48fe-9d6e-b1599aa0892c"),
                    DisplayOrder=1,
                    Visible = false,
                },
                new NavigationMenu()
                {
                    Id = new Guid("4040fa91-49fe-4f6a-822c-e8d836c25fd9"),
                    Name = "Create Role",
                    ControllerName = "Admin",
                    ActionName = "CreateRole",
                    ParentMenuId = new Guid("283264d6-0e5e-48fe-9d6e-b1599aa0892c"),
                    DisplayOrder=2,
                    Visible = false,
                },
                new NavigationMenu()
                {
                    Id = new Guid("381111ad-7c61-4ec1-a60d-89b2ad59f60c"),
                    Name = "Edit Role",
                    ControllerName = "Admin",
                    ActionName = "EditRole",
                    ParentMenuId = new Guid("283264d6-0e5e-48fe-9d6e-b1599aa0892c"),
                    DisplayOrder=3,
                    Visible = false,
                },
                new NavigationMenu()
                {
                    Id = new Guid("0af42e63-a350-4a25-8b85-35c17282be4a"),
                    Name = "Delete Role",
                    ControllerName = "Admin",
                    ActionName = "DeleteRole",
                    ParentMenuId = new Guid("283264d6-0e5e-48fe-9d6e-b1599aa0892c"),
                    DisplayOrder=4,
                    Visible = false,
                },


                #endregion;

                 #region Role_Group_Sub_Menu
                new NavigationMenu()
                {
                    Id = new Guid("DC9F0EBD-031F-4519-8837-FBEC0E299DC5"),
                    Name = "Role Group List",
                    ControllerName = "Admin",
                    ActionName = "RoleGroupList",
                    ParentMenuId = new Guid("6C4C0807-8D54-4579-9C07-4556E1660C4D"),
                    DisplayOrder=1,
                    Visible = false,
                },
                new NavigationMenu()
                {
                    Id = new Guid("228CAC38-B720-41BD-90DD-00B79F84F1FB"),
                    Name = "Create Role Group",
                    ControllerName = "Admin",
                    ActionName = "CreateRoleGroup",
                    ParentMenuId = new Guid("6C4C0807-8D54-4579-9C07-4556E1660C4D"),
                    DisplayOrder=2,
                    Visible = false,
                },
                new NavigationMenu()
                {
                    Id = new Guid("E74728AE-2AC5-4320-A2EB-B7A6989C8299"),
                    Name = "Edit Role Group", 
                    ControllerName = "Admin",
                    ActionName = "EditRoleGroup",
                    ParentMenuId = new Guid("6C4C0807-8D54-4579-9C07-4556E1660C4D"),
                    DisplayOrder=3,
                    Visible = false,
                },
                new NavigationMenu()
                {
                    Id = new Guid("049B1379-FBF3-45B5-81A8-A23C295D4506"),
                    Name = "Delete Role Group",
                    ControllerName = "Admin",
                    ActionName = "_DeleteRoleGroup",
                    ParentMenuId = new Guid("6C4C0807-8D54-4579-9C07-4556E1660C4D"),
                    DisplayOrder=4,
                    Visible = false,
                },

                new NavigationMenu()
                {
                    Id = new Guid("8D354620-780A-4AB7-8A06-3D388F30CC9C"),
                    Name = "Delete Role Group Permissions",
                    ControllerName = "Admin",
                    ActionName = "EditRoleGroupPermission",
                    ParentMenuId = new Guid("6C4C0807-8D54-4579-9C07-4556E1660C4D"),
                    DisplayOrder=5,
                    Visible = false,
                },
                #endregion;

                #region User_Sub_Menu
                 new NavigationMenu()
                {
                    Id = new Guid("68ee43c4-110b-46eb-bafd-55856b220259"),
                    Name = "User List",
                    ControllerName = "Admin",
                    ActionName = "UserList",
                    ParentMenuId = new Guid("7cd0d373-c57d-4c70-aa8c-22791983fe1c"),
                    DisplayOrder=1,
                    Visible = false,
                },
                 new NavigationMenu()
                {
                    Id = new Guid("8ade85ee-5ca2-4b3e-b9d9-c13f113f0bf9"),
                    Name = "Create User",
                    ControllerName = "Admin",
                    ActionName = "CreateUser",
                    ParentMenuId = new Guid("7cd0d373-c57d-4c70-aa8c-22791983fe1c"),
                    DisplayOrder=2,
                    Visible = false,
                },
                 new NavigationMenu()
                {
                    Id = new Guid("a41e26b4-6b60-4f4e-b3da-c6b6a7481d75"),
                    Name = "Edit User",
                    ControllerName = "Admin",
                    ActionName = "EditUser",
                    ParentMenuId = new Guid("7cd0d373-c57d-4c70-aa8c-22791983fe1c"),
                    DisplayOrder=3,
                    Visible = false,
                },
                 new NavigationMenu()
                {
                    Id = new Guid("a5d79ed8-639e-4fa9-bf2d-777e83b7d072"),
                    Name = "Delete User",
                    ControllerName = "Admin",
                    ActionName = "DeleteUser",
                    ParentMenuId = new Guid("7cd0d373-c57d-4c70-aa8c-22791983fe1c"),
                    DisplayOrder=4,
                    Visible = false,
                },
                #endregion;

                #region User_Role_SubMenu
                 new NavigationMenu()
                {
                    Id = new Guid("32eec562-a158-4d98-9a0e-d8ad63bbb05c"),
                    Name = "User Role List",
                    ControllerName = "Admin",
                    ActionName = "UserRoleList",
                    ParentMenuId = new Guid("5d4460f6-5fff-40c5-a8f1-294116d88588"),
                    DisplayOrder=1,
                    Visible = false,
                },
                 
                #endregion;
                #region Menu_Sub_Menu
                new NavigationMenu()
                {
                    Id = new Guid("7a956592-76df-40cc-a80e-307eb415bc90"),
                    Name = "Menu Control List",
                    ControllerName = "Admin",
                    ActionName = "MenuControlList",
                    ParentMenuId = new Guid("09657163-3231-4b2c-964d-6fcd8e6f08b6"),
                    DisplayOrder=1,
                    Visible = false,
                },
                  new NavigationMenu()
                {
                    Id = new Guid("d87055b1-e79e-4984-9657-fa73b39ff339"),
                    Name = "Create Menu Control",
                    ControllerName = "Admin",
                    ActionName = "CreateMenuControl",
                    ParentMenuId = new Guid("09657163-3231-4b2c-964d-6fcd8e6f08b6"),
                    DisplayOrder=2,
                    Visible = false,
                },
                   new NavigationMenu()
                {
                    Id = new Guid("55ecc22c-144f-4442-a610-cad0a46e43ce"),
                    Name = "Edit Menu Control",
                    ControllerName = "Admin",
                    ActionName = "EditMenuControl",
                    ParentMenuId = new Guid("09657163-3231-4b2c-964d-6fcd8e6f08b6"),
                    DisplayOrder=3,
                    Visible = false,
                },
                    new NavigationMenu()
                {
                    Id = new Guid("8818148b-70e1-4931-9730-7b52aee402d7"),
                    Name = "Delete Menu Control",
                    ControllerName = "Admin",
                    ActionName = "DeleteMenuControl",
                    ParentMenuId = new Guid("09657163-3231-4b2c-964d-6fcd8e6f08b6"),
                    DisplayOrder=4,
                    Visible = false,
                },
                #endregion;

                #region RolePermission_Sub_Menu
                 new NavigationMenu()
                {
                    Id = new Guid("65a262c5-e697-4c9b-96f5-1654c393bbeb"),
                    Name = "Edit Role Permission",
                    ControllerName = "Admin",
                    ActionName = "EditRolePermission",
                    ParentMenuId = new Guid("2513c5b5-b4f3-4d99-b3aa-d2ddda555465"),
                    DisplayOrder=1,
                    Visible = false,
                },
                 new NavigationMenu()
                {
                    Id = new Guid("d30ebb41-4113-404c-855c-4aad6c98bd3b"),
                    Name = "Node Permission List",
                    ControllerName = "Admin",
                    ActionName = "PermissionList",
                    ParentMenuId = new Guid("2513c5b5-b4f3-4d99-b3aa-d2ddda555465"),
                    DisplayOrder=1,
                    Visible = false,
                },
                #endregion;



               
                
                #endregion;
               
            };
        }
    }
}
