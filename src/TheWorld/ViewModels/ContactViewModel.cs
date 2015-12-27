using System;
using System.ComponentModel.DataAnnotations;

namespace TheWorld.ViewModels
{
    public class ContactViewModel
    {
        [Required] //Data Annotations 
        [StringLength (255, MinimumLength =5)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 5)]
        public string Message { get; set; }
    }
}
