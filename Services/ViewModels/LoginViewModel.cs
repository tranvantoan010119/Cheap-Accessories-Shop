using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập Email.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Password.")]
        public string Password { get; set; }
    }
}
