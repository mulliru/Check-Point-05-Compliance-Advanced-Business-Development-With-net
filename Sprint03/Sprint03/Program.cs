using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Oracle.EntityFrameworkCore;
using Sprint03.Infrastructure.Data;
using Sprint03.Infrastructure.Repositories;
using Swashbuckle.AspNetCore.Annotations;

var builder = WebApplication.CreateBuilder(args);

// <summary>
// Configura��o do banco de dados Oracle.
// </summary>
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("OracleConnection")));

// <summary>
// Registra o reposit�rio NomeUsuarioRepository para inje��o de depend�ncia.
// </summary>
builder.Services.AddScoped<NomeUsuarioRepository>();

// <summary>
// Adiciona suporte a Controllers na API.
// </summary>
builder.Services.AddControllers();

// <summary>
// Configura��o do Swagger/OpenAPI para documenta��o da API.
// </summary>
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Sprint03 API",
        Version = "v1",
        Description = "API para gerenciamento de usu�rios."
    });

    // Habilita suporte para anota��es no Swagger
    options.EnableAnnotations();
});

var app = builder.Build();

// <summary>
// Se o ambiente for de desenvolvimento, ativa o Swagger para documenta��o da API.
// </summary>
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// <summary>
// Aplica redirecionamento HTTPS para seguran�a.
// </summary>
app.UseHttpsRedirection();

// <summary>
// Habilita autoriza��o para proteger os endpoints, se necess�rio.
// </summary>
app.UseAuthorization();

// <summary>
// Mapeia as rotas dos controllers para a API.
// </summary>
app.MapControllers();

// <summary>
// Inicia a aplica��o.
// </summary>
app.Run();

/// <summary>
/// Classe principal da aplicação para testes de integração.
/// </summary>
public partial class Program { }
