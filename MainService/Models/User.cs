
using System.ComponentModel.DataAnnotations;

namespace MainService.Models
{
    public partial class User
    {
        [Key]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int? Role { get; set; }
        public bool? IsBlocked { get; set; }
        public int? EmployeeId { get; set; }
    }
}
