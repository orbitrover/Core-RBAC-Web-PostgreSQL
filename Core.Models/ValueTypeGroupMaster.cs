using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class ValueTypeGroupMaster : BaseEntity
    {
        public ValueTypeGroupMaster() { }
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<ValueTypeMaster> ValueType { get; set;}
    }
}
