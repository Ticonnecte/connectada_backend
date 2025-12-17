using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CidConnectada.Tests.Models;

namespace CidConnectada.Tests
{
    public class AuthenticationService
    {
        public AuthenticationService(HttpClient client)
        {
            _client = client;
        }
        private HttpClient _client { get; }
        public async Task<LoginResponseDto> SignIn(LoginRequestDto model)
        {
            // Arrange
            LoginResponseDto response = null;
            IDictionary<string, string> credentials = new Dictionary<string, string>();
            credentials.Add("username", model.username);
            credentials.Add("password", model.password);
            credentials.Add("device_id", model.device_id);
            credentials.Add("device_name", model.device_name);
            credentials.Add("device_type", model.device_type);
            credentials.Add("grant_type", model.grant_type);

            FormUrlEncodedContent tokenForm = new FormUrlEncodedContent(credentials);

            // Act
            using (HttpResponseMessage login = await _client.PostAsync("/Token", tokenForm))
            {
                response = await login.Content.ReadFromJsonAsync<LoginResponseDto>();
            }
            return response;
        }

        public async Task SignInFuncionario()
        {
            LoginResponseDto response = null;
            LoginRequestDto loginDto = new LoginRequestDto
            {
                username = "funcionarioteste@hie.tec.br",
                password = "Abc12345!",
                grant_type = "password",
                device_id = "88144686-A2F6-4E79-93EA-37FD938C6F82",
                device_name = "PCZAO2",
                device_type = "Desktop"
            };

            FormUrlEncodedContent tokenForm = new FormUrlEncodedContent(loginDto.ToKeyValuePairs());

            using (HttpResponseMessage login = await _client.PostAsync("/Token", tokenForm))
            {
                response = await login.Content.ReadFromJsonAsync<LoginResponseDto>();
            }
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", response.access_token);
        }
    }
}