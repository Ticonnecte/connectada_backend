using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using CidConnectada.Tests.Attributes;
using CidConnectada.Tests.Routes.Noticias;
using CidConnectada.Webapi.Models.Noticias;
using Xunit;

namespace CidConnectada.Tests.Controllers.Noticias
{
    public class NoticiaCategoriaTests
    {
        public NoticiaCategoriaTests()
        {
            Client = new HttpClient();
            Client.BaseAddress = new Uri("http://localhost:1409");
            authService = new AuthenticationService(Client);
        }
        public HttpClient Client { get; set; }
        public AuthenticationService authService { get; set; }

        
        
        [Fact]
        public async void GetOneNoticiaCategoria_DeveRetornar200()
        {
            // Arrange
            IList<NoticiaCategoriaDto> response = null;
            await authService.SignInFuncionario();
            string uri = $"{NoticiaCategoriaRoutes.GetOne}?id={4}";
            
            // Act
            using (HttpResponseMessage getAllNoticiaCategoria = await Client.GetAsync(uri))
            {
                response = await getAllNoticiaCategoria.Content.ReadFromJsonAsync<IList<NoticiaCategoriaDto>>();

                // Assert
                Assert.True(getAllNoticiaCategoria.IsSuccessStatusCode);
                Assert.True(response.Any());
            }
        }
        
        
        [Fact]
        public async void GetAllNoticiaCategoria_DeveRetornar200()
        {
            // Arrange
            IList<NoticiaCategoriaDto> response = null;

            // Act
            using (HttpResponseMessage getAllNoticiaCategoria = await Client.GetAsync(NoticiaCategoriaRoutes.GetAll))
            {
                response = await getAllNoticiaCategoria.Content.ReadFromJsonAsync<IList<NoticiaCategoriaDto>>();

                // Assert
                Assert.True(getAllNoticiaCategoria.IsSuccessStatusCode);
                Assert.True(response.Any());
            }
        }
    }
}