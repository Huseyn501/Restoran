using System.ComponentModel.DataAnnotations;

namespace RestoranMVC.ViewModels
{
    public class LoginVm
    {
        [Required]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
