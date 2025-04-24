using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sprint03.Domain.Entities;
using Sprint03.Infrastructure.Data;

namespace Sprint03.Infrastructure.Repositories
{
    /// <summary>
    /// Responsável pelo gerenciamento de usuários no banco de dados.
    /// </summary>
    public class NomeUsuarioRepository
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Inicializa uma nova instância do <see cref="NomeUsuarioRepository"/>.
        /// </summary>
        /// <param name="context">Contexto do banco de dados.</param>
        public NomeUsuarioRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retorna todos os usuários cadastrados.
        /// </summary>
        public async Task<List<NomeUsuario>> GetAllUsersAsync()
        {
            return await _context.NomeUsuarios.ToListAsync();
        }

        /// <summary>
        /// Retorna um usuário pelo ID.
        /// </summary>
        /// <param name="id">ID do usuário.</param>
        public async Task<NomeUsuario?> GetUserByIdAsync(int id)
        {
            return await _context.NomeUsuarios.FindAsync(id);
        }

        /// <summary>
        /// Adiciona um novo usuário ao banco de dados.
        /// </summary>
        /// <param name="user">Usuário a ser adicionado.</param>
        public async Task AddUserAsync(NomeUsuario user)
        {
            _context.NomeUsuarios.Add(user);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Atualiza um usuário existente no banco de dados.
        /// </summary>
        /// <param name="user">Usuário com os novos dados.</param>
        public async Task UpdateUserAsync(NomeUsuario user)
        {
            _context.NomeUsuarios.Update(user);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Remove um usuário pelo ID.
        /// </summary>
        /// <param name="id">ID do usuário a ser removido.</param>
        public async Task DeleteUserAsync(int id)
        {
            var user = await _context.NomeUsuarios.FindAsync(id);
            if (user != null)
            {
                _context.NomeUsuarios.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}
