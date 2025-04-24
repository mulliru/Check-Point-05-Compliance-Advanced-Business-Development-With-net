using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sprint03.Domain.Entities
{
    /// <summary>
    /// Representa um usuário no sistema.
    /// </summary>
    public class NomeUsuario
    {
        /// <summary>
        /// Identificador único do usuário.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Nome do usuário.
        /// </summary>
        [Required]
        [MaxLength(100)]
        [Column("NomeUsuario")] // Define o nome da coluna no banco de dados
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Endereço de e-mail do usuário.
        /// </summary>
        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Data de nascimento do usuário.
        /// </summary>
        [Required]
        [Column("NascData")] // Define o nome da coluna no banco de dados
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// Número do telefone do usuário.
        /// </summary>
        [Required]
        [MaxLength(15)]
        public string PhoneNumber { get; set; } = string.Empty;

        /// <summary>
        /// Construtor vazio necessário para evitar erros de inicialização.
        /// </summary>
        public NomeUsuario() { }
    }
}
