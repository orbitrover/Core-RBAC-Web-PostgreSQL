using Core.Models;
using Core.Models.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Core.Repo
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<IdentityUserExtended> Users { get; set; }
        public DbSet<IdentityRoleExtended> Roles {  get; set; }
        //public DbSet<ValueTypeGroupMaster> ValueTypeGroupMaster { get; set; }
        //public DbSet<ValueTypeMaster> ValueTypeMaster { get; set; }
        public DbSet<NavigationMenu> NavigationMenu { get; set; }
        public DbSet<RoleMenuPermission> RoleMenuPermission { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<RoleMenuPermission>().HasKey(c => new { c.RoleId, c.NavigationMenuId });

            foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
            base.OnModelCreating(builder);
            //builder.Entity<ValueTypeMaster>()
            //    .HasOne(a => a.ValueTypeGroup)
            //    .WithMany(b => b.ValueType)
            //    .HasForeignKey(c => c.ValueTypeGroupId)
            //    .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
