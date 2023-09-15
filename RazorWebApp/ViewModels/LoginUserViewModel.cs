
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;


namespace RazorWebApp.ViewModels
{
    public class LoginUserViewModel
    {
        public string Id { get; set; }
        public string FirstName_EN { get; set; }
        public string LastName_EN { get; set; }
        public string FirstName_AR { get; set; }
        public string LastName_AR { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PhotoUrl { get; set; }
    }
    public class LoginUserInfoViewModel
    {
        public string Name_EN { get; set; }
        public string Name_AR { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PhotoUrl { get; set; }
        public string Language { get; set; }
        public string LanguageBtn { get; set; }
        public string LanguageDirection { get; set; }
    }


    public class PostUserViewModel
    {
        public string FirstName_EN { get; set; }
        public string LastName_EN { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }

    }

}