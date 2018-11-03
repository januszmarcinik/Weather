using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Weather.WebUI.Models
{
    public class SignUpViewModel : LoginViewModel
    {
        [Required(ErrorMessage = "Proszę potwierdzić hasło.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}