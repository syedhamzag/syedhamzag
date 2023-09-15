using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using Newtonsoft.Json;
using RazorWebApp.ViewModels;

namespace RazorWebApp.Helpers
{
    public class ConfigurationHelper
    {


        public string GetGWAPI()
        {
            var configuration = new ConfigurationBuilder()
                           .SetBasePath(Directory.GetCurrentDirectory())
                           .AddJsonFile("appsettings.json", false)
                           .Build();

            return configuration.GetSection("UrlAPI:IFS").Value.ToString();

        }
        public ImageJsonViewModel GetImagesUrl()
        {
            var configuration = new ConfigurationBuilder()
                           .SetBasePath(Directory.GetCurrentDirectory())
                           .AddJsonFile("appsettings.json", false)
                           .Build();

            var _ImageJsonViewModel = configuration.GetSection("Image").Get<ImageJsonViewModel>();
            return _ImageJsonViewModel;

        }

        public PageJsonViewModel GetPages()
        {
            var configuration = new ConfigurationBuilder()
                           .SetBasePath(Directory.GetCurrentDirectory())
                           .AddJsonFile("appsettings.json", false)
                           .Build();

            var _PageJsonViewModel = configuration.GetSection("CMSSlug").Get<PageJsonViewModel>();
            return _PageJsonViewModel;

        }
        public async Task<ApiResponse> DefaultLanguageAsync()
        {
            string _BaseUri = GetGWAPI() + "/api/Translation/v1/";


            ApiResponse apiResponse = new ApiResponse();
            HttpClient client = new HttpClient();
            string _Url = "LOVServices/GetDefaultLanguage";
            client.BaseAddress = new Uri(_BaseUri);
            client.DefaultRequestHeaders.Accept.Clear();

            HttpResponseMessage response = await client.GetAsync(_Url);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var _JsonData = response.Content.ReadAsStringAsync().Result;
                if (_JsonData != null)
                {

                    apiResponse = JsonConvert.DeserializeObject<ApiResponse>(_JsonData.ToString());
                }
            }
            else
            {
                apiResponse.statusCode = response.StatusCode.ToString();
                apiResponse.message = response.ReasonPhrase;
            }
            return apiResponse;
        }
    }
}