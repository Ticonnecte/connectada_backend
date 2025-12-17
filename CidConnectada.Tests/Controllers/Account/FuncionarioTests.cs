using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using CidConnectada.Tests.Models;
using CidConnectada.Tests.Routes.Account;
using Xunit;

namespace CidConnectada.Tests.Controllers.Account
{
    public class FuncionarioTests
    {
        public FuncionarioTests()
        {
            Client = new HttpClient();
            Client.BaseAddress = new Uri("http://localhost:1409");
            authService = new AuthenticationService(Client);
        }
        public HttpClient Client { get; set; }
        public AuthenticationService authService { get; set; }

        // [Fact]
        // public async void RegistrarFuncionario_DeveRetornar200()
        // {
        //     // Arrange
        //     PostFuncionarioResponseDto response = null;
        //     var userPostDto = new
        //     {
        //         email = "funcionarioxUnitTest@hie.tec.br",
        //         password = "Teste-12345",
        //         deviceId = "88144686-A2F6-4E79-93EA-37FD938C6F82",
        //         deviceName = "PcGamer",
        //         deviceType = "Desktop",
        //         tenantId = 1
        //     };
        //     string serializedContent = JsonSerializer.Serialize(userPostDto);
        //     StringContent content = new StringContent(serializedContent, Encoding.UTF8, "application/json");
        //
        //     // Act
        //     using (HttpResponseMessage postFuncionario = await Client.PostAsync(FuncionarioRoutes.Post, content))
        //     {
        //         response = await postFuncionario.Content.ReadFromJsonAsync<PostFuncionarioResponseDto>();
        //
        //         // Assert
        //         Assert.True(postFuncionario.IsSuccessStatusCode);
        //         Assert.True(response.key > 0);
        //     }
        // }
    }
}