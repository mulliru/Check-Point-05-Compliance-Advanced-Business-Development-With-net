
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Sprint03.Tests
{
    /// <summary>
    /// Testes de integração para o NomeUsuarioController
    /// </summary>
    public class NomeUsuarioControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public NomeUsuarioControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        /// <summary>
        /// Deve retornar todos os usuários com sucesso
        /// </summary>
        [Fact]
        public async Task GetAll_ShouldReturnSuccess()
        {
            var response = await _client.GetAsync("/api/nomeusuarios");
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData("Murillo Ramos", "murillo@example.com", "2005-07-14", "11999999999")]
        [InlineData("Pedro Silva", "pedro@example.com", "1990-05-20", "11988888888")]
        public async Task Create_ShouldReturnCreatedUser(string name, string email, string birthDateStr, string phoneNumber)
        {
            var newUser = new
            {
                Name = name,
                Email = email,
                BirthDate = DateTime.Parse(birthDateStr),
                PhoneNumber = phoneNumber
            };

            var response = await _client.PostAsJsonAsync("/api/nomeusuarios", newUser);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var responseBody = await response.Content.ReadAsStringAsync();
            Assert.Contains(name, responseBody);
        }

        [Theory]
        [InlineData("Usuário GetById", "getbyid@example.com", "1995-01-01", "11911111111")]
        public async Task GetById_ShouldReturnUser(string name, string email, string birthDateStr, string phoneNumber)
        {
            var newUser = new
            {
                Name = name,
                Email = email,
                BirthDate = DateTime.Parse(birthDateStr),
                PhoneNumber = phoneNumber
            };

            var postResponse = await _client.PostAsJsonAsync("/api/nomeusuarios", newUser);
            postResponse.EnsureSuccessStatusCode();

            var createdUser = await postResponse.Content.ReadFromJsonAsync<Dictionary<string, object>>();
            var id = createdUser["id"]?.ToString();

            var getResponse = await _client.GetAsync($"/api/nomeusuarios/{id}");
            getResponse.EnsureSuccessStatusCode();
            var responseBody = await getResponse.Content.ReadAsStringAsync();
            Assert.Contains(name, responseBody);
        }

        [Theory]
        [InlineData("Usuário PUT", "put@example.com", "1995-01-01", "11999999999", "Usuário Atualizado", "atualizado@example.com", "1990-01-01", "11888888888")]
        public async Task Update_ShouldModifyUser(string originalName, string originalEmail, string originalBirthDateStr, string originalPhone,
                                                  string updatedName, string updatedEmail, string updatedBirthDateStr, string updatedPhone)
        {
            var newUser = new
            {
                Name = originalName,
                Email = originalEmail,
                BirthDate = DateTime.Parse(originalBirthDateStr),
                PhoneNumber = originalPhone
            };

            var createResponse = await _client.PostAsJsonAsync("/api/nomeusuarios", newUser);
            createResponse.EnsureSuccessStatusCode();
            var createdContent = await createResponse.Content.ReadAsStringAsync();
            var createdUser = JsonSerializer.Deserialize<JsonElement>(createdContent);
            var id = createdUser.GetProperty("id").GetInt32();

            var updatedUser = new
            {
                Id = id,
                Name = updatedName,
                Email = updatedEmail,
                BirthDate = DateTime.Parse(updatedBirthDateStr),
                PhoneNumber = updatedPhone
            };

            var putResponse = await _client.PutAsJsonAsync($"/api/nomeusuarios/{id}", updatedUser);
            putResponse.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, putResponse.StatusCode);

            var getResponse = await _client.GetAsync($"/api/nomeusuarios/{id}");
            getResponse.EnsureSuccessStatusCode();
            var responseBody = await getResponse.Content.ReadAsStringAsync();
            Assert.Contains(updatedName, responseBody);
        }

        [Theory]
        [InlineData("Usuário DELETE", "delete@example.com", "1995-01-01", "11999999999")]
        public async Task Delete_ShouldRemoveUser(string name, string email, string birthDateStr, string phoneNumber)
        {
            var newUser = new
            {
                Name = name,
                Email = email,
                BirthDate = DateTime.Parse(birthDateStr),
                PhoneNumber = phoneNumber
            };

            var postResponse = await _client.PostAsJsonAsync("/api/nomeusuarios", newUser);
            postResponse.EnsureSuccessStatusCode();

            var createdUser = await postResponse.Content.ReadFromJsonAsync<Dictionary<string, object>>();
            var id = createdUser["id"]?.ToString();

            var deleteResponse = await _client.DeleteAsync($"/api/nomeusuarios/{id}");
            deleteResponse.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);

            var getResponse = await _client.GetAsync($"/api/nomeusuarios/{id}");
            Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
        }
    }
}
