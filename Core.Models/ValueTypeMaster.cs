using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public class ValueTypeMaster : BaseEntity
    {
        public ValueTypeMaster() { }
        [Key]
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ValueTypeGroupId { get; set; }

        [ForeignKey("ValueTypeGroupId")]
        public ValueTypeGroupMaster ValueTypeGroup { get; set; }
    }
}
