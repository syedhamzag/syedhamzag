using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Text;

namespace RazorWebApp.Helpers
{
    public class ApiResponse
    {
        public string statusCode{get;set;}
        public object message{get;set;}
        public IEnumerable<string> error{get;set;}
        public object data{get;set;}

    }
}