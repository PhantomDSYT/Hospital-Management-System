using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace HMS.ViewModels
{
    public class CreateRoleViewModel
    {
        [Required]
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }
    }
}
