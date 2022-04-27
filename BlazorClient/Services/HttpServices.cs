using BlazorClient.Helpers;
using BlazorClient.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
namespace BlazorClient.Services
{
    // simplify the code for making Htttp requests from other services. and aslo implement some shits below:
    // 1.add JWT token to HTTP Authorization header for API requests when user is logged in
    // 2.automatically logout of the blazor app when a 401-Unauthorized response is received from the API -- for the future permission check and stuff
    // 3.on error response throw an exception with the message from the response body
    public interface IHttpService
    {
        Task<T> Get<T>(string uri);
        Task Post(string uri, object value);
        Task<T> Post<T>(string uri, object value);
        Task Put(string uri, object value);
        Task<T> Put<T>(string uri, object value);
        Task Delete(string uri);
        Task<T> Delete<T>(string uri);
    }
    public class HttpService : IHttpService
    {
        private HttpClient _httpClient;
        private NavigationManager _navigationManager;
        private ILocalStorageService _localStorageService;
        private IConfiguration _configuration;

        public HttpService(HttpClient httpClient, NavigationManager navigationManager, ILocalStorageService localStorageService, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _navigationManager = navigationManager;
            _localStorageService = localStorageService;
            _configuration = configuration;
        }
        public async Task Delete(string uri)
        {
            var request = createRequest(HttpMethod.Delete, uri);
            await sendRequest(request);
        }

        public async Task<T> Delete<T>(string uri)
        {
            var request = createRequest(HttpMethod.Delete, uri);
            return await sendRequest<T>(request);
        }

        public async Task<T> Get<T>(string uri)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            return await sendRequest<T>(request);
        }

        public async Task Post(string uri, object value)
        {
            var request = createRequest(HttpMethod.Post, uri, value);
            await sendRequest(request);
        }

        public async Task<T> Post<T>(string uri, object value)
        {
            var request = createRequest(HttpMethod.Post, uri, value);
            var sendRequestResult = await sendRequest<T>(request);
            return sendRequestResult;
        }

        public async Task Put(string uri, object value)
        {
            var request = createRequest(HttpMethod.Put, uri, value);
            await sendRequest(request);
        }

        public async Task<T> Put<T>(string uri, object value)
        {
            var request = createRequest(HttpMethod.Put, uri, value);
            return await sendRequest<T>(request);
        }

        // helper methods
        private HttpRequestMessage createRequest(HttpMethod method, string uri, object value = null)
        {
            var request = new HttpRequestMessage(method, uri);
            if (value != null)
                // serilization: convert an object into a json string
                // deserilization: convert a json string back in it object state
                request.Content = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json");

            return request;
        }

        private async Task sendRequest(HttpRequestMessage request)
        {
            await addJwtHeader(request);
            //send request
            using var response = await _httpClient.SendAsync(request);

            //auto logout on 401 response
            if(response.StatusCode == HttpStatusCode.Unauthorized)
            {
                // todo: change to fit
                _navigationManager.NavigateTo("account/logout");
                return;
            }
            await handleErrors(response);
        }

        private async Task<T> sendRequest<T>(HttpRequestMessage request)
        {
            await addJwtHeader(request);
            //send request
            using var response = await _httpClient.SendAsync(request);

            //auto logout on 401 response
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                // todo: change to fit
                _navigationManager.NavigateTo("account/logout");
                return default;
            }

            var options = new JsonSerializerOptions();
            options.PropertyNameCaseInsensitive = true;
            options.Converters.Add(new StringConverter());
            var result = await response.Content.ReadFromJsonAsync<T>(options);
            return result;
        }

        private async Task addJwtHeader(HttpRequestMessage request)
        {
            //ad jwt auth header if user is logged in and request is to the api url
            var user = await _localStorageService.GetItem<AccountModel>("user");
            var isApiUrl = !request.RequestUri.IsAbsoluteUri;
            if (user != null && isApiUrl)
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);

        }

        private async Task handleErrors(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
                throw new Exception(error["message"]);
            }
        }
    }
}
