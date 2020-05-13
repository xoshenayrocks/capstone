using System;
using System.ComponentModel.DataAnnotations;

namespace Capstone.Models.ManageViewModels
{
    public class AddRoleViewModel
    {
        [Display(Name = "Role Name")]
        [Required]
        public string RoleName { get; set; }
    }
}
