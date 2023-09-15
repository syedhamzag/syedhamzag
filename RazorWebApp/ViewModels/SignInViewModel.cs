using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;


namespace RazorWebApp.ViewModels
{
    public class SignInViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string wanIp { get; set; } = "string";
        public string Header { get; set; } = "string";
        public string FcmToken { get; set; } = "string";
        public bool ActiveDirectory { get; set; } = false;
        public bool RememberMe {get;set;} = false;
    }
    public class SignInExternalViewModel
    {
        [Required]
        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; }

        public string PhoneNumber { get; set; }
        public string PhotoUrl { get; set; }
        public string FirstName_EN { get; set; }
        public string LastName_EN { get; set; }
        public string ExternalId { get; set; }


    }


}