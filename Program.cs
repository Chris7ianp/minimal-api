using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using minimal.Dominio.Interfaces;
using minimal.Dominio.Servicos;
using minimal_api.Dominio.Entidades;
using minimal_api.Dominio.ModelView;
using minimal_api.DTOs;
using minimal_api.Infraestrutura.Db;


#region Builder
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddScoped<IAdministradorServicos, AdministradorServicos>();
builder.Services.AddScoped<IVeiculosServico, VeiculoServico>();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<DbContexto>(options =>
{
    options.UseMySql(builder.Configuration.GetConnectionString("mysql"),
                     ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("mysql"))
                    );
});

var app = builder.Build();

#endregion

#region Home

app.MapGet("/", () => Results.Json(new Home())).WithTags("Home");

#endregion

#region Administradores

app.MapPost("/administradores/login", ([FromBody] LoginDTO loginDTO, IAdministradorServicos administradorServicos) =>
{
    if (administradorServicos.Login(loginDTO) != null)
    {
        return Results.Ok("Login feito com sucesso");
    }
    else
        return Results.Unauthorized();

}).WithTags("Administradores");


app.MapPost("/administradores", ([FromBody] LoginDTO loginDTO, IAdministradorServicos administradorServicos) =>
{
    if (administradorServicos.Login(loginDTO) != null)
    {
        return Results.Ok("Login feito com sucesso");
    }
    else
        return Results.Unauthorized();

}).WithTags("Administradores");

#endregion

#region Veiculos

ErroDeValidacoes validaDTO(VeiculoDTO veiculoDTO)
{
    var validacoes = new ErroDeValidacoes
    {
        Mensagem = new List<string>()
    };

    if (string.IsNullOrEmpty(veiculoDTO.Nome))
    {
        validacoes.Mensagem.Add("O nome do veiculo n�o pode ser vazio");
    }

    if (string.IsNullOrEmpty(veiculoDTO.Marca))
    {
        validacoes.Mensagem.Add("A marca do veiculo n�o pode ficar embranco");
    }

    if (veiculoDTO.Ano < 1950)
    {
        validacoes.Mensagem.Add("Veiculo muito antigo, aceitamos somente anos superiores a 1950");
    }

    return validacoes;
}

app.MapPost("/veiculos", ([FromBody] VeiculoDTO veiculoDTO, IVeiculosServico veiculosServico) =>
{
    var validacoes = validaDTO(veiculoDTO);

    if(validacoes.Mensagem.Count > 0)
    {
        return Results.BadRequest(validacoes);
    }


    var veiculo = new Veiculo
    {
        Nome = veiculoDTO.Nome,
        Marca = veiculoDTO.Marca,
        Ano = veiculoDTO.Ano
    };

    veiculosServico.Incluir(veiculo);

    return Results.Created($"/veiculo/{veiculo.Id}", veiculo);
}).WithTags("Veiculos");


app.MapGet("/veiculos", ([FromQuery]int? pagina, IVeiculosServico veiculosServico) =>
{
    var veiculos = veiculosServico.Todos(pagina);

    return Results.Ok(veiculos);
}).WithTags("Veiculos");


app.MapGet("/veiculos/{id}", ([FromRoute] int id, IVeiculosServico veiculosServico) =>
{
    var veiculos = veiculosServico.BuscaPorId(id);

    if(veiculos == null)
    {
        return Results.NotFound();
    }
   
    return Results.Ok(veiculos);

}).WithTags("Veiculos");


app.MapPut("/veiculos/{id}", ([FromRoute] int id, VeiculoDTO veiculoDTO, IVeiculosServico veiculosServico) =>
{

    var veiculos = veiculosServico.BuscaPorId(id);

    if (veiculos == null)
    {
        return Results.NotFound();
    }

    var validacoes = validaDTO(veiculoDTO);

    if (validacoes.Mensagem.Count > 0)
    {
        return Results.BadRequest(validacoes);
    }   

    veiculos.Nome = veiculoDTO.Nome;
    veiculos.Marca = veiculoDTO.Marca;
    veiculos.Ano = veiculoDTO.Ano;

    veiculosServico.Atualizar(veiculos);

    return Results.Ok(veiculos);

}).WithTags("Veiculos");


app.MapDelete("/veiculos/{id}", ([FromRoute] int id, IVeiculosServico veiculosServico) =>
{
    var veiculos = veiculosServico.BuscaPorId(id);

    if (veiculos == null)
    {
        return Results.NotFound();
    }

  
    veiculosServico.Apagar(veiculos);

    return Results.NoContent();

}).WithTags("Veiculos");
#endregion

#region App

app.UseSwagger();
app.UseSwaggerUI();

app.Run();
#endregion


