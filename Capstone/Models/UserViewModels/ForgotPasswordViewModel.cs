using System;
using System.ComponentModel.DataAnnotations;

namespace Capstone.Models.UserViewModels
{
    public class ForgotPasswordViewModel
    {
            [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
    
}
