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
using RazorWebApp.ViewModels;
using Newtonsoft.Json;
using RazorWebApp.Helpers;

namespace RazorWebApp.Helpers
{
    public class SecurityHelper
    {
        string _BaseUri = ""; string _Url = "";
        ConfigurationHelper _ConfigurationHelper = new ConfigurationHelper();
        static string _UserId = "";
        public SecurityHelper()
        {
            _BaseUri = _ConfigurationHelper.GetGWAPI() + "/api/Admin/v1/";
        }

        public async Task<ApiResponse> LoginAsync(SignInViewModel _SignInViewModel)
        {
            ApiResponse apiResponse = new ApiResponse();
            try
            {
                HttpClient client = new HttpClient();
                _Url = "LMSAuth/Login";
                client.BaseAddress = new Uri(_BaseUri);
                client.DefaultRequestHeaders.Accept.Clear();
                var data = System.Text.Json.JsonSerializer.Serialize(_SignInViewModel);
                var requestContent = new StringContent(data, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(_Url, requestContent);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var _JsonData = response.Content.ReadAsStringAsync().Result;
                    if (_JsonData != null)
                    {
                        var _ApiResponse = JsonConvert.DeserializeObject<ApiResponse>(_JsonData.ToString());
                        if (_ApiResponse.statusCode == StatusCodes.Status200OK.ToString())
                        {
                            var _UserViewModel = JsonConvert.DeserializeObject<LoginUserViewModel>(_ApiResponse.data.ToString());
                            _ApiResponse.data = _UserViewModel;
                            _UserId = _UserViewModel.Id;
                        }
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
            catch (Exception ex)
            {
                apiResponse.message = ex.Message + " " + ex.InnerException;
                return apiResponse;

            }
        }
        public async Task<ApiResponse> LoginExternalAsync(SignInExternalViewModel _SignInViewModel)
        {
            ApiResponse apiResponse = new ApiResponse();
            try
            {
                HttpClient client = new HttpClient();
                _Url = "LMSAuth/LoginExternal";
                client.BaseAddress = new Uri(_BaseUri);
                client.DefaultRequestHeaders.Accept.Clear();
                var data = System.Text.Json.JsonSerializer.Serialize(_SignInViewModel);
                var requestContent = new StringContent(data, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(_Url, requestContent);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var _JsonData = response.Content.ReadAsStringAsync().Result;
                    if (_JsonData != null)
                    {
                        var _ApiResponse = JsonConvert.DeserializeObject<ApiResponse>(_JsonData.ToString());
                        if (_ApiResponse.statusCode == StatusCodes.Status200OK.ToString())
                        {
                            var _UserViewModel = JsonConvert.DeserializeObject<LoginUserViewModel>(_ApiResponse.data.ToString());
                            _ApiResponse.data = _UserViewModel;
                            _UserId = _UserViewModel.Id;

                        }

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
            catch (Exception ex)
            {
                apiResponse.message = ex.Message + " " + ex.InnerException;
                return apiResponse;

            }
        }
        public async Task<ApiResponse> GenerateOTPAsync(string UserId)
        {
            ApiResponse apiResponse = new ApiResponse();
            try
            {
                HttpClient client = new HttpClient();
                _Url = "LMSAuth/OTPGenerate";
                client.BaseAddress = new Uri(_BaseUri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("UserId", UserId);

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
            catch (Exception ex)
            {
                apiResponse.message = ex.Message + " " + ex.InnerException;
                return apiResponse;

            }
        }
        public async Task<ApiResponse> ConfirmOTPAsync(string OTP)
        {
            ApiResponse apiResponse = new ApiResponse();
            try
            {
                if (string.IsNullOrEmpty(_UserId))
                {
                    apiResponse.statusCode = StatusCodes.Status409Conflict.ToString();
                    apiResponse.message = "Invalid User";
                    return apiResponse;
                }
                HttpClient client = new HttpClient();
                _Url = "LMSAuth/ConfirmOTP";
                client.BaseAddress = new Uri(_BaseUri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("OTP", OTP);
                client.DefaultRequestHeaders.Add("UserId", _UserId);

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
            catch (Exception ex)
            {
                apiResponse.message = ex.Message + " " + ex.InnerException;
                return apiResponse;

            }
        }
        public async Task<ApiResponse> GetUserLoginInfoAsync(string Key)
        {
            ApiResponse apiResponse = new ApiResponse();
            try
            {

                HttpClient client = new();
                _Url = "LMSAuth/UserLoginInfo";
                client.BaseAddress = new Uri(_BaseUri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Key);

                HttpResponseMessage response = await client.GetAsync(_Url);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var _JsonData = response.Content.ReadAsStringAsync().Result;
                    if (_JsonData != null)
                    {
                        var _ApiResponse = JsonConvert.DeserializeObject<ApiResponse>(_JsonData.ToString());
                        if (_ApiResponse.statusCode == StatusCodes.Status200OK.ToString())
                        {
                            var _LoginUserViewModel = JsonConvert.DeserializeObject<LoginUserViewModel>(_ApiResponse.data.ToString());
                            _ApiResponse.data = _LoginUserViewModel;


                        }
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
            catch (Exception ex)
            {
                apiResponse.message = ex.Message + " " + ex.InnerException;
                return apiResponse;

            }
        }
    }
}