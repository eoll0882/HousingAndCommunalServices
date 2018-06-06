using System.ComponentModel.DataAnnotations;

namespace MainService.ViewModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Не указан логин")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Не указан пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
