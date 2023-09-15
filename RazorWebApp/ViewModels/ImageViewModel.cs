using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RazorWebApp.ViewModels
{
    public class ImageJsonViewModel
    {
        public string ImageLocation { get; set; }
        public string ImageLogoUrl { get; set; }
    }
    public class PageJsonViewModel
    {
        public List<string> Pages { get; set; }
    }

    public class ImagePath
    {
        public string FolderPath { get; set; }
        public string ImageUrlPath { get; set; }
    }

}