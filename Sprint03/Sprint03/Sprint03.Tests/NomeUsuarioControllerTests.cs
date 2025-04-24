
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

        /// <summary>
        /// Construtor que inicializa o cliente HTTP para os testes de integração.
        /// </summary>
        public NomeUsuarioControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        /// <summary>
        /// Deve retornar todos os usuários com sucesso.
        /// </summary>
        [Fact]
        public async Task GetAll_ShouldReturnSuccess()
        {
            var response = await _client.GetAsync("/api/nomeusuarios");
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        
        /// <summary>
        /// Deve criar um novo usuário e retornar status 201.
        /// </summary>
        [Fact]
        public async Task Create_ShouldReturnCreatedUser()
        {
            var newUser = new
            {
                Name = "Murillo Ramos",
                Email = "murillo@example.com",
                BirthDate = new DateTime(2005, 7, 14),
                PhoneNumber = "11999999999"

            };

            var response = await _client.PostAsJsonAsync("/api/nomeusuarios", newUser);

            response.EnsureSuccessStatusCode(); // Verifica 2xx
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var responseBody = await response.Content.ReadAsStringAsync();
            Assert.Contains("Murillo Ramos", responseBody);
        }
        
        /// <summary>
        /// Deve buscar um usuário pelo ID e retornar os dados corretamente.
        /// </summary>
        [Fact]
        public async Task GetById_ShouldReturnUser()
        {
            var newUser = new
            {
                Name = "Murillo Ramos",
                Email = "murillo@example.com",
                BirthDate = new DateTime(2005, 7, 14),
                PhoneNumber = "11999999999"
            };

            var postResponse = await _client.PostAsJsonAsync("/api/nomeusuarios", newUser);
            postResponse.EnsureSuccessStatusCode();
    
            var createdUser = await postResponse.Content.ReadFromJsonAsync<Dictionary<string, object>>();
            if (createdUser == null || !createdUser.ContainsKey("id"))
                throw new Exception("A resposta da API não contém o campo 'id'.");

            var id = createdUser["id"]?.ToString();

            var getResponse = await _client.GetAsync($"/api/nomeusuarios/{id}");
            getResponse.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

            var responseBody = await getResponse.Content.ReadAsStringAsync();
            Assert.Contains("Murillo Ramos", responseBody);
        }
        
        /// <summary>
        /// Deve atualizar os dados de um usuário existente.
        /// </summary>
        [Fact]
        public async Task Update_ShouldModifyUser()
        {
            var newUser = new
            {
                Name = "Usuário Teste PUT",
                Email = "put@example.com",
                BirthDate = new DateTime(1995, 1, 1),
                PhoneNumber = "11999999999"
            };

            var createResponse = await _client.PostAsJsonAsync("/api/nomeusuarios", newUser);
            createResponse.EnsureSuccessStatusCode();

            var createdContent = await createResponse.Content.ReadAsStringAsync();
            var createdUser = JsonSerializer.Deserialize<JsonElement>(createdContent);
            var id = createdUser.GetProperty("id").GetInt32();

            var updatedUser = new
            {
                Id = id,
                Name = "Usuário Atualizado",
                Email = "atualizado@example.com",
                BirthDate = new DateTime(1990, 1, 1),
                PhoneNumber = "11888888888"
            };

            var putResponse = await _client.PutAsJsonAsync($"/api/nomeusuarios/{id}", updatedUser);

            // Mostra a resposta do servidor mesmo se der erro
            var responseContent = await putResponse.Content.ReadAsStringAsync();
            Console.WriteLine("❗ RESPOSTA DO SERVIDOR (PUT): " + responseContent);

            // Só valida o status depois de ver a resposta
            putResponse.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, putResponse.StatusCode);

            var getResponse = await _client.GetAsync($"/api/nomeusuarios/{id}");
            getResponse.EnsureSuccessStatusCode();
            var responseBody = await getResponse.Content.ReadAsStringAsync();

            Assert.Contains("Usuário Atualizado", responseBody);

        }

        /// <summary>
        /// Deve deletar um usuário e garantir que ele não possa mais ser acessado.
        /// </summary>
        [Fact]
        public async Task Delete_ShouldRemoveUser()
        {
            var newUser = new
            {
                Name = "Murillo Ramos",
                Email = "murillo@example.com",
                BirthDate = new DateTime(2005, 7, 14),
                PhoneNumber = "11999999999"
            };

            var postResponse = await _client.PostAsJsonAsync("/api/nomeusuarios", newUser);
            postResponse.EnsureSuccessStatusCode();

            var createdUser = await postResponse.Content.ReadFromJsonAsync<Dictionary<string, object>>();
            if (createdUser == null || !createdUser.ContainsKey("id"))
                throw new Exception("A resposta da API não contém o campo 'id'.");

            var id = createdUser["id"]?.ToString();
            
            var deleteResponse = await _client.DeleteAsync($"/api/nomeusuarios/{id}");
            deleteResponse.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);

            var getResponse = await _client.GetAsync($"/api/nomeusuarios/{id}");
            Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
        }
    }
}
