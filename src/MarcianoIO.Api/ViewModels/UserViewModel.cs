using System.ComponentModel.DataAnnotations;

namespace MarcianoIO.Api.ViewModels
{
    public class RegisterUserViewModel
    {
        [Required(ErrorMessage ="O campo {0} é obrigatorio")]
        [EmailAddress(ErrorMessage ="O campo {0} está em formato inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        [StringLength(100,ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength =6)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage ="As senhas não conferem.")]
        public string ConfirmPassworf { get; set; }
    }

    public class LoginUserViewModel
    {
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        [EmailAddress(ErrorMessage = "O campo {0} está em formato inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 6)]
        public string Password { get; set; }
    }
}
