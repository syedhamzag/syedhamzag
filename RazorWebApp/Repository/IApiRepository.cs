using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Text;
using RazorWebApp.Helpers;

namespace RazorWebApp.Repository
{
    public interface IApiRepository
    {
        Task<ApiResponse> GetDataAsync(string _Url, Dictionary<string, string> _Parameters);
        Task<ApiResponse> GetDataByIdAsync(string _Url, Dictionary<string, string> _Parameters);
        Task<ApiResponse> AddAsync(string _Url, Dictionary<string, string> _Parameters, object model);
        Task<ApiResponse> AddFormDataAsync(string _Url, Dictionary<string, string> _Parameters, MultipartFormDataContent model);
        Task<ApiResponse> UpdateAsync(string _Url, Dictionary<string, string> _Parameters, object model);
        Task<ApiResponse> UpdateFormDataAsync(string _Url, Dictionary<string, string> _Parameters, MultipartFormDataContent model);
        Task<ApiResponse> DeleteAsync(string _Url, Dictionary<string, string> _Parameters);
    }
    public class ApiRepository : IApiRepository
    {

        ConfigurationHelper _ConfigurationHelper = new ConfigurationHelper();
        string _BaseUri = "";
        public ApiRepository()
        {
            _BaseUri = _ConfigurationHelper.GetGWAPI() + "/api/Admin/v1/";
        }
        public async Task<ApiResponse> GetDataAsync(string _Url, Dictionary<string, string> _Parameters)
        {
            ApiResponse apiResponse = new ApiResponse();
            try
            {
                HttpClient client = new HttpClient();

                client.BaseAddress = new Uri(_BaseUri);
                client.DefaultRequestHeaders.Accept.Clear();
                foreach (var item in _Parameters)
                {
                    client.DefaultRequestHeaders.Add(item.Key, item.Value);
                }
                HttpResponseMessage response = await client.GetAsync(_Url);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var _JsonData = response.Content.ReadAsStringAsync().Result;
                    if (_JsonData != null)
                    {
                        var _ApiResponse = JsonConvert.DeserializeObject<ApiResponse>(_JsonData.ToString());
                        apiResponse = _ApiResponse;
                    }
                }
                else
                {
                    apiResponse.statusCode = response.StatusCode.ToString();
                    apiResponse.message = response.ReasonPhrase;
                }
                return apiResponse;
            }
            catch (Exception e)
            {

                string innerexp = e.InnerException == null ? e.Message : e.Message + " Inner Error : " + e.InnerException.ToString();
                apiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString();
                apiResponse.message = innerexp;
                return apiResponse;
            }
        }
        public async Task<ApiResponse> GetDataByIdAsync(string _Url, Dictionary<string, string> _Parameters)
        {
            ApiResponse apiResponse = new ApiResponse();
            try
            {
                HttpClient client = new HttpClient();

                client.BaseAddress = new Uri(_BaseUri);
                client.DefaultRequestHeaders.Accept.Clear();
                foreach (var item in _Parameters)
                {
                    client.DefaultRequestHeaders.Add(item.Key, item.Value);
                }
                HttpResponseMessage response = await client.GetAsync(_Url);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var _JsonData = response.Content.ReadAsStringAsync().Result;
                    if (_JsonData != null)
                    {
                        var _ApiResponse = JsonConvert.DeserializeObject<ApiResponse>(_JsonData.ToString());
                        apiResponse = _ApiResponse;
                    }
                }
                else
                {
                    apiResponse.statusCode = response.StatusCode.ToString();
                    apiResponse.message = response.ReasonPhrase;
                }
                return apiResponse;
            }
            catch (Exception e)
            {

                string innerexp = e.InnerException == null ? e.Message : e.Message + " Inner Error : " + e.InnerException.ToString();
                apiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString();
                apiResponse.message = innerexp;
                return apiResponse;
            }
        }

        public async Task<ApiResponse> AddAsync(string _Url, Dictionary<string, string> _Parameters, object model)
        {
            ApiResponse apiResponse = new ApiResponse();
            try
            {
                HttpClient client = new HttpClient();

                client.BaseAddress = new Uri(_BaseUri);
                client.DefaultRequestHeaders.Accept.Clear();
                foreach (var item in _Parameters)
                {
                    client.DefaultRequestHeaders.Add(item.Key, item.Value);
                }
                var data = System.Text.Json.JsonSerializer.Serialize(model);
                var requestContent = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(_Url, requestContent);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var _JsonData = response.Content.ReadAsStringAsync().Result;
                    if (_JsonData != null)
                    {
                        var _ApiResponse = JsonConvert.DeserializeObject<ApiResponse>(_JsonData.ToString());
                        apiResponse = _ApiResponse;
                    }
                }
                else
                {
                    apiResponse.statusCode = response.StatusCode.ToString();
                    apiResponse.message = response.ReasonPhrase;
                }
                return apiResponse;
            }
            catch (Exception e)
            {

                string innerexp = e.InnerException == null ? e.Message : e.Message + " Inner Error : " + e.InnerException.ToString();
                apiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString();
                apiResponse.message = innerexp;
                return apiResponse;
            }
        }
        public async Task<ApiResponse> AddFormDataAsync(string _Url, Dictionary<string, string> _Parameters, MultipartFormDataContent requestContent)
        {
            ApiResponse apiResponse = new ApiResponse();
            try
            {
                HttpClient client = new HttpClient();

                client.BaseAddress = new Uri(_BaseUri);
                client.DefaultRequestHeaders.Accept.Clear();
                foreach (var item in _Parameters)
                {
                    client.DefaultRequestHeaders.Add(item.Key, item.Value);
                }

                HttpResponseMessage response = await client.PostAsync(_Url, requestContent);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var _JsonData = response.Content.ReadAsStringAsync().Result;
                    if (_JsonData != null)
                    {
                        var _ApiResponse = JsonConvert.DeserializeObject<ApiResponse>(_JsonData.ToString());
                        apiResponse = _ApiResponse;
                    }
                }
                else
                {
                    apiResponse.statusCode = response.StatusCode.ToString();
                    apiResponse.message = response.ReasonPhrase;
                }
                return apiResponse;
            }
            catch (Exception e)
            {

                string innerexp = e.InnerException == null ? e.Message : e.Message + " Inner Error : " + e.InnerException.ToString();
                apiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString();
                apiResponse.message = innerexp;
                return apiResponse;
            }
        }
        public async Task<ApiResponse> UpdateAsync(string _Url, Dictionary<string, string> _Parameters, object model)
        {
            ApiResponse apiResponse = new ApiResponse();
            try
            {
                HttpClient client = new HttpClient();

                client.BaseAddress = new Uri(_BaseUri);
                client.DefaultRequestHeaders.Accept.Clear();
                foreach (var item in _Parameters)
                {
                    client.DefaultRequestHeaders.Add(item.Key, item.Value);
                }

                var data = System.Text.Json.JsonSerializer.Serialize(model);
                var requestContent = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync(_Url, requestContent);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var _JsonData = response.Content.ReadAsStringAsync().Result;
                    if (_JsonData != null)
                    {
                        var _ApiResponse = JsonConvert.DeserializeObject<ApiResponse>(_JsonData.ToString());
                        apiResponse = _ApiResponse;
                    }
                }
                else
                {
                    apiResponse.statusCode = response.StatusCode.ToString();
                    apiResponse.message = response.ReasonPhrase;
                }
                return apiResponse;
            }
            catch (Exception e)
            {

                string innerexp = e.InnerException == null ? e.Message : e.Message + " Inner Error : " + e.InnerException.ToString();
                apiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString();
                apiResponse.message = innerexp;
                return apiResponse;
            }
        }
        public async Task<ApiResponse> UpdateFormDataAsync(string _Url, Dictionary<string, string> _Parameters, MultipartFormDataContent requestContent)
        {
            ApiResponse apiResponse = new ApiResponse();
            try
            {
                HttpClient client = new HttpClient();

                client.BaseAddress = new Uri(_BaseUri);
                client.DefaultRequestHeaders.Accept.Clear();
                foreach (var item in _Parameters)
                {
                    client.DefaultRequestHeaders.Add(item.Key, item.Value);
                }

                HttpResponseMessage response = await client.PutAsync(_Url, requestContent);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var _JsonData = response.Content.ReadAsStringAsync().Result;
                    if (_JsonData != null)
                    {
                        var _ApiResponse = JsonConvert.DeserializeObject<ApiResponse>(_JsonData.ToString());
                        apiResponse = _ApiResponse;
                    }
                }
                else
                {
                    apiResponse.statusCode = response.StatusCode.ToString();
                    apiResponse.message = response.ReasonPhrase;
                }
                return apiResponse;
            }
            catch (Exception e)
            {

                string innerexp = e.InnerException == null ? e.Message : e.Message + " Inner Error : " + e.InnerException.ToString();
                apiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString();
                apiResponse.message = innerexp;
                return apiResponse;
            }
        }
        public async Task<ApiResponse> DeleteAsync(string _Url, Dictionary<string, string> _Parameters)
        {
            ApiResponse apiResponse = new ApiResponse();
            try
            {
                HttpClient client = new HttpClient();

                client.BaseAddress = new Uri(_BaseUri);
                client.DefaultRequestHeaders.Accept.Clear();
                foreach (var item in _Parameters)
                {
                    client.DefaultRequestHeaders.Add(item.Key, item.Value);
                }
                HttpResponseMessage response = await client.DeleteAsync(_Url);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var _JsonData = response.Content.ReadAsStringAsync().Result;
                    if (_JsonData != null)
                    {
                        var _ApiResponse = JsonConvert.DeserializeObject<ApiResponse>(_JsonData.ToString());
                        apiResponse = _ApiResponse;
                    }
                }
                else
                {
                    apiResponse.statusCode = response.StatusCode.ToString();
                    apiResponse.message = response.ReasonPhrase;
                }
                return apiResponse;
            }
            catch (Exception e)
            {

                string innerexp = e.InnerException == null ? e.Message : e.Message + " Inner Error : " + e.InnerException.ToString();
                apiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString();
                apiResponse.message = innerexp;
                return apiResponse;
            }
        }

    }
}