namespace Core.Models
{
	public class UserViewModel
	{
		public string? Id { get; set; }
		public string? UserName { get; set; }
		public string? NormalizedUserName { get; set; }
		public string? Email { get; set; }
		public string? NormalizedEmail { get; set; }
		public bool EmailConfirmed { get; set; } = false;
        public string? PasswordHash { get; set; }
		public string? SecurityStamp { get; set; }
		public string? ConcurrencyStamp { get; set; }
		public string? PhoneNumber { get; set; }
		public bool PhoneNumberConfirmed { get; set; } = false;
        public bool TwoFactorEnabled { get; set; } = false;
        public DateTime? LockoutEnd { get; set; }
		public bool LockoutEnabled { get; set; } = false;
        public int? AccessFailedCount { get; set; }
        public bool IsApproved { get; set; } = false;
        public bool IsOnline { get; set; } = false;
        public int OnlineActiveDurationInMs { get; set; } = 0;
        public bool IsSystemAdmin { get; set; } = false;

        public RoleViewModel[]? Roles { get; set; }
	}
}