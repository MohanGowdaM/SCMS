using Newtonsoft.Json;
using SmartClinic.Web.Models.Account;
using System.Text;

namespace SmartClinic.Web.Services
{
    public class AuthApiService
    {
        private readonly HttpClient _httpClient;

        public AuthApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<LoginResponseViewModel?>LoginAsync(LoginViewModel model)
        {
            var json =JsonConvert.SerializeObject(model);

            var content =new StringContent(json,Encoding.UTF8,"application/json");

            var response =await _httpClient.PostAsync("Auth/login", content);

            if (!response.IsSuccessStatusCode)
                return null;

            string result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<LoginResponseViewModel>(result);
        }
    }
}
