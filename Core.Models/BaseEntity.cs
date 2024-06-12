namespace Core.Models
{
    public abstract class BaseEntity
    {
        public string CreatedBy { get; set; } = "1";
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime CreatedOnUtc { get; set; } = DateTime.UtcNow;
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public DateTime? ModifiedOnUtc { get; set; }
        public bool? IsActive { get; set; } = true;
        public bool? IsDeleted { get; set; } = false;
        public string? IsDeletedBy { get; set; }
        public DateTime? IsDeletedOn { get; set; }
        public DateTime? IsDeletedOnUtc { get; set; }
    }
}
