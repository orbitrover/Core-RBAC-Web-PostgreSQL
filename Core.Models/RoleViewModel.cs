namespace Core.Models
{
	public class RoleViewModel
	{
		public string? Id { get; set; }

		public string? Name { get; set; }
        public string? ParentRoleId { get; set; }
        public bool Selected { get; set; }
	}
}