
using Microsoft.AspNetCore.Mvc;
using Sprint03.Domain.Entities;
using Sprint03.Infrastructure.Repositories;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Sprint03.Presentation.Controllers
{
    /// <summary>
    /// Controlador para operações de NomeUsuario
    /// </summary>
    [ApiController]
    [Route("api/nomeusuarios")]
    public class NomeUsuarioController : ControllerBase
    {
        private readonly NomeUsuarioRepository _repository;

        /// <summary>
        /// Inicializa uma nova instância do controller NomeUsuario.
        /// </summary>
        /// <param name="repository">Instância do repositório de NomeUsuario</param>
        public NomeUsuarioController(NomeUsuarioRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Retorna todos os usuários
        /// </summary>
        [HttpGet]
        [SwaggerOperation(Summary = "Lista todos os usuários", Description = "Retorna uma lista de todos os usuários cadastrados.")]
        [SwaggerResponse(200, "Usuários encontrados com sucesso")]
        public async Task<ActionResult<IEnumerable<NomeUsuario>>> GetAll()
        {
            var users = await _repository.GetAllUsersAsync();
            return Ok(users);
        }

        /// <summary>
        /// Retorna um usuário pelo ID
        /// </summary>
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Busca um usuário por ID", Description = "Retorna um único usuário pelo seu identificador.")]
        [SwaggerResponse(200, "Usuário encontrado com sucesso")]
        [SwaggerResponse(404, "Usuário não encontrado")]
        public async Task<ActionResult<NomeUsuario>> GetById(int id)
        {
            var user = await _repository.GetUserByIdAsync(id);
            if (user == null)
                return NotFound("Usuário não encontrado.");
            return Ok(user);
        }

        /// <summary>
        /// Cria um novo usuário
        /// </summary>
        [HttpPost]
        [SwaggerOperation(Summary = "Cria um novo usuário", Description = "Adiciona um novo usuário ao sistema.")]
        [SwaggerResponse(201, "Usuário criado com sucesso")]
        public async Task<ActionResult<NomeUsuario>> Create([FromBody] NomeUsuario user)
        {
            await _repository.AddUserAsync(user);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        /// <summary>
        /// Atualiza os dados de um usuário existente com base no ID informado.
        /// </summary>
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Atualiza um usuário", Description = "Modifica os dados de um usuário existente.")]
        [SwaggerResponse(200, "Usuário atualizado com sucesso")]
        [SwaggerResponse(404, "Usuário não encontrado")]
        public async Task<IActionResult> Update(int id, [FromBody] NomeUsuario user)
        {
            var existingUser = await _repository.GetUserByIdAsync(id);
            if (existingUser == null)
                return NotFound("Usuário não encontrado.");

            existingUser.Name = user.Name;
            existingUser.Email = user.Email;
            existingUser.PhoneNumber = user.PhoneNumber;
            existingUser.BirthDate = user.BirthDate;

            await _repository.UpdateUserAsync(existingUser);
            return Ok("Usuário atualizado com sucesso.");
        }

        /// <summary>
        /// Remove um usuário com base no ID informado.
        /// </summary>
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Remove um usuário", Description = "Exclui um usuário existente do sistema.")]
        [SwaggerResponse(200, "Usuário removido com sucesso")]
        [SwaggerResponse(404, "Usuário não encontrado")]
        public async Task<IActionResult> Delete(int id)
        {
            var existingUser = await _repository.GetUserByIdAsync(id);
            if (existingUser == null)
                return NotFound("Usuário não encontrado.");

            await _repository.DeleteUserAsync(id);
            return Ok("Usuário removido com sucesso.");
        }
    }
}
