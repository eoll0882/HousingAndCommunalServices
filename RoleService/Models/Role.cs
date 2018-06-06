using System.ComponentModel.DataAnnotations;

namespace RoleService.Models
{
    public partial class Role
    {
        [Key]
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
}
